using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StageDefinition))]
public class StageDefinitionEditor : Editor
{
    private StageDefinition _pattern;

    #region Monobehaviour

    private void OnEnable()
    {
        _pattern = (StageDefinition)target;

        if (_pattern.Grid.Column.Count != _pattern.GridWidth || _pattern.Grid.Column[0].Row.Count != _pattern.GridHeight)
        {
            _pattern.Init();
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("GridWidth"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("GridHeight"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Spacing"));
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Generate Grid"))
        {
            _pattern.Init();
            EditorUtility.SetDirty(_pattern);
        }
        EditorGUILayout.Space();

        //Draw the grid
        for (int y = _pattern.GridHeight - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < _pattern.GridWidth; x++)
            {
                bool newValue = GUILayout.Toggle(_pattern.Grid[x, y], GUIContent.none, GUILayout.Width(25), GUILayout.Height(25));
                if (newValue != _pattern.Grid[x, y])
                {
                    _pattern.Grid[x, y] = newValue;
                    EditorUtility.SetDirty(_pattern);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(_pattern);
        }

        serializedObject.ApplyModifiedProperties();
    }

    #endregion
}