using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    bool spawned = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && spawned == false)
        {
            float randomNumber = UnityEngine.Random.Range(0f, 1f);
            if (randomNumber > .65f)
            {
                Instantiate(GameObject.Find("Alien"), gameObject.transform.position, Quaternion.identity);

            }
            else if (randomNumber > .3f)
            {
                Instantiate(GameObject.Find("Bug"), gameObject.transform.position, Quaternion.identity);

            }

            spawned = true;
        }
    }
}
