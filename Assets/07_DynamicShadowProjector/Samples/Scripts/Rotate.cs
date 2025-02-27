using UnityEngine;

namespace DynamicShadowProjector.Sample
{
    public class Rotate : MonoBehaviour
    {
        public float m_rotateSpeed = 90.0f;

        private void Update()
        {
            transform.rotation = Quaternion.AngleAxis(m_rotateSpeed * Time.deltaTime, Vector3.up) * transform.rotation;
        }
    }
}
