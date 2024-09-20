using System;
using System.Collections.Generic;
using Interfaces;

public class MainThreadDispatcher : IGameUpdatable, IDestroyable
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

    public void OnDestroy()
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
