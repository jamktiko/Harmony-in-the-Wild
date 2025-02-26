using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class LBBird : MonoBehaviour
{
    enum BirdBehaviors
    {
        Sing,
        Preen,
        Ruffle,
        Peck,
        HopForward,
        HopBackward,
        HopLeft,
        HopRight,
    }

    [FormerlySerializedAs("song1")] public AudioClip Song1;
    [FormerlySerializedAs("song2")] public AudioClip Song2;
    [FormerlySerializedAs("flyAway1")] public AudioClip FlyAway1;
    [FormerlySerializedAs("flyAway2")] public AudioClip FlyAway2;

    [FormerlySerializedAs("fleeCrows")] public bool FleeCrows = true;

    Animator _anim;
    LBBirdController _controller;

    bool _paused = false;
    bool _idle = true;
    bool _flying = false;
    bool _landing = false;
    bool _perched = false;
    bool _onGround = true;
    bool _dead = false;
    BoxCollider _birdCollider;
    Vector3 _bColCenter;
    Vector3 _bColSize;
    SphereCollider _solidCollider;
    float _distanceToTarget = 0.0f;
    float _agitationLevel = .5f;
    float _originalAnimSpeed = 1.0f;
    Vector3 _originalVelocity = Vector3.zero;

    //hash variables for the animation states and animation properties
    int _idleAnimationHash;
    int _singAnimationHash;
    int _ruffleAnimationHash;
    int _preenAnimationHash;
    int _peckAnimationHash;
    int _hopForwardAnimationHash;
    int _hopBackwardAnimationHash;
    int _hopLeftAnimationHash;
    int _hopRightAnimationHash;
    int _worriedAnimationHash;
    int _landingAnimationHash;
    int _flyAnimationHash;
    int _hopIntHash;
    int _flyingBoolHash;
    //int perchedBoolHash;
    int _peckBoolHash;
    int _ruffleBoolHash;
    int _preenBoolHash;
    //int worriedBoolHash;
    int _landingBoolHash;
    int _singTriggerHash;
    int _flyingDirectionHash;
    int _dieTriggerHash;

    void OnEnable()
    {
        _birdCollider = gameObject.GetComponent<BoxCollider>();
        _bColCenter = _birdCollider.center;
        _bColSize = _birdCollider.size;
        _solidCollider = gameObject.GetComponent<SphereCollider>();
        _anim = gameObject.GetComponent<Animator>();

        _idleAnimationHash = Animator.StringToHash("Base Layer.Idle");
        //singAnimationHash = Animator.StringToHash ("Base Layer.sing");
        //ruffleAnimationHash = Animator.StringToHash ("Base Layer.ruffle");
        //preenAnimationHash = Animator.StringToHash ("Base Layer.preen");
        //peckAnimationHash = Animator.StringToHash ("Base Layer.peck");
        //hopForwardAnimationHash = Animator.StringToHash ("Base Layer.hopForward");
        //hopBackwardAnimationHash = Animator.StringToHash ("Base Layer.hopBack");
        //hopLeftAnimationHash = Animator.StringToHash ("Base Layer.hopLeft");
        //hopRightAnimationHash = Animator.StringToHash ("Base Layer.hopRight");
        //worriedAnimationHash = Animator.StringToHash ("Base Layer.worried");
        //landingAnimationHash = Animator.StringToHash ("Base Layer.landing");
        _flyAnimationHash = Animator.StringToHash("Base Layer.fly");
        _hopIntHash = Animator.StringToHash("hop");
        _flyingBoolHash = Animator.StringToHash("flying");
        //perchedBoolHash = Animator.StringToHash("perched");
        _peckBoolHash = Animator.StringToHash("peck");
        _ruffleBoolHash = Animator.StringToHash("ruffle");
        _preenBoolHash = Animator.StringToHash("preen");
        //worriedBoolHash = Animator.StringToHash("worried");
        _landingBoolHash = Animator.StringToHash("landing");
        _singTriggerHash = Animator.StringToHash("sing");
        _flyingDirectionHash = Animator.StringToHash("flyingDirectionX");
        _dieTriggerHash = Animator.StringToHash("die");
        _anim.SetFloat("IdleAgitated", _agitationLevel);
        if (_dead)
        {
            Revive();
        }
    }

    void PauseBird()
    {
        if (!_dead)
        {
            _originalAnimSpeed = _anim.speed;
            _anim.speed = 0;
            if (!GetComponent<Rigidbody>().isKinematic) { _originalVelocity = GetComponent<Rigidbody>().velocity; }
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<AudioSource>().Stop();
            _paused = true;
        }
    }

    void UnPauseBird()
    {
        if (!_dead)
        {
            _anim.speed = _originalAnimSpeed;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().velocity = _originalVelocity;
            _paused = false;
        }
    }

    IEnumerator FlyToTarget(Vector3 target)
    {
        if (Random.value < .5)
        {
            GetComponent<AudioSource>().PlayOneShot(FlyAway1, .1f);
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(FlyAway2, .1f);
        }
        _flying = true;
        _landing = false;
        _onGround = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().drag = 0.5f;
        _anim.applyRootMotion = false;
        _anim.SetBool(_flyingBoolHash, true);
        _anim.SetBool(_landingBoolHash, false);

        //Wait to apply velocity until the bird is entering the flying animation
        while (_anim.GetCurrentAnimatorStateInfo(0).nameHash != _flyAnimationHash)
        {
            yield return 0;
        }

        //birds fly up and away from their perch for 1 second before orienting to the next target
        GetComponent<Rigidbody>().AddForce((transform.forward * 50.0f * _controller.BirdScale) + (transform.up * 100.0f * _controller.BirdScale));
        float t = 0.0f;
        while (t < 1.0f)
        {
            if (!_paused)
            {
                t += Time.deltaTime;
                if (t > .2f && !_solidCollider.enabled && _controller.CollideWithObjects)
                {
                    _solidCollider.enabled = true;
                }
            }
            yield return 0;
        }
        //start to rotate toward target
        Vector3 vectorDirectionToTarget = (target - transform.position).normalized;
        Quaternion finalRotation = Quaternion.identity;
        Quaternion startingRotation = transform.rotation;
        _distanceToTarget = Vector3.Distance(transform.position, target);
        Vector3 forwardStraight;//the forward vector on the xz plane
        RaycastHit hit;
        Vector3 tempTarget = target;
        t = 0.0f;

        //if the target is directly above the bird the bird needs to fly out before going up
        //this should stop them from taking off like a rocket upwards
        if (vectorDirectionToTarget.y > .5f)
        {
            tempTarget = transform.position + (new Vector3(transform.forward.x, .5f, transform.forward.z) * _distanceToTarget);

            while (vectorDirectionToTarget.y > .5f)
            {
                //Debug.DrawLine (tempTarget,tempTarget+Vector3.up,Color.red);
                vectorDirectionToTarget = (tempTarget - transform.position).normalized;
                finalRotation = Quaternion.LookRotation(vectorDirectionToTarget);
                transform.rotation = Quaternion.Slerp(startingRotation, finalRotation, t);
                _anim.SetFloat(_flyingDirectionHash, FindBankingAngle(transform.forward, vectorDirectionToTarget));
                t += Time.deltaTime * 0.5f;
                GetComponent<Rigidbody>().AddForce(transform.forward * 70.0f * _controller.BirdScale * Time.deltaTime);

                //Debug.DrawRay (transform.position,transform.forward,Color.green);

                vectorDirectionToTarget = (target - transform.position).normalized;//reset the variable to reflect the actual target and not the temptarget

                if (Physics.Raycast(transform.position, -Vector3.up, out hit, 0.15f * _controller.BirdScale) && GetComponent<Rigidbody>().velocity.y < 0)
                {
                    //if the bird is going to collide with the ground zero out vertical velocity
                    if (!hit.collider.isTrigger)
                    {
                        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.0f, GetComponent<Rigidbody>().velocity.z);
                    }
                }
                if (Physics.Raycast(transform.position, Vector3.up, out hit, 0.15f * _controller.BirdScale) && GetComponent<Rigidbody>().velocity.y > 0)
                {
                    //if the bird is going to collide with something overhead zero out vertical velocity
                    if (!hit.collider.isTrigger)
                    {
                        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.0f, GetComponent<Rigidbody>().velocity.z);
                    }
                }
                //check for collisions with non trigger colliders and abort flight if necessary
                if (_controller.CollideWithObjects)
                {
                    forwardStraight = transform.forward;
                    forwardStraight.y = 0.0f;
                    //Debug.DrawRay (transform.position+(transform.forward*.1f),forwardStraight*.75f,Color.green);
                    if (Physics.Raycast(transform.position + (transform.forward * .15f * _controller.BirdScale), forwardStraight, out hit, .75f * _controller.BirdScale))
                    {
                        if (!hit.collider.isTrigger)
                        {
                            AbortFlyToTarget();
                        }
                    }
                }
                yield return null;
            }
        }

        finalRotation = Quaternion.identity;
        startingRotation = transform.rotation;
        _distanceToTarget = Vector3.Distance(transform.position, target);

        //rotate the bird toward the target over time
        while (transform.rotation != finalRotation || _distanceToTarget >= 1.5f)
        {
            if (!_paused)
            {
                _distanceToTarget = Vector3.Distance(transform.position, target);
                vectorDirectionToTarget = (target - transform.position).normalized;
                if (vectorDirectionToTarget == Vector3.zero)
                {
                    vectorDirectionToTarget = new Vector3(0.0001f, 0.00001f, 0.00001f);
                }
                finalRotation = Quaternion.LookRotation(vectorDirectionToTarget);
                transform.rotation = Quaternion.Slerp(startingRotation, finalRotation, t);
                _anim.SetFloat(_flyingDirectionHash, FindBankingAngle(transform.forward, vectorDirectionToTarget));
                t += Time.deltaTime * 0.5f;
                GetComponent<Rigidbody>().AddForce(transform.forward * 70.0f * _controller.BirdScale * Time.deltaTime);
                if (Physics.Raycast(transform.position, -Vector3.up, out hit, 0.15f * _controller.BirdScale) && GetComponent<Rigidbody>().velocity.y < 0)
                {
                    //if the bird is going to collide with the ground zero out vertical velocity
                    if (!hit.collider.isTrigger)
                    {
                        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.0f, GetComponent<Rigidbody>().velocity.z);
                    }
                }
                if (Physics.Raycast(transform.position, Vector3.up, out hit, 0.15f * _controller.BirdScale) && GetComponent<Rigidbody>().velocity.y > 0)
                {
                    //if the bird is going to collide with something overhead zero out vertical velocity
                    if (!hit.collider.isTrigger)
                    {
                        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.0f, GetComponent<Rigidbody>().velocity.z);
                    }
                }

                //check for collisions with non trigger colliders and abort flight if necessary
                if (_controller.CollideWithObjects)
                {
                    forwardStraight = transform.forward;
                    forwardStraight.y = 0.0f;
                    //Debug.DrawRay (transform.position+(transform.forward*.1f),forwardStraight*.75f,Color.green);
                    if (Physics.Raycast(transform.position + (transform.forward * .15f * _controller.BirdScale), forwardStraight, out hit, .75f * _controller.BirdScale))
                    {
                        if (!hit.collider.isTrigger)
                        {
                            AbortFlyToTarget();
                        }
                    }
                }
            }
            yield return 0;
        }

        //keep the bird pointing at the target and move toward it
        float flyingForce = 50.0f * _controller.BirdScale;
        while (true)
        {
            if (!_paused)
            {
                //do a raycast to see if the bird is going to hit the ground
                if (Physics.Raycast(transform.position, -Vector3.up, 0.15f * _controller.BirdScale) && GetComponent<Rigidbody>().velocity.y < 0)
                {
                    GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.0f, GetComponent<Rigidbody>().velocity.z);
                }
                if (Physics.Raycast(transform.position, Vector3.up, out hit, 0.15f * _controller.BirdScale) && GetComponent<Rigidbody>().velocity.y > 0)
                {
                    //if the bird is going to collide with something overhead zero out vertical velocity
                    if (!hit.collider.isTrigger)
                    {
                        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.0f, GetComponent<Rigidbody>().velocity.z);
                    }
                }

                //check for collisions with non trigger colliders and abort flight if necessary
                if (_controller.CollideWithObjects)
                {
                    forwardStraight = transform.forward;
                    forwardStraight.y = 0.0f;
                    //Debug.DrawRay (transform.position+(transform.forward*.1f),forwardStraight*.75f,Color.green);
                    if (Physics.Raycast(transform.position + (transform.forward * .15f * _controller.BirdScale), forwardStraight, out hit, .75f * _controller.BirdScale))
                    {
                        if (!hit.collider.isTrigger)
                        {
                            AbortFlyToTarget();
                        }
                    }
                }

                vectorDirectionToTarget = (target - transform.position).normalized;
                finalRotation = Quaternion.LookRotation(vectorDirectionToTarget);
                _anim.SetFloat(_flyingDirectionHash, FindBankingAngle(transform.forward, vectorDirectionToTarget));
                transform.rotation = finalRotation;
                GetComponent<Rigidbody>().AddForce(transform.forward * flyingForce * Time.deltaTime);
                _distanceToTarget = Vector3.Distance(transform.position, target);
                if (_distanceToTarget <= 1.5f * _controller.BirdScale)
                {
                    _solidCollider.enabled = false;
                    if (_distanceToTarget < 0.5f * _controller.BirdScale)
                    {
                        break;
                    }
                    else
                    {
                        GetComponent<Rigidbody>().drag = 2.0f;
                        flyingForce = 50.0f * _controller.BirdScale;
                    }
                }
                else if (_distanceToTarget <= 5.0f * _controller.BirdScale)
                {
                    GetComponent<Rigidbody>().drag = 1.0f;
                    flyingForce = 50.0f * _controller.BirdScale;
                }
            }
            yield return 0;
        }

        _anim.SetFloat(_flyingDirectionHash, 0);
        //initiate the landing for the bird to finally reach the target
        Vector3 vel = Vector3.zero;
        _flying = false;
        _landing = true;
        _solidCollider.enabled = false;
        _anim.SetBool(_landingBoolHash, true);
        _anim.SetBool(_flyingBoolHash, false);
        t = 0.0f;
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        //tell any birds that are in the way to move their butts
        Collider[] hitColliders = Physics.OverlapSphere(target, 0.05f * _controller.BirdScale);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == "lb_bird" && hitColliders[i].transform != transform)
            {
                hitColliders[i].SendMessage("FlyAway");
            }
        }

        //this while loop will reorient the rotation to vertical and translate the bird exactly to the target
        startingRotation = transform.rotation;
        transform.localEulerAngles = new Vector3(0.0f, transform.localEulerAngles.y, 0.0f);
        finalRotation = transform.rotation;
        transform.rotation = startingRotation;
        while (_distanceToTarget > 0.05f * _controller.BirdScale)
        {
            if (!_paused)
            {
                transform.rotation = Quaternion.Slerp(startingRotation, finalRotation, t * 4.0f);
                transform.position = Vector3.SmoothDamp(transform.position, target, ref vel, 0.5f);
                t += Time.deltaTime;
                _distanceToTarget = Vector3.Distance(transform.position, target);
                if (t > 2.0f)
                {
                    break;//failsafe to stop birds from getting stuck
                }
            }
            yield return 0;
        }
        GetComponent<Rigidbody>().drag = .5f;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        _anim.SetBool(_landingBoolHash, false);
        _landing = false;
        transform.localEulerAngles = new Vector3(0.0f, transform.localEulerAngles.y, 0.0f);
        transform.position = target;
        _anim.applyRootMotion = true;
        _onGround = true;
    }

    //Sets a variable between -1 and 1 to control the left and right banking animation
    float FindBankingAngle(Vector3 birdForward, Vector3 dirToTarget)
    {
        Vector3 cr = Vector3.Cross(birdForward, dirToTarget);
        float ang = Vector3.Dot(cr, Vector3.up);
        return ang;
    }

    void OnGroundBehaviors()
    {
        _idle = _anim.GetCurrentAnimatorStateInfo(0).nameHash == _idleAnimationHash;
        if (!GetComponent<Rigidbody>().isKinematic)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
        if (_idle)
        {
            //the bird is in the idle animation, lets randomly choose a behavior every 3 seconds
            if (Random.value < Time.deltaTime * .33)
            {
                //bird will display a behavior
                //in the perched state the bird can only sing, preen, or ruffle
                float rand = Random.value;
                if (rand < .3)
                {
                    DisplayBehavior(BirdBehaviors.Sing);
                }
                else if (rand < .5)
                {
                    DisplayBehavior(BirdBehaviors.Peck);
                }
                else if (rand < .6)
                {
                    DisplayBehavior(BirdBehaviors.Preen);
                }
                else if (!_perched && rand < .7)
                {
                    DisplayBehavior(BirdBehaviors.Ruffle);
                }
                else if (!_perched && rand < .85)
                {
                    DisplayBehavior(BirdBehaviors.HopForward);
                }
                else if (!_perched && rand < .9)
                {
                    DisplayBehavior(BirdBehaviors.HopLeft);
                }
                else if (!_perched && rand < .95)
                {
                    DisplayBehavior(BirdBehaviors.HopRight);
                }
                else if (!_perched && rand <= 1)
                {
                    DisplayBehavior(BirdBehaviors.HopBackward);
                }
                else
                {
                    DisplayBehavior(BirdBehaviors.Sing);
                }
                //lets alter the agitation level of the brid so it uses a different mix of idle animation next time
                _anim.SetFloat("IdleAgitated", Random.value);
            }
            //birds should fly to a new target about every 10 seconds
            if (Random.value < Time.deltaTime * .1)
            {
                FlyAway();
            }
        }
    }

    void DisplayBehavior(BirdBehaviors behavior)
    {
        _idle = false;
        switch (behavior)
        {
            case BirdBehaviors.Sing:
                _anim.SetTrigger(_singTriggerHash);
                break;
            case BirdBehaviors.Ruffle:
                _anim.SetTrigger(_ruffleBoolHash);
                break;
            case BirdBehaviors.Preen:
                _anim.SetTrigger(_preenBoolHash);
                break;
            case BirdBehaviors.Peck:
                _anim.SetTrigger(_peckBoolHash);
                break;
            case BirdBehaviors.HopForward:
                _anim.SetInteger(_hopIntHash, 1);
                break;
            case BirdBehaviors.HopLeft:
                _anim.SetInteger(_hopIntHash, -2);
                break;
            case BirdBehaviors.HopRight:
                _anim.SetInteger(_hopIntHash, 2);
                break;
            case BirdBehaviors.HopBackward:
                _anim.SetInteger(_hopIntHash, -1);
                break;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "lb_bird")
        {
            FlyAway();
        }
    }

    void OnTriggerExit(Collider col)
    {
        //if bird has hopped out of the target area lets fly
        if (_onGround && (col.tag == "lb_groundTarget" || col.tag == "lb_perchTarget"))
        {
            FlyAway();
        }
    }

    void AbortFlyToTarget()
    {
        StopCoroutine("FlyToTarget");
        _solidCollider.enabled = false;
        _anim.SetBool(_landingBoolHash, false);
        _anim.SetFloat(_flyingDirectionHash, 0);
        transform.localEulerAngles = new Vector3(
            0.0f,
            transform.localEulerAngles.y,
            0.0f);
        FlyAway();
    }

    void FlyAway()
    {
        if (!_dead)
        {
            StopCoroutine("FlyToTarget");
            _anim.SetBool(_landingBoolHash, false);
            _controller.SendMessage("BirdFindTarget", gameObject);
        }
    }

    void Flee()
    {
        if (!_dead)
        {
            StopCoroutine("FlyToTarget");
            GetComponent<AudioSource>().Stop();
            _anim.Play(_flyAnimationHash);
            Vector3 farAwayTarget = transform.position;
            farAwayTarget += new Vector3(Random.Range(-100, 100) * _controller.BirdScale, 10 * _controller.BirdScale, Random.Range(-100, 100) * _controller.BirdScale);
            StartCoroutine("FlyToTarget", farAwayTarget);
        }
    }

    void CrowIsClose()
    {
        if (FleeCrows && !_dead)
        {
            Flee();
        }
    }

    public void KillBird()
    {
        if (!_dead)
        {
            _controller.SendMessage("FeatherEmit", transform.position);
            _anim.SetTrigger(_dieTriggerHash);
            _anim.applyRootMotion = false;
            _dead = true;
            _onGround = false;
            _flying = false;
            _landing = false;
            _idle = false;
            _perched = false;
            AbortFlyToTarget();
            StopAllCoroutines();
            GetComponent<Collider>().isTrigger = false;
            _birdCollider.center = new Vector3(0.0f, 0.0f, 0.0f);
            _birdCollider.size = new Vector3(0.1f, 0.01f, 0.1f) * _controller.BirdScale;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public void KillBirdWithForce(Vector3 force)
    {
        if (!_dead)
        {
            _controller.SendMessage("FeatherEmit", transform.position);
            _anim.SetTrigger(_dieTriggerHash);
            _anim.applyRootMotion = false;
            _dead = true;
            _onGround = false;
            _flying = false;
            _landing = false;
            _idle = false;
            _perched = false;
            AbortFlyToTarget();
            StopAllCoroutines();
            GetComponent<Collider>().isTrigger = false;
            _birdCollider.center = new Vector3(0.0f, 0.0f, 0.0f);
            _birdCollider.size = new Vector3(0.1f, 0.01f, 0.1f) * _controller.BirdScale;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().AddForce(force);
        }
    }

    void Revive()
    {
        if (_dead)
        {
            _birdCollider.center = _bColCenter;
            _birdCollider.size = _bColSize;
            GetComponent<Collider>().isTrigger = true;
            _dead = false;
            _onGround = false;
            _flying = false;
            _landing = false;
            _idle = true;
            _perched = false;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = false;
            _anim.Play(_idleAnimationHash);
            _controller.SendMessage("BirdFindTarget", gameObject);
        }
    }

    void SetController(LBBirdController cont)
    {
        _controller = cont;
    }

    void ResetHopInt()
    {
        _anim.SetInteger(_hopIntHash, 0);
    }

    void ResetFlyingLandingVariables()
    {
        if (_flying || _landing)
        {
            _flying = false;
            _landing = false;
        }
    }

    void PlaySong()
    {
        if (!_dead)
        {
            if (Random.value < .5)
            {
                GetComponent<AudioSource>().PlayOneShot(Song1, 1);
            }
            else
            {
                GetComponent<AudioSource>().PlayOneShot(Song2, 1);
            }
        }
    }

    void Update()
    {
        if (_onGround && !_paused && !_dead)
        {
            OnGroundBehaviors();
        }
    }
}
