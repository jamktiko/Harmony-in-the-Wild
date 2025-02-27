using System.Collections;
using UnityEngine;

public class TeleGrabbing : MonoBehaviour, IAbility
{
    public static TeleGrabbing Instance;

    public bool IsTelegrabActivated;
    public bool IsObjectGrabbed;
    [SerializeField] private Material _materialForGrabbedObject;
    [SerializeField] private GameObject _telegrabUI;
    [SerializeField] private AudioSource _telegrabAudio;
    [SerializeField] private Light _telegrabGlowPrefab;

    private float _viewDistance = 50f;
    private Transform _grabbedObjectPosition;
    private GameObject _grabbedGameObject;
    private Light _telegrabGlow;

    //note: remove this script and use a dictionary somehow instead cus extra scripts = stinky
    //private List<TelegrabObject> telegrabObjects = new List<TelegrabObject>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more than one TeleGrabbing ability.");
            Destroy(gameObject);
            return;
        }
        else
            Instance = this;
    }
    private void Start()
    {
        AbilityManager.Instance.RegisterAbility(Abilities.TeleGrabbing, this);
        _telegrabGlow = Instantiate(_telegrabGlowPrefab);
        _telegrabGlow.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (FoxMovement.Instance != null)
        {
            _grabbedObjectPosition = GameObject.FindGameObjectWithTag("Grabbed").transform;
        }
    }

    public void Activate()
    {
        if (IsTelegrabActivated)
        {
            Telegrab();
        }
    }

    private void Telegrab()
    {
        if (!IsObjectGrabbed)
        {
            if (Physics.Raycast(FoxMovement.Instance.CameraPosition.position, FoxMovement.Instance.CameraPosition.forward, out RaycastHit hit, _viewDistance, FoxMovement.Instance.MoveableLayerMask) && !IsObjectGrabbed)
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
        _grabbedGameObject = hit.transform.gameObject;

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

        _grabbedGameObject.transform.parent = _grabbedObjectPosition;
        _grabbedGameObject.transform.position = _grabbedObjectPosition.position;
        _grabbedGameObject.transform.GetComponent<Rigidbody>().isKinematic = true;
        IsObjectGrabbed = true;

        StartCoroutine(TelegrabVFXControl());
        _telegrabAudio.Play();
    }

    private void DropObject()
    {
        //foreach (TelegrabObject telegrabObject in telegrabObjects)
        //{
        //    telegrabObject.gameObject.GetComponent<MeshRenderer>().material = telegrabObject.telegrabMaterial;
        //}

        FoxMovement.Instance.TelegrabEffect.SendEvent("Sparks");
        _grabbedGameObject.transform.parent = null;
        _grabbedGameObject.transform.GetComponent<Rigidbody>().isKinematic = false;
        IsObjectGrabbed = false;
        //telegrabObjects.Clear();
    }

    public void ActivateTelegrabCamera()
    {

        if (!IsTelegrabActivated)
        {
            FoxMovement.Instance.CameraMovement.FreeLookCam.SetActive(false);
            FoxMovement.Instance.CameraMovement.TelegrabCam.SetActive(true);
            FoxMovement.Instance.CameraMovement.CurrentStyle = CameraMovement.CameraStyle.Telegrab;
            StartCoroutine(CrosshairEnable());
        }
        else
        {
            FoxMovement.Instance.CameraMovement.FreeLookCam.SetActive(true);
            FoxMovement.Instance.CameraMovement.TelegrabCam.SetActive(false);
            FoxMovement.Instance.CameraMovement.CurrentStyle = CameraMovement.CameraStyle.Basic;
            StartCoroutine(CrosshairDisable());
        }

        IEnumerator CrosshairEnable()
        {
            yield return new WaitForSeconds(0.2f);
            _telegrabUI.SetActive(true);
        }

        IEnumerator CrosshairDisable()
        {
            yield return new WaitForSeconds(0.2f);
            _telegrabUI.SetActive(false);
        }
    }

    private IEnumerator TelegrabVFXControl()
    {
        _telegrabGlow.gameObject.SetActive(true);
        _telegrabGlow.transform.SetParent(_grabbedGameObject.transform);
        _telegrabGlow.transform.localPosition = Vector3.zero;
        FoxMovement.Instance.TelegrabEffect.SetInt("SpawnMultiplier", 1);
        Vector3 boundsSize = _grabbedGameObject.GetComponent<MeshFilter>().sharedMesh.bounds.size;
        FoxMovement.Instance.TelegrabEffect.SetVector3("TargetBounds", boundsSize);
        float noiseX = UnityEngine.Random.Range(0, 100f);
        float noiseY = UnityEngine.Random.Range(0, 100f);
        float targetIntensity = (boundsSize.x + boundsSize.y + boundsSize.z) / 10;
        _telegrabGlow.range = targetIntensity * 5;
        while (_telegrabGlow.intensity < targetIntensity + targetIntensity * Mathf.PerlinNoise(noiseX, noiseY))
        {
            if (!IsObjectGrabbed)
                break;
            FoxMovement.Instance.TelegrabEffect.SetVector3("TargetPos", _grabbedGameObject.transform.position);
            _telegrabGlow.intensity += Time.deltaTime;
            yield return null;
        }
        while (true)
        {
            if (!IsObjectGrabbed)
                break;
            FoxMovement.Instance.TelegrabEffect.SetVector3("TargetPos", _grabbedGameObject.transform.position);
            noiseX += Time.deltaTime * .01f;
            _telegrabGlow.intensity = targetIntensity + targetIntensity * Mathf.PerlinNoise(noiseX, noiseY);
            yield return null;
        }
        FoxMovement.Instance.TelegrabEffect.SetInt("SpawnMultiplier", 0);
        while (_telegrabGlow.intensity > 0)
        {
            if (IsObjectGrabbed)
                break;
            FoxMovement.Instance.TelegrabEffect.SetVector3("TargetPos", _grabbedGameObject.transform.position);
            _telegrabGlow.intensity -= Time.deltaTime;
            yield return null;
        }
        _telegrabGlow.transform.SetParent(null);
        _telegrabGlow.gameObject.SetActive(false);
    }
}
