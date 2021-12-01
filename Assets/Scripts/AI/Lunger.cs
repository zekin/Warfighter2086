using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lunger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public float speed = 4f;
    [SerializeField] public float rotationSpeed = 2f;
    [SerializeField] public float pursuitRange = 11f;
    [SerializeField] public float attackRadius = 3f;
    [SerializeField] public float hitRadius = 4f;


    [SerializeField] public AudioClip AttackSound;
    [SerializeField] public AudioClip HurtSound;
    
    AudioSource audio;
    GameObject player;
    Unit unit;
    Animator anim;
    bool _isMoving = false;
    bool _isAttacking = false;
    float _monsterSpeed = 0f;
    int _curAttack = 0;

    int curAttack
    {
        get
        {
            return _curAttack;
        }
        set
        {
            _curAttack = value;
            anim.SetInteger("currentAttack", _curAttack);
        }
    }
    float monsterSpeed 
    {
        get {
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
    void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        unit = GetComponent<Unit>();
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
        if (direction.magnitude > attackRadius)
        {
            isAttacking = false;
            return;
        }
        isAttacking = true;

        if (animationIsOver("rig_Stab") && direction.magnitude < hitRadius)
        {

            curAttack = 1;
            audio.clip = AttackSound;
            audio.Play();
            EventManager.EventHitPlayer.Invoke(gameObject, direction.magnitude, 15f);
        }
        else if (animationIsOver("rig_Swipe") && direction.magnitude < hitRadius)
        {
            curAttack = 2;

            audio.clip = AttackSound;
            audio.Play();
            EventManager.EventHitPlayer.Invoke(gameObject, direction.magnitude, 10f);
        }
        else if (animationIsOver("rig_Lunge") && direction.magnitude < hitRadius+2)
        {
            curAttack = 0;

            audio.clip = AttackSound;
            audio.Play();
            EventManager.EventHitPlayer.Invoke(gameObject, direction.magnitude, 15f);
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
    void Update()
    {
        Vector3 dir = player.transform.position - this.transform.position;
        MoveTo(dir);
        Attack(dir);
        CheckIfHurt();

        //        d = Vector3.Distance(this.transform.position, player.transform.position);
    }
}
