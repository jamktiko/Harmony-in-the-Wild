using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BF_PlayerSnow : MonoBehaviour
{
    public Collider playerCollider;
    public ParticleSystem particleSys;

    public PhysicMaterial playerMatDefault;
    public PhysicMaterial playerMatSnow;
    public PhysicMaterial playerMatIce;

    private Rigidbody rB;
    private float speedMult = 1;
    private float lerpIce = 0;
    private MeshCollider oldMC = null;
    private Mesh mesh = null;
    private ParticleSystem.MainModule pSMain;


    // Start is called before the first frame update
    void Start()
    {
        oldMC = null;
        mesh = null;
        rB = this.GetComponent<Rigidbody>();
        pSMain = particleSys.main;
    }

    private void CheckIceCols(float snowCol)
    {
        lerpIce = snowCol / 255f;

        if (snowCol == -1)
        {
            if (playerCollider.material != playerMatDefault)
            {
                playerCollider.material = playerMatDefault;
            }
            return;
        }


        if (lerpIce <= 0.925f && playerCollider.material != playerMatIce)
        {
            playerCollider.material = playerMatIce;
            rB.angularDrag = 0.25f;
        }
        else if(lerpIce >= 0.925f && playerCollider.material != playerMatSnow)
        {
            playerCollider.material = playerMatSnow;
            rB.angularDrag = 5f;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (lerpIce >= 0.925f && collision.collider.gameObject.layer == 4)
        {
            AddSnow(1.5f);
        }
        else
        {
            RemoveSnow(0.05f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.impulse.magnitude>10)
        {
           // RemoveSnow(20);
        }
    }

    private void AddSnow(float multiplier)
    {
        if (playerCollider.transform.localScale.x < 7f)
        {
            speedMult = Mathf.Clamp(rB.velocity.magnitude * 0.02f,0,1);
            playerCollider.transform.localScale += Vector3.zero + Vector3.one * 0.0035f * 2 * multiplier* speedMult;
            playerCollider.transform.localScale += Vector3.zero + Vector3.one * 0.005f * 2 * multiplier * speedMult;
        }
    }
    private void RemoveSnow(float multiplier)
    {
        if (playerCollider.transform.localScale.x >= 1.1f)
        {
            if (!playerCollider.transform.gameObject.activeInHierarchy)
            {
               // SnowPlayer.gameObject.SetActive(true);
            }
            playerCollider.transform.localScale -= Vector3.zero + Vector3.one * 0.0035f * 4 * multiplier;
            playerCollider.transform.localScale -= Vector3.zero + Vector3.one * 0.005f * 4 * multiplier;
        }
        if (playerCollider.transform.localScale.x < 1.1f)
        {
            if (playerCollider.transform.gameObject.activeInHierarchy)
            {
               // SnowPlayer.gameObject.SetActive(false);
            }
            playerCollider.transform.localScale = Vector3.one * 1.1f;
            playerCollider.transform.localScale = Vector3.one * 1.1f;
        }
    }

    private void FixedUpdate()
    {
        ChangePlayerMass();
        CheckSnowUnderneath();
    }

    private void CheckSnowUnderneath()
    {
        RaycastHit hit;

        int layerMask = 1 << 4;
        if (Physics.Raycast(transform.position+(Vector3.down*(playerCollider.transform.localScale.x/2)+Vector3.up*0.5f), Vector3.down, out hit, 5, layerMask,QueryTriggerInteraction.Ignore))
        {
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (oldMC != meshCollider || mesh == null)
            {
                mesh = meshCollider.GetComponent<MeshFilter>().sharedMesh;
            }
            oldMC = meshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
            {
                CheckIceCols(255f);
                return;
            }

            //Mesh mesh = meshCollider.sharedMesh;

            int[] triangles = mesh.triangles;
            Color32[] colorArray;
            colorArray = mesh.colors32;

            var vertIndex1 = triangles[hit.triangleIndex * 3 + 0];
            CheckIceCols(((float)colorArray[vertIndex1].g) / 1);
        }
        else
        {
            if (playerCollider.material != playerMatDefault)
            {
                playerCollider.material = playerMatDefault;
            }
        }
    }

    private void ChangePlayerMass()
    {
        rB.mass = Mathf.Lerp(1.95f, 2.5f, (playerCollider.transform.localScale.x-1.2f) / 7);
        pSMain.startSize = playerCollider.transform.localScale.x+0.5f;
    }
}
