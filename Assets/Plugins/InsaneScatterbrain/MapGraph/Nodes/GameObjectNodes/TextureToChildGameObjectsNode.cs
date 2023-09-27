using System;
using System.Collections;
using InsaneScatterbrain.ScriptGraph;
using InsaneScatterbrain.Services;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InsaneScatterbrain.MapGraph
{
    [ScriptNode("Texture To Child GameObjects", "GameObjects"), Serializable]
    public class TextureToChildGameObjectsNode : ProcessorNode
    {
        [InPort("Texture", typeof(TextureData), true), SerializeField] 
        private InPort textureIn = null;
        
        [InPort("Prefab Set", typeof(PrefabSet), true), SerializeField] 
        private InPort prefabSetIn = null;
        
        [InPort("Parent", typeof(GameObject), true), SerializeField] 
        private InPort parentIn = null;

        [InPort("Use XZ Plane", typeof(bool)), SerializeField]
        private InPort useXzPlaneIn = null;
        
        protected override IEnumerator OnProcessMainThreadCoroutine()
        {
            var rng = Get<Rng>();
            
            var namedColorSet = Get<NamedColorSet>();
            
            var parent = parentIn.Get<GameObject>();
            var useXzPlane = useXzPlaneIn.Get<bool>();
            var textureData = textureIn.Get<TextureData>();
            
            var prefabSet = prefabSetIn.Get<PrefabSet>();
            var prefabWidth = prefabSet.PrefabWidth;
            var prefabHeight = prefabSet.PrefabHeight;

            var width = textureData.Width;

            // Remove any existing child game objects from the parent object.
            while (parent.transform.childCount > 0)
            {
                var child = parent.transform.GetChild(0).gameObject;
                Object.DestroyImmediate(child);
                
                yield return null;
            }
            
            for (var i = 0; i < textureData.ColorCount; ++i)
            {
                var color = textureData[i];
                if (color == new Color(0, 0, 0, 0)) continue;    // Empty space, skip it 
            
                var prefabTypeName = namedColorSet.GetName(color); 
                if (prefabTypeName == null)
                {
                    Debug.LogError($"Unknown color: {color}");
                }
            
                var x = i % width * prefabWidth;
                var y = i / width * prefabHeight;

                var position = useXzPlane ? new Vector3(x, 0, y) : new Vector3(x, y, 0);
            
                var prefab = prefabSet.GetRandomObject(prefabTypeName, rng);
                var instance = Object.Instantiate(prefab, parent.transform);
                instance.transform.position += position;
                
                yield return null;
            }
        }
    }
}