using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] bool enableDebug = false;

    // Start is called before the first frame update
    void Start()
    {

        if (enableDebug)
        {
            EventManager.EventDebug.Invoke(); //fill in debug classes maybe

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
