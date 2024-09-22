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
        EditorGUILayout.PropertyField(serializedObject.FindProperty("DestructibleDefinition"));
        EditorGUILayout.Space();

        if (serializedObject.FindProperty("DestructibleDefinition").objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Please attach a DestructibleDefiniton", MessageType.Warning);
            return;
        }
        
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
                int currentHitPoints = _pattern.Grid[x, y];
                if (GUILayout.Button(currentHitPoints.ToString(), GUILayout.Width(25), GUILayout.Height(25)))
                {
                    _pattern.Grid[x, y] = (currentHitPoints + 1) % (_pattern.DestructibleDefinition.MaxHitPoints + 1);
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