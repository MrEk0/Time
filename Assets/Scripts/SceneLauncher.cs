using System;
using System.Collections;
using Configs;
using UnityEngine;

public class SceneLauncher : MonoBehaviour
{
    [SerializeField] private SettingsData _settingsData;
    [SerializeField] private SceneInitializer _sceneInitializer;
    [SerializeField] private TimeWindow _timeWindow;

    private IEnumerator Start()
    {
        _sceneInitializer.Initialize();
        
        var isComplete = false;

        LoadTime(() =>
        {
            isComplete = true;
        });

        while (!isComplete)
            yield return new WaitForEndOfFrame();

        _timeWindow.Init(_sceneInitializer.TimeDataManager, _sceneInitializer.TimeController);
    }

    private void LoadTime(Action onComplete)
    {
        _sceneInitializer.TimeDataManager.RequestTime(_settingsData.ServerURL, dateTime =>
        {
            _sceneInitializer.TimeController.SetTimeUtc(dateTime);

            onComplete();
        });
    }
}
