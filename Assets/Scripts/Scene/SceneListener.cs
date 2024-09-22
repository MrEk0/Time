using System.Collections.Generic;
using Interfaces;

namespace Scene
{
    public class SceneListener
    {
        private readonly List<object> _listeners = new();

        public void AddListener(object listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(object listener)
        {
            _listeners.Remove(listener);
        }

        public void Edit()
        {
            foreach (var listener in _listeners)
            {
                if (listener is IEditListener editListener)
                {
                    editListener.Edit();
                }
            }
        }

        public void Confirm()
        {
            foreach (var listener in _listeners)
            {
                if (listener is IConfirmListener confirmListener)
                {
                    confirmListener.Confirm();
                }
            }
        }

        public void RemoveAll()
        {
            _listeners.Clear();
        }
    }
}
