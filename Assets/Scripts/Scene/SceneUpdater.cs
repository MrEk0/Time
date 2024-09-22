using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Scene
{
    public class SceneUpdater : MonoBehaviour, IEditListener, IConfirmListener
    {
        private readonly List<ISceneUpdatable> _updateListeners = new();

        private bool _isActive = true;

        public void AddListener(ISceneUpdatable listener)
        {
            _updateListeners.Add(listener);
        }

        public void RemoveListener(ISceneUpdatable listener)
        {
            _updateListeners.Remove(listener);
        }

        public void RemoveAll()
        {
            _updateListeners.Clear();
        }

        private void Update()
        {
            if (!_isActive)
                return;

            var deltaTime = Time.deltaTime;
            for (var i = 0; i < _updateListeners.Count; i++)
            {
                _updateListeners[i].OnUpdate(deltaTime);
            }
        }

        public void Edit()
        {
            _isActive = false;
        }

        public void Confirm()
        {
            _isActive = true;
        }
    }
}

