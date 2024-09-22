using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageList
{
    [field: SerializeField] public List<StageDefinition> Stages { get; private set; }

    #region Properties

    public int CurrentStageIndex { get; private set; } = 0;
    public StageDefinition CurrentStage => CurrentStageIndex >= Stages.Count ? 
        null : Stages[CurrentStageIndex];
    public string CurrentStageName => $"Stage {CurrentStageIndex + 1}";

    #endregion

    #region Public

    public bool TryGetNextStage(out StageDefinition nextStage)
    {
        nextStage = null;
        CurrentStageIndex++;

        if (CurrentStageIndex >= Stages.Count) { return false; }

        nextStage = Stages[CurrentStageIndex];
        return true;
    }

    public void ResetList()
    {
        CurrentStageIndex = 0;
    }

    #endregion
}
