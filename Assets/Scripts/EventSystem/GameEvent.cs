using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Others/GameEvent", fileName = "new GameEvent")]
public class GameEvent : ScriptableObject
{
    private readonly List<IGameEventListener> _listeners = new();

    #region Public

    public void Raise(Component sender, object arg)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(sender, arg);
        }
    }

    public void RegisterListener(IGameEventListener gameEventListener)
    {
        _listeners.Add(gameEventListener);
    }

    public void UnRegisterListener(IGameEventListener gameEventListener)
    {
        if (_listeners.Contains(gameEventListener))
        {
            _listeners.Remove(gameEventListener);
        }
    }

    #endregion
}