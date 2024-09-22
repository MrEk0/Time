using System;
using System.Collections;
using Configs;
using UnityEngine;

namespace Scene
{
    public class SceneLauncher : MonoBehaviour
    {
        [SerializeField] private SceneInitializer _sceneInitializer;

        private IEnumerator Start()
        {
            _sceneInitializer.Initialize();

            var isComplete = false;

            LoadTime(() => { isComplete = true; });

            while (!isComplete)
                yield return new WaitForEndOfFrame();
        }

        private void LoadTime(Action onComplete)
        {
            var timeDataManager = _sceneInitializer.ServiceLocator.GetService<TimeDataManager>();
            var timeController = _sceneInitializer.ServiceLocator.GetService<TimeController>();
            var data = _sceneInitializer.ServiceLocator.GetService<SettingsData>();

            timeDataManager.RequestTime(data.ServerURL, dateTime =>
            {
                timeController.SetTimeUtc(dateTime);

                onComplete();
            });
        }
    }
}
