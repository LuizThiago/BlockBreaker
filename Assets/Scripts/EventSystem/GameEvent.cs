using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Others/GameEvent", fileName = "new GameEvent")]
public class GameEvent : ScriptableObject
{
    private readonly List<Action<Component, object>> _responses = new();

    #region Public

    public void Raise(Component sender, object arg)
    {
        for (int i = 0; i < _responses.Count; i++)
        {
            _responses[i]?.Invoke(sender, arg);
        }
    }

    public void RegisterResponse(Action<Component, object> response)
    {
        _responses.Add(response);
    }

    public void UnRegisterResponse(Action<Component, object> listener)
    {
        if (_responses.Contains(listener))
        {
            _responses.Remove(listener);
        }
    }

    #endregion
}