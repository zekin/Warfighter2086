using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float health;
    [SerializeField] public AudioClip deathSound;
    public bool isHurt = false;

    void Start()
    {
        
    }
    private void OnEnable()
    {
    }
    public float damage(float amount)
    {
        health -= amount;
        if (health < 0)
        {
            EventManager.EventDie.Invoke(gameObject);
        }
        isHurt = true;
        return health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
