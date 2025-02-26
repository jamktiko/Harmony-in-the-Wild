using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class CloudManager : MonoBehaviour
{
    [FormerlySerializedAs("cam")] public Camera Cam;
    [FormerlySerializedAs("cloudPrefab")] public GameObject CloudPrefab;
    [FormerlySerializedAs("lightningEmitter")] public GameObject LightningEmitter;
    [FormerlySerializedAs("windScale")] public float WindScale = 1;
    [FormerlySerializedAs("fadeOutStartDist")] public float FadeOutStartDist;
    [FormerlySerializedAs("fadeOutEndDist")] public float FadeOutEndDist;
    [FormerlySerializedAs("cloudBaseScale")] public float CloudBaseScale = 20;
    [FormerlySerializedAs("weatherSeverity")] public float WeatherSeverity = 1;

    private List<Cloud> _clouds = new List<Cloud>();
    private Vector2 _wind;
    private float _cloudDespawnDist;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //wind = new Vector2(Mathf.PerlinNoise(.25f, Time.time * windChangeTimeScale) - .5f, Mathf.PerlinNoise(.75f, Time.time * windChangeTimeScale) - .5f);
        //cloudCoverage = (int)(Mathf.PerlinNoise(.5f, Time.time * cloudinessChangeTimeScale) * 15);
        if (_clouds.Count < 1)
            _clouds.Add(AddCloud());
        //else if (clouds.Count > cloudCoverage)
        //TryRemoveCloud();

        // NOTE! uncommented for the current build; activate back later
        //MoveClouds();
    }

    private Cloud AddCloud()
    {
        int size = UnityEngine.Random.Range(10, 21);
        Vector3 cloudScale = new Vector3(UnityEngine.Random.Range(CloudBaseScale * size * .4f, CloudBaseScale * size * 1.5f), 0, UnityEngine.Random.Range(CloudBaseScale * size * .4f, CloudBaseScale * size * 1.5f));
        if (cloudScale.x < cloudScale.z)
            cloudScale += new Vector3(0, cloudScale.x / 2);
        else
            cloudScale += new Vector3(0, cloudScale.z / 2);
        float cloudRot = UnityEngine.Random.Range(0, 360);
        CloudPrefab.transform.rotation = Quaternion.Euler(0, cloudRot, 0);
        Vector3 wind3D = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0) * Vector3.forward;
        _wind = new Vector2(wind3D.x, wind3D.z);
        float windAngle = Vector3.Angle(_wind, CloudPrefab.transform.rotation * Vector3.forward);
        float spawnDist = FadeOutEndDist + 100;
        if (windAngle < 90)
            spawnDist += ((cloudScale.x * windAngle) + (cloudScale.z * (90 - windAngle))) / 90;
        else
            spawnDist += ((cloudScale.z * (windAngle - 90)) + (cloudScale.x * (180 - windAngle))) / 90;
        CloudPrefab.transform.position = new Vector3(Cam.transform.position.x - _wind.x * spawnDist, 450 + cloudScale.y / 2, Cam.transform.position.z - _wind.y * spawnDist);
        _cloudDespawnDist = (spawnDist + 100) * (spawnDist + 100);
        Cloud cloud = new Cloud(CloudPrefab.transform, CloudPrefab.GetComponent<VisualEffect>(), UnityEngine.Random.Range(2, 5), UnityEngine.Random.Range(.1f, 1) * WeatherSeverity, size, cloudScale, CloudBaseScale);
        cloud.Lightning = LightningEmitter.GetComponent<VisualEffect>();
        cloud.Lightning.Stop();

        cloud.VFX.Stop();
        cloud.VFX.Play();

        return cloud;
    }

    private void TryRemoveCloud()
    {
        for (int i = 0; i < _clouds.Count; i++)
        {
            if ((_clouds[i].T.position - Camera.main.transform.position).sqrMagnitude > FadeOutEndDist * FadeOutEndDist)
            {
                Destroy(_clouds[i].T.gameObject);
                _clouds.RemoveAt(i);
                break;
            }
        }
    }

    private void MoveClouds()
    {
        float oldZ;
        for (int i = 0; i < _clouds.Count; i++)
        {
            oldZ = _clouds[i].T.position.z;
            _clouds[i].T.position += new Vector3(_wind.x * Time.deltaTime * WindScale, 0, _wind.y * Time.deltaTime * WindScale);
            if (_clouds[i].Type == 4)
            {
                _clouds[i].LightningCountdown -= Time.deltaTime;
                if (_clouds[i].LightningCountdown < 0)
                {
                    StartCoroutine(LightningFlash(_clouds[i].Lightning));
                    _clouds[i].Lightning.transform.localPosition = new Vector3(UnityEngine.Random.Range(-_clouds[i].VFX.GetVector3("CloudScale").x / 2, _clouds[i].VFX.GetVector3("CloudScale").x / 2), 0, UnityEngine.Random.Range(-_clouds[i].VFX.GetVector3("CloudScale").z / 2, _clouds[i].VFX.GetVector3("CloudScale").z / 2));
                    _clouds[i].Lightning.Play();
                    _clouds[i].Lightning.SetFloat("RenderAlpha", 0);
                    _clouds[i].LightningCountdown = 7 / _clouds[i].Severity / _clouds[i].Severity;
                }
            }
            if ((_clouds[i].T.position.x - Cam.transform.position.x) * (_clouds[i].T.position.x - Cam.transform.position.x) + (_clouds[i].T.position.z - Cam.transform.position.z) * (_clouds[i].T.position.z - Cam.transform.position.z) > _cloudDespawnDist)
            {
                _clouds.RemoveAt(i);
                i--;
            }
        }
    }

    IEnumerator LightningFlash(VisualEffect lightning)
    {
        yield return new WaitForSeconds(4);
        lightning.SetFloat("RenderAlpha", 1);
        lightning.pause = true;
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 1.9f));
        lightning.SetFloat("RenderAlpha", 0);
        lightning.Stop();
    }
}

public class Cloud
{
    public Transform T;
    public VisualEffect VFX;
    public VisualEffect Lightning;
    public int Type;
    public float Severity;
    public float Size;
    public float LightningCountdown;

    public Cloud(Transform t, VisualEffect vfx, int type, float severity, float size, Vector3 cloudScale, float baseScale)
    {
        this.T = t;
        this.VFX = vfx;
        this.Type = type;
        this.Severity = severity;
        this.Size = size;

        LightningCountdown = 7 / severity / severity;
        switch (type)
        {
            case 0: // high regular
                vfx.SetVector4("CloudColour", new Vector4(1, 1, 1, 1));
                break;
            case 1: // flat regular
                vfx.SetVector4("CloudColour", new Vector4(1, 1, 1, 1));
                break;
            case 2: // flat rain
                vfx.SetVector4("CloudColour", new Vector4(1 - severity * .6f, 1 - severity * .6f, 1 - severity * .6f, 1));
                break;
            case 3: // high rain
                vfx.SetVector4("CloudColour", new Vector4(1 - severity * .6f, 1 - severity * .6f, 1 - severity * .6f, 1));
                break;
            case 4: // storm
                vfx.SetVector4("CloudColour", new Vector4(.4f - severity * .35f, .4f - severity * .35f, .4f - severity * .35f, 1));
                break;
            default:
                break;
        }
        vfx.SetVector3("CloudScale", cloudScale);
        vfx.SetVector3("CloudElementScaleMin", new Vector3(baseScale * size / 2, baseScale * size / 4, baseScale * size / 2));
        vfx.SetVector3("CloudElementScaleMax", new Vector3(baseScale * size * 2, baseScale * size, baseScale * size * 2));
        vfx.SetInt("CloudElementCount", (int)(50 * severity * size));
        if (type > 1)
        {
            vfx.SetFloat("RainPerFrame", 1 + severity * 16);
            vfx.SetFloat("SnowPerFrame", 1 + severity * 4);
        }
        else
        {
            vfx.SetInt("RainPerFrame", 0);
            vfx.SetInt("SnowPerFrame", 0);
        }
    }
}
