using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConsoleLogController : MonoBehaviour
{
    [SerializeField] private ConsoleLogSettings _settings;
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _logText;
    [Header("Events")]
    [SerializeField] private GameEvent _hitEvent;
    [SerializeField] private GameEvent _projectileSpawnEvent;
    [SerializeField] private GameEvent _destructibleDestroyedEvent;
    [SerializeField] private GameEvent _playerStartedEvent;

    private readonly Queue<string> _messageQueue = new();

    #region Monobehaviour

    private void OnEnable()
    {
        _hitEvent.RegisterResponse(OnProjectileHitEvent);
        _projectileSpawnEvent.RegisterResponse(OnProjectileSpawnEvent);
        _playerStartedEvent.RegisterResponse(OnPlayerStartedEvent);
        _destructibleDestroyedEvent.RegisterResponse(OnBlockDestroyedEvent);
    }

    private void Start()
    {
        ClearLog();
    }

    private void OnDisable()
    {
        _hitEvent.UnRegisterResponse(OnProjectileHitEvent);
        _projectileSpawnEvent.UnRegisterResponse(OnProjectileSpawnEvent);
        _playerStartedEvent.UnRegisterResponse(OnPlayerStartedEvent);
        _destructibleDestroyedEvent.UnRegisterResponse(OnBlockDestroyedEvent);
    }

    #endregion

    #region Public

    public void LogMessage(string message)
    {
        if (_messageQueue.Count >= _settings.MaxMessages)
        {
            _messageQueue.Dequeue();
        }

        _messageQueue.Enqueue(message);
        UpdateLogText();
    }

    public void ClearLog()
    {
        _logText.text = "";
        _messageQueue.Clear();
    }

    #endregion

    #region Private

    private void UpdateLogText()
    {
        List<string> formattedMessages = new();
        int messageCount = _messageQueue.Count;
        int index = 0;

        foreach (string message in _messageQueue)
        {
            FormatMessage(formattedMessages, messageCount, index, message);
            index++;
        }

        _logText.text = string.Join("\n", formattedMessages);
    }

    private void FormatMessage(List<string> formattedMessages, int messageCount, int index, string message)
    {
        if (messageCount >= _settings.MaxMessages - 1)
        {
            //Apply a different color to the last lines
            if (index == messageCount - 1)
            {
                formattedMessages.Add($"<color=#696969>{message}</color>");
            }
            else if (index == messageCount - 2)
            {
                formattedMessages.Add($"<color=#A9A9A9>{message}</color>");
            }
            else
            {
                formattedMessages.Add(message);
            }
        }
        else
        {
            formattedMessages.Add(message);
        }
    }

    private void OnProjectileSpawnEvent(Component sender, object arg)
    {
        if (arg is Projectile projectile)
        {
            LogMessage($"<color={_settings.ShotLogColor}>{projectile.name} was shot</color>");
        }
    }

    private void OnProjectileHitEvent(Component sender, object arg)
    {
        if (sender is Projectile projectile && arg is Collider2D destructible)
        {
            var isWall = Utils.ContainsLayerMask(_settings.WallLayer, destructible.gameObject);
            if (isWall && !_settings.LogWallHits) { return; }

            LogMessage($"<color={_settings.HitLogColor}>{projectile.name} has hit {destructible.gameObject.name}</color>");
        }
    }

    private void OnBlockDestroyedEvent(Component sender, object arg)
    {
        if (sender is Destructible destructible)
        {
            LogMessage($"<color={_settings.DestroyLogColor}>{destructible.name} was destroyed</color>");
        }
    }

    private void OnPlayerStartedEvent(Component sender, object arg)
    {
        ClearLog();
    }

    #endregion
}
