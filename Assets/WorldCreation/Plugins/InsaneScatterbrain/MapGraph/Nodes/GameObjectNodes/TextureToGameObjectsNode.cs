using System;
using System.Collections;
using System.Collections.Generic;
using InsaneScatterbrain.Extensions;
using InsaneScatterbrain.ScriptGraph;
using InsaneScatterbrain.Services;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InsaneScatterbrain.MapGraph
{
    /// <summary>
    /// Generates the GameObjects from the prefab set, based on the texture.
    /// </summary>
    [ScriptNode("Texture To GameObjects", "GameObjects"), Serializable]
    public class TextureToGameObjectsNode : ProcessorNode
    {
        [InPort("Texture", typeof(TextureData), true), SerializeField] 
        private InPort textureIn = null;
        
        [InPort("Prefab Set", typeof(PrefabSet), true), SerializeField] 
        private InPort prefabSetIn = null;
        
        
        [OutPort("GameObjects", typeof(GameObject[])), SerializeReference] 
        private OutPort gameObjectsOut = null;

        protected override IEnumerator OnProcessMainThreadCoroutine()
        {
            var instanceProvider = Get<IInstanceProvider>();
            var namedColorSet = Get<NamedColorSet>();
            var rng = Get<Rng>();
            
            var textureData = textureIn.Get<TextureData>();
            var prefabSet = prefabSetIn.Get<PrefabSet>();
            var prefabWidth = prefabSet.PrefabWidth;
            var prefabHeight = prefabSet.PrefabHeight;
            
            var width = textureData.Width;
            
            var gameObjects = instanceProvider.Get<List<GameObject>>();
            gameObjects.EnsureCapacity(textureData.ColorCount);
            
            for (var i = 0; i < textureData.ColorCount; ++i)
            {
                var color = textureData[i];
                if (color == new Color(0, 0, 0, 0))
                {
                    yield return null;
                    continue;    // Empty space, skip it
                }
            
                var prefabTypeName = namedColorSet.GetName(color); 
                if (prefabTypeName == null)
                {
                    Debug.LogError($"Unknown color: {color}");
                }

                var x = i % width * prefabWidth;
                var y = i / width * prefabHeight;

                var prefab = prefabSet.GetRandomObject(prefabTypeName, rng);
                var instance = Object.Instantiate(prefab, 
                    new Vector3(x, y, 0) + prefab.transform.position, 
                    prefab.transform.rotation);
            
                instance.name = prefab.name;
                ScriptGraphComponents.RegisterTemporaryObject(instance);

                gameObjects.Add(instance);

                yield return null;
            }

            gameObjectsOut.Set(() => gameObjects.ToArray());
        }
    }
}