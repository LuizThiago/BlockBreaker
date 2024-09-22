using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Stage Definition", fileName = "new StageDefinition")]
public class StageDefinition : ScriptableObject
{
    public int GridWidth = 10;
    public int GridHeight = 5;
    public float Spacing = 1.5f;
    public DestructibleDefiniton DestructibleDefinition;
    public StageMatrix Grid = new StageMatrix();

    #region Monobehaviour

    private void OnValidate()
    {
        if (Grid.Column.Count != GridWidth || Grid.Column[0].Row.Count != GridHeight)
        {
            Init();
        }
    }

    #endregion

    #region Public

    public void Init()
    {
        Grid.Column.Clear();

        for (int x = 0; x < GridWidth; x++)
        {
            StageArray newArray = new();

            for (int y = 0; y < GridHeight; y++)
            {
                newArray.Row.Add(0); 
            }

            Grid.Column.Add(newArray);
        }
    }

    #endregion
}

[System.Serializable]
public class StageMatrix
{
    public List<StageArray> Column = new();

    public int this[int x, int y]
    {
        get => Column[x][y]; 
        set => Column[x][y] = value;
    }
}

[System.Serializable]
public class StageArray
{
    public List<int> Row = new();

    public int this[int index]
    {
        get => Row[index];
        set => Row[index] = value;
    }
}