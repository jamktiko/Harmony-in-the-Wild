using UnityEditor;
using UnityEngine;

public class BF_TerrainEdit : Editor
{
    [CustomEditor(typeof(BF_SnowTerrain))]
    private class DecalMeshHelperEditor : Editor
    {
        private GUIStyle style;
        public override void OnInspectorGUI()
        {
            BF_SnowTerrain myTarget = (BF_SnowTerrain)target;
            myTarget.terrainToCopy = EditorGUILayout.ObjectField("Terrain To Copy (Data Override)", myTarget.terrainToCopy, typeof(Terrain), true) as Terrain;
            myTarget.avoidCulling = EditorGUILayout.Toggle("Avoid Terrain Culling", myTarget.avoidCulling);
            if (style == null)
            {
                style = new GUIStyle(GUI.skin.button);
            }
            if (myTarget.terrainToCopy != null)
            {
                if (GUILayout.Button("Sync Terrain Data (Data Override)", style))
                {
                    myTarget.CopyTerrainData();
                    myTarget.MoveTerrainSync();
                    style.normal.background = Texture2D.linearGrayTexture;
                }
                if (GUILayout.Button("Revert Terrain Data"))
                {
                    myTarget.RevertTerrainData();
                    style.normal.background = Texture2D.whiteTexture;
                }
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
}
