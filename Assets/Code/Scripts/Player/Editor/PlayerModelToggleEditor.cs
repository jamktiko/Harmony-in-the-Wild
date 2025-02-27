using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerModelToggle))]
public class PlayerModelToggleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(Application.isPlaying)
        {
            DrawToggleButtons();
        }
    }

    private void DrawToggleButtons()
    {
        PlayerModelToggle playerModelToggle = (PlayerModelToggle)target;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Switch to Red fox"))
        {
            playerModelToggle.TogglePlayerModelPublic(1);
        }
        if (GUILayout.Button("Switch to Arctic fox"))
        {
            playerModelToggle.TogglePlayerModelPublic(2);
        }
        EditorGUILayout.EndHorizontal();
    }
}
