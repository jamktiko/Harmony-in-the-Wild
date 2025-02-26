using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CloudManager : MonoBehaviour
{
    public Camera cam;
    public GameObject cloudPrefab;
    public GameObject lightningEmitter;
    public float windScale = 1;
    public float fadeOutStartDist;
    public float fadeOutEndDist;
    public float cloudBaseScale = 20;
    public float weatherSeverity = 1;

    private List<Cloud> clouds = new List<Cloud>();
    private Vector2 wind;
    private float cloudDespawnDist;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //wind = new Vector2(Mathf.PerlinNoise(.25f, Time.time * windChangeTimeScale) - .5f, Mathf.PerlinNoise(.75f, Time.time * windChangeTimeScale) - .5f);
        //cloudCoverage = (int)(Mathf.PerlinNoise(.5f, Time.time * cloudinessChangeTimeScale) * 15);
        if (clouds.Count < 1)
            clouds.Add(AddCloud());
        //else if (clouds.Count > cloudCoverage)
        //TryRemoveCloud();

        // NOTE! uncommented for the current build; activate back later
        //MoveClouds();
    }

    private Cloud AddCloud()
    {
        int size = UnityEngine.Random.Range(10, 21);
        Vector3 cloudScale = new Vector3(UnityEngine.Random.Range(cloudBaseScale * size * .4f, cloudBaseScale * size * 1.5f), 0, UnityEngine.Random.Range(cloudBaseScale * size * .4f, cloudBaseScale * size * 1.5f));
        if (cloudScale.x < cloudScale.z)
            cloudScale += new Vector3(0, cloudScale.x / 2);
        else
            cloudScale += new Vector3(0, cloudScale.z / 2);
        float cloudRot = UnityEngine.Random.Range(0, 360);
        cloudPrefab.transform.rotation = Quaternion.Euler(0, cloudRot, 0);
        Vector3 wind3D = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0) * Vector3.forward;
        wind = new Vector2(wind3D.x, wind3D.z);
        float windAngle = Vector3.Angle(wind, cloudPrefab.transform.rotation * Vector3.forward);
        float spawnDist = fadeOutEndDist + 100;
        if (windAngle < 90)
            spawnDist += ((cloudScale.x * windAngle) + (cloudScale.z * (90 - windAngle))) / 90;
        else
            spawnDist += ((cloudScale.z * (windAngle - 90)) + (cloudScale.x * (180 - windAngle))) / 90;
        cloudPrefab.transform.position = new Vector3(cam.transform.position.x - wind.x * spawnDist, 450 + cloudScale.y / 2, cam.transform.position.z - wind.y * spawnDist);
        cloudDespawnDist = (spawnDist + 100) * (spawnDist + 100);
        Cloud cloud = new Cloud(cloudPrefab.transform, cloudPrefab.GetComponent<VisualEffect>(), UnityEngine.Random.Range(2, 5), UnityEngine.Random.Range(.1f, 1) * weatherSeverity, size, cloudScale, cloudBaseScale);
        cloud.lightning = lightningEmitter.GetComponent<VisualEffect>();
        cloud.lightning.Stop();

        cloud.vfx.Stop();
        cloud.vfx.Play();

        return cloud;
    }

    private void TryRemoveCloud()
    {
        for (int i = 0; i < clouds.Count; i++)
        {
            if ((clouds[i].t.position - Camera.main.transform.position).sqrMagnitude > fadeOutEndDist * fadeOutEndDist)
            {
                Destroy(clouds[i].t.gameObject);
                clouds.RemoveAt(i);
                break;
            }
        }
    }

    private void MoveClouds()
    {
        float oldZ;
        for (int i = 0; i < clouds.Count; i++)
        {
            oldZ = clouds[i].t.position.z;
            clouds[i].t.position += new Vector3(wind.x * Time.deltaTime * windScale, 0, wind.y * Time.deltaTime * windScale);
            if (clouds[i].type == 4)
            {
                clouds[i].lightningCountdown -= Time.deltaTime;
                if (clouds[i].lightningCountdown < 0)
                {
                    StartCoroutine(LightningFlash(clouds[i].lightning));
                    clouds[i].lightning.transform.localPosition = new Vector3(UnityEngine.Random.Range(-clouds[i].vfx.GetVector3("CloudScale").x / 2, clouds[i].vfx.GetVector3("CloudScale").x / 2), 0, UnityEngine.Random.Range(-clouds[i].vfx.GetVector3("CloudScale").z / 2, clouds[i].vfx.GetVector3("CloudScale").z / 2));
                    clouds[i].lightning.Play();
                    clouds[i].lightning.SetFloat("RenderAlpha", 0);
                    clouds[i].lightningCountdown = 7 / clouds[i].severity / clouds[i].severity;
                }
            }
            if ((clouds[i].t.position.x - cam.transform.position.x) * (clouds[i].t.position.x - cam.transform.position.x) + (clouds[i].t.position.z - cam.transform.position.z) * (clouds[i].t.position.z - cam.transform.position.z) > cloudDespawnDist)
            {
                clouds.RemoveAt(i);
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
    public Transform t;
    public VisualEffect vfx;
    public VisualEffect lightning;
    public int type;
    public float severity;
    public float size;
    public float lightningCountdown;

    public Cloud(Transform t, VisualEffect vfx, int type, float severity, float size, Vector3 cloudScale, float baseScale)
    {
        this.t = t;
        this.vfx = vfx;
        this.type = type;
        this.severity = severity;
        this.size = size;

        lightningCountdown = 7 / severity / severity;
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
