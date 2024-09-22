using UnityEngine;

public interface IGameEventListener
{
    void OnEventRaised(Component sender, object arg);
}