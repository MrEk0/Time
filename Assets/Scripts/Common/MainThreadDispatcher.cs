using System;
using System.Collections.Generic;
using Interfaces;

namespace Common
{
    public class MainThreadDispatcher : ISceneUpdatable, IDisposable
    {
        private readonly Queue<Action> _executions = new();

        public void OnUpdate(float deltaTime)
        {
            lock (_executions)
            {
                while (_executions.Count > 0)
                    _executions.Dequeue().Invoke();
            }
        }

        public void Dispose()
        {
            lock (_executions)
            {
                _executions.Clear();
            }
        }

        public void Process(Action action)
        {
            lock (_executions)
            {
                _executions.Enqueue(action);
            }
        }
    }
}
