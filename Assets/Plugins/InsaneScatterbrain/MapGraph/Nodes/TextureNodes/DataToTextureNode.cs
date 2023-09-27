using System;
using InsaneScatterbrain.ScriptGraph;
using UnityEngine;

namespace InsaneScatterbrain.MapGraph
{
    /// <summary>
    /// Converts texture data to texture object.
    /// </summary>
    [ScriptNode("Data To Texture", "Textures"), Serializable]
    public class DataToTextureNode : ProcessorNode
    {
        [InPort("Data", typeof(TextureData), true), SerializeReference]
        private InPort textureDataIn = null;

        [OutPort("Texture", typeof(Texture2D)), SerializeReference]
        private OutPort textureOut = null;

        /// <inheritdoc cref="ProcessorNode.OnProcess"/>
        protected override void OnProcess()
        {
            var textureData = textureDataIn.Get<TextureData>();
            var texture = textureData.ToTexture2D();
            textureOut.Set(() => texture);
        }
    }
}