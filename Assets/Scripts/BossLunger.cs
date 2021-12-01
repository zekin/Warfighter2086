using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLunger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public float speed = 4f;
    [SerializeField] public float rotationSpeed = 2f;
    [SerializeField] public float pursuitRange = 13f;
    [SerializeField] public float attackRadius = 3f;
    [SerializeField] public float hitRadius = 4f;
    GameObject player;

    Animator anim;
    Unit unit;
    AudioSource audio;
    bool _isMoving = false;
    bool _isAttacking = false;
    bool _isFiring = false;

    float _monsterSpeed = 0f;

    public AudioClip AttackSound;
    public AudioClip HurtSound;
    float monsterSpeed
    {
        get
        {
            return _monsterSpeed;
        }
        set
        {
            _monsterSpeed = value;
            anim.SetFloat("speed", _monsterSpeed);
        }
    }
    bool isAttacking
    {
        get
        {

            return _isAttacking;
        }
        set
        {
            _isAttacking = value;
            anim.SetBool("isAttacking", _isAttacking);
        }
    }

    bool isFiring
    {
        get
        {

            return _isFiring;
        }
        set
        {
            _isFiring = value;
            anim.SetBool("isFiring", _isFiring);
        }
    }
    /*    bool isHurt
        {
            get
            {

                return unit.isHurt;
            }
            set
            {
                unit.isHurt = value;
                anim.SetBool("isHurt", unit.isHurt);
            }
        }*/
    void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();

        unit = GetComponent<Unit>();
        audio = GetComponent<AudioSource>();
        audio.clip = AttackSound;
    }

    void MoveTo(Vector3 direction)
    {
        if (direction.magnitude > pursuitRange || direction.magnitude <= attackRadius)
        {
            monsterSpeed = 0f;
            return;
        }
        Quaternion lookAt = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, Time.deltaTime * rotationSpeed);

        monsterSpeed = direction.magnitude;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

    }
    bool animationIsOver(string name)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            return true;
        }
        return false;
    }
    void Attack(Vector3 direction)
    {
        if (direction.magnitude > attackRadius )
        {
            isAttacking = false;
            return;
        }
        isAttacking = true;

        if (animationIsOver("rig_Charge") && direction.magnitude < hitRadius)
        {
            audio.clip = AttackSound;
            
            audio.Play();
            EventManager.EventHitPlayer.Invoke(gameObject, direction.magnitude, 100f);
        }
    }
    void CheckIfHurt()
    {
        if (unit.isHurt)
        {
            audio.clip = HurtSound;
            audio.Play();
            anim.Play("rig_Hurt");
            unit.isHurt = false;
        }
    }
    // Update is called once per frame
    void DoDamage()
    {

        bool animationIsOver(string name)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName(name))
            {
                return true;
            }
            return false;
        }
    }
    void Update()
    {
        Vector3 dir = player.transform.position - this.transform.position;
        
    bool animationIsOver(string name)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            return true;
        }
        return false;
    }
        MoveTo(dir);
        Attack(dir);
        CheckIfHurt();

    }
}

