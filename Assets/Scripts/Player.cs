using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float playerSpeed = 5f;
    [SerializeField] float rotationSpeed = 2f;

    [SerializeField] float fireDelay = 1f;
    float lastFired = 1f;
    private Unit unit;
    private Rigidbody rb;
    private Animator anim;
    bool _isMoving = false;
    bool _isShooting = false;
    bool _isAttacking = false;

    bool isMoving
    {
        get
        {

            return _isMoving;
        }
        set
        {
            _isMoving = value;
            anim.SetBool("isMoving", _isMoving);
        }
    }

    bool isShooting
    {
        get
        {

            return _isShooting;
        }
        set
        {
            _isShooting = value;
            anim.SetBool("isShooting", _isShooting);
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
    private void getHit(GameObject attacker, float distance, float damage)
    {
        unit.damage(damage / (distance + 1f));
    }

    private void OnEnable()
    {

        EventManager.EventHitPlayer.AddListener(getHit);
    }
    private void OnDisable()
    {
        EventManager.EventHitPlayer.RemoveListener(getHit);
    }
    private void Awake()
    {

    }
    void Start()
    {
        anim = GetComponent<Animator>();
        rb=GetComponent<Rigidbody>();
        unit = GetComponent<Unit>();
    }

    void rotateTo(float x, float z)
    {

        if (x == 0 && z == 0)
        {
            return;
        }
        Vector3 lookDir = new Vector3(x, 0, z);
        Vector3 dir = lookDir;
        Quaternion lookAt = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, Time.deltaTime * rotationSpeed);

    }
    void moveTo(float x, float z)
    {
        if (x == 0 && z == 0)
        {
            isMoving = false;
            return;
        }

        isMoving = true;
        rb.velocity = new Vector3(x * playerSpeed, rb.velocity.y, z * playerSpeed);

    }
    void Shoot(Vector3 direction)
    {

        if (!Input.GetMouseButtonDown(0) || Time.fixedTime < lastFired+fireDelay)
        {
            isShooting = false;
            return;
        }
        isShooting = true;
//        transform.forward = direction;
        EventManager.EventFireGun.Invoke(gameObject, new Vector2(1f, 0f));
        lastFired = Time.fixedTime;
    }

    void Attack(Vector3 direction)
    {

        if (!Input.GetKeyDown("space"))
        {
            isAttacking = false;
            return;
        }

        isAttacking = true;
//        transform.forward = direction;
    }
    // Update is called once per frame
    void Update()
    {

        //this goes somewhere else
        //        mouseCoords.Normalize();
        //        mouseCoords-= new Vector3(.5f, .5f, 0f);
        //        mouseCoords.Normalize();

        float xInput = Input.GetAxis("Vertical");
        float zInput = -Input.GetAxis("Horizontal");
        Vector3 mouseCoords = new Vector3(Input.mousePosition.x / Screen.width - .5f, 0f, Input.mousePosition.y / Screen.height - .5f);
        Vector3 mouseToDirection = Quaternion.Euler(new Vector3(0f, 45f, 0f)) * mouseCoords;

        if (!Input.GetMouseButton(1))
        {
            rotateTo(xInput, zInput);
        } else
        {
            rotateTo(mouseToDirection.x, mouseToDirection.z);
        }



        moveTo(xInput, zInput);
        Attack(mouseToDirection);
        Shoot(mouseToDirection);

    }
    private void FixedUpdate()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
    }
}
