using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new Vector3(-11f, 14.5f, -9f);
    [SerializeField] Vector3 debugTargetPosition = new Vector3(0f, 20f, 0f);
    [SerializeField] Vector3 debugOffset = new Vector3(0f, 0f, 0f);
    private void DebugHandler()
    {

        GameObject targetObj = new GameObject();
        targetObj.transform.position = debugTargetPosition;
        offset = debugOffset;

        target = targetObj.transform;
        transform.position = targetObj.transform.position;
        transform.forward = new Vector3(0f, -1f, 0f);
//        .GetComponent<Camera>().transform.position = new Vector3(0f, 20f, 0f);
//        target.GetComponent<Camera>().transform.forward = new Vector3(0f, -1f, 0f);
    }
    private void OnEnable()
    {
        EventManager.EventDebug.AddListener(DebugHandler);
    }
    private void OnDisable()
    {
        EventManager.EventDebug.RemoveListener(DebugHandler);
    }
    void Start()
    {
        //        target = new GameObject().transform;//GameObject.Find("Player").transform;
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}
