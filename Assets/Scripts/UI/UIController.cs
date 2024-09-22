using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EUIPanel { StartPanel, NextStagePanel, EndStagePanel }

public class UIController : MonoBehaviour
{
    [SerializeField] private List<UIPanel> _panels;
    [Header("Events")]

    [SerializeField] private GameEvent _playerStartedEvent;
    [SerializeField] private GameEvent _gameStartEvent;
    [SerializeField] private GameEvent _gameEndEvent;

    #region Monobehaviours

    private void OnEnable()
    {
        _playerStartedEvent.RegisterResponse(OnPlayerStartEvent);
        _gameStartEvent.RegisterResponse(OnGameStartEvent);
        _gameEndEvent.RegisterResponse(OnGameEndEvent);
    }

    private void Start()
    {
        ShowPanel(EUIPanel.StartPanel);
    }

    private void OnDisable()
    {
        _playerStartedEvent.UnRegisterResponse(OnPlayerStartEvent);
        _gameStartEvent.UnRegisterResponse(OnGameStartEvent);
        _gameEndEvent.RegisterResponse(OnGameEndEvent);
    }

    #endregion

    #region Public

    public void ShowPanel(EUIPanel panelType, string labelText = null, bool hideOthers = true)
    {
        foreach (var panel in _panels)
        {
            if (hideOthers)
            {
                if (panel.Type == panelType && labelText != null)
                {
                    panel.SetTextLabel(labelText);
                }

                panel.SetActive(panel.Type == panelType);
            }
            else if (panel.Type == panelType)
            {
                if (labelText != null)
                {
                    panel.SetTextLabel(labelText);
                }

                panel.SetActive(true);
            }
        }
    }

    public void HidePanel(EUIPanel panelType)
    {
        foreach (var panel in _panels)
        {
            if (panel.Type == panelType)
            {
                panel.SetActive(false);
            }
        }
    }

    public void HideAllPanels()
    {
        foreach (var panel in _panels)
        {
            panel.SetActive(false);
        }
    }

    #endregion

    #region Private

    private void OnGameStartEvent(Component sender, object arg)
    {
        HideAllPanels();
    }

    private void OnPlayerStartEvent(Component sender, object arg)
    {
        if (arg is string stageName)
        {
            ShowPanel(EUIPanel.NextStagePanel, stageName);
        }
    }

    private void OnGameEndEvent(Component sender, object arg)
    {
        if (arg is null)
        {
            ShowPanel(EUIPanel.EndStagePanel);
        }
    }

    #endregion

    [System.Serializable]
    private class UIPanel
    {
        [field: SerializeField] public EUIPanel Type { get; private set; }
        [field: SerializeField] public GameObject Panel { get; private set; }
        [SerializeField] private TMP_Text _label;
        
        public void SetActive (bool isActive)
        {
            Panel.SetActive (isActive);
        }

        public void SetTextLabel (string text)
        {
            if (_label == null) { return; }

            _label.SetText (text);
        }
    }
}
