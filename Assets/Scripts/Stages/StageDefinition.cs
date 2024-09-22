using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Stage Definition", fileName = "new StageDefinition")]
public class StageDefinition : ScriptableObject
{
    public int GridWidth = 10;
    public int GridHeight = 5;
    public float Spacing = 1.5f;
    public StageMatrix Grid = new StageMatrix();

    #region Monobehaviour

    private void OnValidate()
    {
        // Garante que o grid seja inicializado no momento correto no editor
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
            StageArray newArray = new StageArray();

            for (int y = 0; y < GridHeight; y++)
            {
                newArray.Row.Add(false); // Inicializa todas as células como "false"
            }
            Grid.Column.Add(newArray);
        }
    }

    #endregion
}

[System.Serializable]
public class StageMatrix
{
    public List<StageArray> Column = new List<StageArray>();
    public bool this[int x, int y]
    {
        get => Column[x][y];  // Getter: Acessa o valor no grid
        set => Column[x][y] = value;  // Setter: Atribui o valor no grid
    }
}

[System.Serializable]
public class StageArray
{
    public List<bool> Row = new List<bool>();
    public bool this[int index]
    {
        get => Row[index];  // Getter: Acessa o valor na linha
        set => Row[index] = value;  // Setter: Atribui o valor na linha
    }
}