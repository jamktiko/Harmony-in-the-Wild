//
// DrawSceneObject.cs
//
// Dynamic Shadow Projector
//
// Copyright 2015 NYAHOON GAMES PTE. LTD. All Rights Reserved.
//

using UnityEngine;

namespace DynamicShadowProjector
{
#if UNITY_2018_3_OR_NEWER
    [ExecuteAlways]
#else
	[ExecuteInEditMode]
#endif
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ShadowTextureRenderer))]
#pragma warning disable CS1692, RECS0001
    public partial class DrawSceneObject : MonoBehaviour
    {
#pragma warning restore // CS1692, RECS0001
        // serialize fields
        [SerializeField]
        private Shader m_replacementShader;
        [SerializeField]
        private LayerMask m_cullingMask;

        // public property
        public Shader replacementShader
        {
            get { return m_replacementShader; }
            set
            {
                m_replacementShader = value;
                shadowTextureRenderer.SetReplacementShader(m_replacementShader, "RenderType");
            }
        }
        public LayerMask cullingMask
        {
            get { return m_cullingMask; }
            set
            {
                m_cullingMask = value;
                if (shadowTextureRenderer.isProjectorVisible)
                {
                    shadowTextureRenderer.cameraCullingMask = value;
                }
            }
        }

        private ShadowTextureRenderer m_shadowTextureRenderer;
        public ShadowTextureRenderer shadowTextureRenderer
        {
            get
            {
                if (m_shadowTextureRenderer == null)
                {
                    m_shadowTextureRenderer = GetComponent<ShadowTextureRenderer>();
                }
                return m_shadowTextureRenderer;
            }
        }

        private void OnValidate()
        {
            shadowTextureRenderer.SetReplacementShader(m_replacementShader, "RenderType");
            if (shadowTextureRenderer.isProjectorVisible)
            {
                shadowTextureRenderer.cameraCullingMask = m_cullingMask;
            }
        }

        private void OnEnable()
        {
            shadowTextureRenderer.cameraCullingMask = m_cullingMask;
            shadowTextureRenderer.SetReplacementShader(m_replacementShader, "RenderType");
        }

        private void OnDisable()
        {
            shadowTextureRenderer.cameraCullingMask = 0;
            shadowTextureRenderer.SetReplacementShader(null, null);
        }

        private void OnVisibilityChanged(bool isVisible)
        {
            if (isVisible)
            {
                shadowTextureRenderer.cameraCullingMask = m_cullingMask;
            }
            else
            {
                shadowTextureRenderer.cameraCullingMask = 0;
            }
        }
    }
}
