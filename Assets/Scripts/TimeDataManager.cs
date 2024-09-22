using System;
using Common;
using Configs;
using Interfaces;
using JetBrains.Annotations;
using Scene;
using UnityEngine;

public class TimeDataManager : ISceneUpdatable
{
    public event Action<DateTime> UpdateTimeEvent = delegate {  };
    
    [Serializable]
    private class TimeData
    {
        public string time = string.Empty;
        public string clocks = string.Empty;
    }
    
    [CanBeNull] private readonly AsyncTaskController _asyncTaskController;
    [CanBeNull] private readonly MainThreadDispatcher _mainThreadDispatcher;
    [CanBeNull] private readonly SettingsData _data;

    private float _timer;
    private readonly float _updateTime;

    public TimeDataManager(ServiceLocator serviceLocator)
    {
        _data = serviceLocator.GetService<SettingsData>();
        _asyncTaskController = serviceLocator.GetService<AsyncTaskController>();
        _mainThreadDispatcher = serviceLocator.GetService<MainThreadDispatcher>();

        if (_data == null)
            return;
        
        _updateTime = _data.ServerRequestUpdateTime;
    }
    
    public void OnUpdate(float deltaTime)
    {
        if (_data == null)
            return;
        
        _timer += deltaTime;
        if (_timer < _updateTime)
            return;

        _timer = 0f;

        RequestTime(_data.ServerURL, (time) =>
        {
            UpdateTimeEvent(time);
        });
    }

    public void RequestTime(string urlTime, Action<DateTime> onComplete)
    {
        if (_asyncTaskController == null || _mainThreadDispatcher == null)
        {
            onComplete(DateTime.UtcNow);
            return;
        }

        _asyncTaskController.Wait(WebRequestHelper.HttpGetAsync(urlTime), response =>
        {
            _mainThreadDispatcher.Process(() =>
            {
                var data = Deserializer.DeserializeFromJson(response, new TimeData());

                onComplete(DateTimeOffset.TryParse(data?.time, out var result) ? result.UtcDateTime : DateTime.UtcNow);
            });
        }, error =>
        {
            _mainThreadDispatcher.Process(() =>
            {
                Debug.LogError($" {error}");

                onComplete(DateTime.UtcNow);
            });
        });
    }
}
