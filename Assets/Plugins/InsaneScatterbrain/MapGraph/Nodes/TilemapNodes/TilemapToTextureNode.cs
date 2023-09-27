using System;
using InsaneScatterbrain.ScriptGraph;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InsaneScatterbrain.MapGraph
{
    /// <summary>
    /// Generates a texture from a tilemap. Tile names are used to lookup the associated color from the graph's named
    /// color set.
    /// </summary>
    [ScriptNode("Tilemap To Texture", "Tilemaps"), Serializable]
    public class TilemapToTextureNode : ProcessorNode
    {
        [InPort("Tilemap", typeof(Tilemap), true), SerializeReference]
        private InPort tilemapIn = null;
        
        [InPort("Tileset", typeof(Tileset), true), SerializeReference]
        private InPort tilesetIn = null;
        
        
        [OutPort("Texture", typeof(TextureData)), SerializeReference] 
        private OutPort textureOut = null;
        

        private TextureData textureData;
        
#if UNITY_EDITOR
        /// <summary>
        /// Gets the latest generated texture data. Only available in the editor.
        /// </summary>
        public TextureData TextureData => textureData;
#endif

        /// <inheritdoc cref="ProcessorNode.OnProcess"/>
        protected override void OnProcess()
        {
            var instanceProvider = Get<IInstanceProvider>();
            var namedColorSet = Get<NamedColorSet>();
            
            var tileset = tilesetIn.Get<Tileset>();
            var tilemap = tilemapIn.Get<Tilemap>();

            tilemap.CompressBounds();
            
            var bounds = tilemap.cellBounds;

            var width = bounds.size.x;
            var height = bounds.size.y;

            textureData = instanceProvider.Get<TextureData>();
            textureData.Set(width, height);
            
            for (var x = bounds.xMin; x < bounds.xMax; ++x)
            {
                for (var y = bounds.yMin; y < bounds.yMax; ++y)
                {
                    var tile = tilemap.GetTile(new Vector3Int(x, y, 0));

                    Color32 color;
                    if (tile == null)
                    {
                        color = default;
                    }
                    else
                    {
                        var tileTypeName = tileset.GetTypeName(tile);
                        color = namedColorSet.GetColorByName(tileTypeName);
                    }

                    var i = (y - bounds.yMin) * width + (x - bounds.xMin);
                    textureData[i] = color;
                }
            }
            
            textureOut.Set(() => textureData);
        }
    }
}