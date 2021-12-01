using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EventManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public static UnityEvent<GameObject, Vector2> EventFireGun = new UnityEvent<GameObject, Vector2>();
    [SerializeField] public static UnityEvent<GameObject, float, float> EventHitPlayer = new UnityEvent<GameObject, float, float>();
    [SerializeField] public static UnityEvent<GameObject> EventDie = new UnityEvent<GameObject>();
    [SerializeField] public static UnityEvent EventWin = new UnityEvent();
    [SerializeField] public static UnityEvent EventDebug = new UnityEvent();

    private static EventManager _instance;
    public static EventManager instance
    {
        get
        {
            
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType<EventManager>();
                if (!instance)
                {
                    throw new System.Exception("You need an EventManager attached to an object.");
                }
            }
            return _instance;
        }
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
