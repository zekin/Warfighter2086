using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    // Start is called before the first frame update
    bool isAnimating = false;
    bool closed = false;
    LTDescr open;
    LTDescr close;
    void Start()
    {
        
    }

    public void setClosed()
    {
        closed = true;
        
//        GetComponent<Light>().
    }
    // Update is called once per frame
    void finished()
    {
        isAnimating = false;
    }
    void Update()
    {
//        if (open != null)
//            open.callOnCompletes();
//        if (close != null)
//            close.callOnCompletes();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"I entered here {other.tag}"); //issue is it occurs twice??
        if (other.tag == "Player" && isAnimating == false && closed == false)
        {
            open = LeanTween.moveY(gameObject, 5f, 2f).setEaseOutQuad();
            open.setOnComplete(finished);
            isAnimating = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && isAnimating == false)
        {
            close = LeanTween.moveY(gameObject, 0f, 2f).setEaseOutQuad();
            close.setOnComplete(finished);
            isAnimating = true;
        }
    }
}
