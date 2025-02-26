//
// DrawSceneObjectEditor.cs
//
// Dynamic Shadow Projector
//
// Copyright 2015 NYAHOON GAMES PTE. LTD. All Rights Reserved.
//

using UnityEditor;
using UnityEngine;

namespace DynamicShadowProjector.Editor
{
    [CustomEditor(typeof(DrawSceneObject))]
    public class DrawSceneObjectEditor : EditorBase
    {
        void OnEnable()
        {
            DrawSceneObject component = target as DrawSceneObject;
            if (component.replacementShader == null)
            {
                component.replacementShader = Shader.Find("Hidden/DynamicShadowProjector/Shadow/Replacement");
                serializedObject.Update();
                EditorUtility.SetDirty(component);
            }
        }
    }
}
