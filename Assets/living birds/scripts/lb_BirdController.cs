using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LBBirdController : MonoBehaviour
{
    [FormerlySerializedAs("idealNumberOfBirds")] public int IdealNumberOfBirds;
    [FormerlySerializedAs("maximumNumberOfBirds")] public int MaximumNumberOfBirds;
    [FormerlySerializedAs("currentCamera")] public Camera CurrentCamera;
    [FormerlySerializedAs("unspawnDistance")] public float UnspawnDistance = 10.0f;
    [FormerlySerializedAs("highQuality")] public bool HighQuality = true;
    [FormerlySerializedAs("collideWithObjects")] public bool CollideWithObjects = true;
    [FormerlySerializedAs("groundLayer")] public LayerMask GroundLayer;
    [FormerlySerializedAs("birdScale")] public float BirdScale = 1.0f;

    [FormerlySerializedAs("robin")] public bool Robin = true;
    [FormerlySerializedAs("blueJay")] public bool BlueJay = true;
    [FormerlySerializedAs("cardinal")] public bool Cardinal = true;
    [FormerlySerializedAs("chickadee")] public bool Chickadee = true;
    [FormerlySerializedAs("sparrow")] public bool Sparrow = true;
    [FormerlySerializedAs("goldFinch")] public bool GoldFinch = true;
    [FormerlySerializedAs("crow")] public bool Crow = true;

    private bool _pause = false;
    private GameObject[] _myBirds;
    private List<string> _myBirdTypes = new List<string>();
    private List<GameObject> _birdGroundTargets = new List<GameObject>();
    private List<GameObject> _birdPerchTargets = new List<GameObject>();
    private int _activeBirds = 0;
    private int _birdIndex = 0;
    private GameObject[] _featherEmitters = new GameObject[3];

    public void AllFlee()
    {
        if (!_pause)
        {
            for (int i = 0; i < _myBirds.Length; i++)
            {
                if (_myBirds[i].activeSelf)
                {
                    _myBirds[i].SendMessage("Flee");
                }
            }
        }
    }

    public void Pause()
    {
        if (_pause)
        {
            AllUnPause();
        }
        else
        {
            AllPause();
        }
    }

    public void AllPause()
    {
        _pause = true;
        for (int i = 0; i < _myBirds.Length; i++)
        {
            if (_myBirds[i].activeSelf)
            {
                _myBirds[i].SendMessage("PauseBird");
            }
        }
    }

    public void AllUnPause()
    {
        _pause = false;
        for (int i = 0; i < _myBirds.Length; i++)
        {
            if (_myBirds[i].activeSelf)
            {
                _myBirds[i].SendMessage("UnPauseBird");
            }
        }
    }

    public void SpawnAmount(int amt)
    {
        for (int i = 0; i <= amt; i++)
        {
            SpawnBird();
        }
    }

    public void ChangeCamera(Camera cam)
    {
        CurrentCamera = cam;
    }

    private void Start()
    {
        //find the camera
        if (CurrentCamera == null)
        {
            CurrentCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        if (IdealNumberOfBirds >= MaximumNumberOfBirds)
        {
            IdealNumberOfBirds = MaximumNumberOfBirds - 1;
        }
        //set up the bird types to use
        if (Robin)
        {
            _myBirdTypes.Add("lb_robin");
        }
        if (BlueJay)
        {
            _myBirdTypes.Add("lb_blueJay");
        }
        if (Cardinal)
        {
            _myBirdTypes.Add("lb_cardinal");
        }
        if (Chickadee)
        {
            _myBirdTypes.Add("lb_chickadee");
        }
        if (Sparrow)
        {
            _myBirdTypes.Add("lb_sparrow");
        }
        if (GoldFinch)
        {
            _myBirdTypes.Add("lb_goldFinch");
        }
        if (Crow)
        {
            _myBirdTypes.Add("lb_crow");
        }
        //Instantiate birds based on amounts and bird types
        _myBirds = new GameObject[MaximumNumberOfBirds];
        GameObject bird;
        for (int i = 0; i < _myBirds.Length; i++)
        {
            if (HighQuality)
            {
                bird = Resources.Load(_myBirdTypes[Random.Range(0, _myBirdTypes.Count)] + "HQ", typeof(GameObject)) as GameObject;
            }
            else
            {
                bird = Resources.Load(_myBirdTypes[Random.Range(0, _myBirdTypes.Count)], typeof(GameObject)) as GameObject;
            }
            _myBirds[i] = Instantiate(bird, Vector3.zero, Quaternion.identity) as GameObject;
            _myBirds[i].transform.localScale = _myBirds[i].transform.localScale * BirdScale;
            _myBirds[i].transform.parent = transform;
            _myBirds[i].SendMessage("SetController", this);
            _myBirds[i].SetActive(false);
        }

        //find all the targets
        GameObject[] groundTargets = GameObject.FindGameObjectsWithTag("lb_groundTarget");
        GameObject[] perchTargets = GameObject.FindGameObjectsWithTag("lb_perchTarget");

        for (int i = 0; i < groundTargets.Length; i++)
        {
            if (Vector3.Distance(groundTargets[i].transform.position, CurrentCamera.transform.position) < UnspawnDistance)
            {
                _birdGroundTargets.Add(groundTargets[i]);
            }
        }
        for (int i = 0; i < perchTargets.Length; i++)
        {
            if (Vector3.Distance(perchTargets[i].transform.position, CurrentCamera.transform.position) < UnspawnDistance)
            {
                _birdPerchTargets.Add(perchTargets[i]);
            }
        }

        //instantiate 3 feather emitters for killing the birds
        GameObject fEmitter = Resources.Load("featherEmitter", typeof(GameObject)) as GameObject;
        for (int i = 0; i < 3; i++)
        {
            _featherEmitters[i] = Instantiate(fEmitter, Vector3.zero, Quaternion.identity) as GameObject;
            _featherEmitters[i].transform.parent = transform;
            _featherEmitters[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        InvokeRepeating("UpdateBirds", 1, 1);
        StartCoroutine("UpdateTargets");
    }

    private Vector3 FindPointInGroundTarget(GameObject target)
    {
        //find a random point within the collider of a ground target that touches the ground
        Vector3 point;
        point.x = Random.Range(target.GetComponent<Collider>().bounds.max.x, target.GetComponent<Collider>().bounds.min.x);
        point.y = target.GetComponent<Collider>().bounds.max.y;
        point.z = Random.Range(target.GetComponent<Collider>().bounds.max.z, target.GetComponent<Collider>().bounds.min.z);
        //raycast down until it hits the ground
        RaycastHit hit;
        if (Physics.Raycast(point, -Vector3.up, out hit, target.GetComponent<Collider>().bounds.size.y, GroundLayer))
        {
            return hit.point;
        }

        return point;
    }

    private void UpdateBirds()
    {
        //this function is called once a second
        if (_activeBirds < IdealNumberOfBirds && AreThereActiveTargets())
        {
            //if there are less than ideal birds active, spawn a bird
            SpawnBird();
        }
        else if (_activeBirds < MaximumNumberOfBirds && Random.value < .05 && AreThereActiveTargets())
        {
            //if there are less than maximum birds active spawn a bird every 20 seconds
            SpawnBird();
        }

        //check one bird every second to see if it should be unspawned
        if (_myBirds[_birdIndex].activeSelf && BirdOffCamera(_myBirds[_birdIndex].transform.position) && Vector3.Distance(_myBirds[_birdIndex].transform.position, CurrentCamera.transform.position) > UnspawnDistance)
        {
            //if the bird is off camera and at least unsapwnDistance units away lets unspawn
            Unspawn(_myBirds[_birdIndex]);
        }

        _birdIndex = _birdIndex == _myBirds.Length - 1 ? 0 : _birdIndex + 1;
    }

    //this function will cycle through targets removing those outside of the unspawnDistance
    //it will also add any new targets that come into range
    private IEnumerator UpdateTargets()
    {
        List<GameObject> gtRemove = new List<GameObject>();
        List<GameObject> ptRemove = new List<GameObject>();

        while (true)
        {
            gtRemove.Clear();
            ptRemove.Clear();
            //check targets to see if they are out of range
            for (int i = 0; i < _birdGroundTargets.Count; i++)
            {
                if (Vector3.Distance(_birdGroundTargets[i].transform.position, CurrentCamera.transform.position) > UnspawnDistance)
                {
                    gtRemove.Add(_birdGroundTargets[i]);
                }
                yield return 0;
            }
            for (int i = 0; i < _birdPerchTargets.Count; i++)
            {
                if (Vector3.Distance(_birdPerchTargets[i].transform.position, CurrentCamera.transform.position) > UnspawnDistance)
                {
                    ptRemove.Add(_birdPerchTargets[i]);
                }
                yield return 0;
            }
            //remove any targets that have been found out of range
            foreach (GameObject entry in gtRemove)
            {
                _birdGroundTargets.Remove(entry);
            }
            foreach (GameObject entry in ptRemove)
            {
                _birdPerchTargets.Remove(entry);
            }
            yield return 0;
            //now check for any new Targets
            Collider[] hits = Physics.OverlapSphere(CurrentCamera.transform.position, UnspawnDistance);
            foreach (Collider hit in hits)
            {
                if (hit.tag == "lb_groundTarget" && !_birdGroundTargets.Contains(hit.gameObject))
                {
                    _birdGroundTargets.Add(hit.gameObject);
                }
                if (hit.tag == "lb_perchTarget" && !_birdPerchTargets.Contains(hit.gameObject))
                {
                    _birdPerchTargets.Add(hit.gameObject);
                }
            }
            yield return 0;
        }
    }

    private bool BirdOffCamera(Vector3 birdPos)
    {
        Vector3 screenPos = CurrentCamera.WorldToViewportPoint(birdPos);
        if (screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Unspawn(GameObject bird)
    {
        bird.transform.position = Vector3.zero;
        bird.SetActive(false);
        _activeBirds--;
    }

    private void SpawnBird()
    {
        if (!_pause)
        {
            GameObject bird = null;
            int randomBirdIndex = Mathf.FloorToInt(Random.Range(0, _myBirds.Length));
            int loopCheck = 0;
            //find a random bird that is not active
            while (bird == null)
            {
                if (_myBirds[randomBirdIndex].activeSelf == false)
                {
                    bird = _myBirds[randomBirdIndex];
                }
                randomBirdIndex = randomBirdIndex + 1 >= _myBirds.Length ? 0 : randomBirdIndex + 1;
                loopCheck++;
                if (loopCheck >= _myBirds.Length)
                {
                    //all birds are active
                    return;
                }
            }
            //Find a point off camera to positon the bird and activate it
            bird.transform.position = FindPositionOffCamera();
            if (bird.transform.position == Vector3.zero)
            {
                //couldnt find a suitable spawn point
                return;
            }
            else
            {
                bird.SetActive(true);
                _activeBirds++;
                BirdFindTarget(bird);
            }
        }
    }

    private bool AreThereActiveTargets()
    {
        if (_birdGroundTargets.Count > 0 || _birdPerchTargets.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Vector3 FindPositionOffCamera()
    {
        RaycastHit hit;
        float dist = Random.Range(2, 10);
        Vector3 ray = -CurrentCamera.transform.forward;
        int loopCheck = 0;
        //find a random ray pointing away from the cameras field of view
        ray += new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
        //cycle through random rays until we find one that doesnt hit anything
        while (Physics.Raycast(CurrentCamera.transform.position, ray, out hit, dist))
        {
            dist = Random.Range(2, 10);
            loopCheck++;
            if (loopCheck > 35)
            {
                //can't find any good spawn points so lets cancel
                return Vector3.zero;
            }
        }
        return CurrentCamera.transform.position + (ray * dist);
    }

    private void BirdFindTarget(GameObject bird)
    {
        //yield return new WaitForSeconds(1);
        GameObject target;
        if (_birdGroundTargets.Count > 0 || _birdPerchTargets.Count > 0)
        {
            //pick a random target based on the number of available targets vs the area of ground targets
            //each perch target counts for .3 area, each ground target's area is calculated
            float gtArea = 0.0f;
            float ptArea = _birdPerchTargets.Count * 0.3f;

            for (int i = 0; i < _birdGroundTargets.Count; i++)
            {
                gtArea += _birdGroundTargets[i].GetComponent<Collider>().bounds.size.x * _birdGroundTargets[i].GetComponent<Collider>().bounds.size.z;
            }
            if (ptArea == 0.0f || Random.value < gtArea / (gtArea + ptArea))
            {
                target = _birdGroundTargets[Mathf.FloorToInt(Random.Range(0, _birdGroundTargets.Count))];
                bird.SendMessage("FlyToTarget", FindPointInGroundTarget(target));
            }
            else
            {
                target = _birdPerchTargets[Mathf.FloorToInt(Random.Range(0, _birdPerchTargets.Count))];
                bird.SendMessage("FlyToTarget", target.transform.position);
            }
        }
        else
        {
            bird.SendMessage("FlyToTarget", CurrentCamera.transform.position + new Vector3(Random.Range(-100, 100), Random.Range(5, 10), Random.Range(-100, 100)));
        }
    }

    private void FeatherEmit(Vector3 pos)
    {
        foreach (GameObject fEmit in _featherEmitters)
        {
            if (!fEmit.activeSelf)
            {
                fEmit.transform.position = pos;
                fEmit.SetActive(true);
                StartCoroutine("DeactivateFeathers", fEmit);
                break;
            }
        }
    }

    private IEnumerator DeactivateFeathers(GameObject featherEmit)
    {
        yield return new WaitForSeconds(4.5f);
        featherEmit.SetActive(false);
    }
}
