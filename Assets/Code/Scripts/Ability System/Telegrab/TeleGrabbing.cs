using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleGrabbing : MonoBehaviour, IAbility
{
    public static TeleGrabbing instance;

    public bool isTelegrabActivated;
    public bool isObjectGrabbed;
    [SerializeField] private Material materialForGrabbedObject;
    [SerializeField] private GameObject telegrabUI;
    [SerializeField] private AudioSource telegrabAudio;
    [SerializeField] private Light telegrabGlowPrefab;

    private float viewDistance = 50f;
    private Transform grabbedObjectPosition;
    private GameObject grabbedGameObject;
    private Light telegrabGlow;

    //note: remove this script and use a dictionary somehow instead cus extra scripts = stinky
    //private List<TelegrabObject> telegrabObjects = new List<TelegrabObject>();
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("There is more than one TeleGrabbing ability.");
            Destroy(gameObject);
            return;
        }
        else
        instance = this;
    }
    private void Start()
    {
        AbilityManager.instance.RegisterAbility(Abilities.TeleGrabbing, this);
        telegrabGlow = Instantiate(telegrabGlowPrefab);
    }
    private void Update()
    {
        if (FoxMovement.instance != null)
        {
            grabbedObjectPosition = GameObject.FindGameObjectWithTag("Grabbed").transform;
        }
    }

    public void Activate()
    {
        if (isTelegrabActivated)
        {
            Telegrab();
        }
    }

    private void Telegrab()
    {
        if (!isObjectGrabbed)
        {
            if (Physics.Raycast(FoxMovement.instance.cameraPosition.position, FoxMovement.instance.cameraPosition.forward, out RaycastHit hit, viewDistance, FoxMovement.instance.moveableLayerMask) && !isObjectGrabbed)
            {
                GrabObject(hit);
            }
        }
        else
        {
            DropObject();
        }
    }

    private void GrabObject(RaycastHit hit)
    {
            grabbedGameObject = hit.transform.gameObject;

            //if (grabbedGameObject.transform.childCount != 0)
            //{
            //    List<GameObject> childrenOfObject = new List<GameObject>();
            //    for (int i = 0; i < grabbedGameObject.transform.childCount; i++)
            //    {
            //        childrenOfObject.Add(grabbedGameObject.transform.GetChild(i).gameObject);
            //        telegrabObjects.Add(grabbedGameObject.transform.GetChild(i).GetComponent<TelegrabObject>());
            //    }

            //    foreach (GameObject child in childrenOfObject)
            //    {
            //        child.GetComponent<MeshRenderer>().material = materialForGrabbedObject;
            //        grabbedGameObject.GetComponent<MeshRenderer>().material = materialForGrabbedObject;
            //    }
            //}

            grabbedGameObject.transform.parent = grabbedObjectPosition;
            grabbedGameObject.transform.position = grabbedObjectPosition.position;
            grabbedGameObject.transform.GetComponent<Rigidbody>().isKinematic = true;
            isObjectGrabbed = true;

            StartCoroutine(TelegrabVFXControl());
            telegrabAudio.Play();
    }

    private void DropObject()
    {
        //foreach (TelegrabObject telegrabObject in telegrabObjects)
        //{
        //    telegrabObject.gameObject.GetComponent<MeshRenderer>().material = telegrabObject.telegrabMaterial;
        //}

        FoxMovement.instance.telegrabEffect.SendEvent("Sparks");
        grabbedGameObject.transform.parent = null;
        grabbedGameObject.transform.GetComponent<Rigidbody>().isKinematic = false;
        isObjectGrabbed = false;
        //telegrabObjects.Clear();
    }

    public void ActivateTelegrabCamera()
    {

        if (!isTelegrabActivated)
        {
            FoxMovement.instance.cameraMovement.freeLookCam.SetActive(false);
            FoxMovement.instance.cameraMovement.telegrabCam.SetActive(true);
            FoxMovement.instance.cameraMovement.currentStyle = CameraMovement.CameraStyle.Telegrab;
            StartCoroutine(CrosshairEnable());
        }
        else
        {
            FoxMovement.instance.cameraMovement.freeLookCam.SetActive(true);
            FoxMovement.instance.cameraMovement.telegrabCam.SetActive(false);
            FoxMovement.instance.cameraMovement.currentStyle = CameraMovement.CameraStyle.Basic;
            StartCoroutine(CrosshairDisable());
        }

        IEnumerator CrosshairEnable()
        {
            yield return new WaitForSeconds(0.2f);
            telegrabUI.SetActive(true);
        }

        IEnumerator CrosshairDisable()
        {
            yield return new WaitForSeconds(0.2f);
            telegrabUI.SetActive(false);
        }
    }

    IEnumerator TelegrabVFXControl()
    {
        telegrabGlow.gameObject.SetActive(true);
        telegrabGlow.transform.SetParent(grabbedGameObject.transform);
        telegrabGlow.transform.localPosition = Vector3.zero;
        FoxMovement.instance.telegrabEffect.SetInt("SpawnMultiplier", 1);
        Vector3 boundsSize = grabbedGameObject.GetComponent<MeshFilter>().sharedMesh.bounds.size;
        FoxMovement.instance.telegrabEffect.SetVector3("TargetBounds", boundsSize);
        float noiseX = UnityEngine.Random.Range(0, 100f);
        float noiseY = UnityEngine.Random.Range(0, 100f);
        float targetIntensity = (boundsSize.x + boundsSize.y + boundsSize.z) / 6;
        while (telegrabGlow.intensity < targetIntensity + targetIntensity * Mathf.PerlinNoise(noiseX, noiseY))
        {
            if (!isObjectGrabbed)
                break;
            FoxMovement.instance.telegrabEffect.SetVector3("TargetPos", grabbedGameObject.transform.position);
            telegrabGlow.intensity += Time.deltaTime;
            yield return null;
        }
        while (true)
        {
            if (!isObjectGrabbed)
                break;
            FoxMovement.instance.telegrabEffect.SetVector3("TargetPos", grabbedGameObject.transform.position);
            noiseX += Time.deltaTime * .01f;
            telegrabGlow.intensity = targetIntensity + targetIntensity * Mathf.PerlinNoise(noiseX, noiseY);
            yield return null;
        }
        FoxMovement.instance.telegrabEffect.SetInt("SpawnMultiplier", 0);
        while (telegrabGlow.intensity > 0)
        {
            if (isObjectGrabbed)
                break;
            FoxMovement.instance.telegrabEffect.SetVector3("TargetPos", grabbedGameObject.transform.position);
            telegrabGlow.intensity -= Time.deltaTime;
            yield return null;
        }
        telegrabGlow.transform.SetParent(null);
        telegrabGlow.gameObject.SetActive(false);
    }
}
