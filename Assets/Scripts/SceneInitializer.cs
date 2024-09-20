using System.Collections.Generic;
using Configs;
using Interfaces;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] private GameUpdater _gameUpdater;
    [SerializeField] private SettingsData _settingsData;
    [SerializeField] private TimeWindow _timeWindow;
    
    public TimeDataManager TimeDataManager { get; private set; }
    public TimeController TimeController { get; private set; }

    private readonly List<IDestroyable> _destroyables = new();

    public void Initialize()
    {
        var asyncTaskController = new AsyncTaskController();
        var mainThreadDispatcher = new MainThreadDispatcher();
        TimeController = new TimeController();
        TimeDataManager = new TimeDataManager(_settingsData, asyncTaskController, mainThreadDispatcher);

        _destroyables.Add(asyncTaskController);
        _destroyables.Add(mainThreadDispatcher);
        
        _gameUpdater.AddListener(mainThreadDispatcher);
        _gameUpdater.AddListener(_timeWindow);
    }

    private void OnDestroy()
    {
        foreach (var destroyable in _destroyables)
        {
            destroyable.OnDestroy();
        }
        _destroyables.Clear();
        
        _gameUpdater.RemoveAll();
    }
}
