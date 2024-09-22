using System;
using Windows;
using Common;
using Configs;
using JetBrains.Annotations;
using UnityEngine;

namespace Scene
{
    public class SceneInitializer : MonoBehaviour
    {
        [SerializeField] private SceneUpdater _sceneUpdater;
        [SerializeField] private SettingsData _settingsData;
        [SerializeField] private TimeWindow _timeWindow;
        [SerializeField] private ObjectEditActivator[] _objectEditActivators = Array.Empty<ObjectEditActivator>();
        [SerializeField] private ObjectConfirmDeactivator[] _objectConfirmDeactivators = Array.Empty<ObjectConfirmDeactivator>();
        
        [CanBeNull] private SceneListener _sceneListener;

        public ServiceLocator ServiceLocator { get; private set; }

        public void Initialize()
        {
            ServiceLocator = new ServiceLocator();
            _sceneListener = new SceneListener();
            ServiceLocator.AddService(_sceneListener);
            
            foreach (var editActivator in _objectEditActivators)
                _sceneListener.AddListener(editActivator);

            foreach (var confirmDeactivator in _objectConfirmDeactivators)
                _sceneListener.AddListener(confirmDeactivator);

            _sceneListener.AddListener(_sceneUpdater);

            var asyncTaskController = new AsyncTaskController();
            ServiceLocator.AddService(asyncTaskController);

            var mainThreadDispatcher = new MainThreadDispatcher();
            ServiceLocator.AddService(mainThreadDispatcher);

            ServiceLocator.AddService(_settingsData);
            ServiceLocator.AddService(_timeWindow);

            var timeController = new TimeController();
            ServiceLocator.AddService(timeController);

            var timeDataManager = new TimeDataManager(ServiceLocator);
            ServiceLocator.AddService(timeDataManager);

            var timeWindowController = new TimeWindowController(ServiceLocator);

            _sceneUpdater.AddListener(mainThreadDispatcher);
            _sceneUpdater.AddListener(timeWindowController);
            _sceneUpdater.AddListener(timeDataManager);
        }

        private void OnDestroy()
        {
            if (_sceneListener == null)
                return;
            
            _sceneListener.RemoveAll();
            _sceneUpdater.RemoveAll();
            ServiceLocator.RemoveAll();
        }
    }
}
