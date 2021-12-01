using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    // Start is called before the first frame update
    private Light light;
    void Start()
    {
        light = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        light.intensity = ((Mathf.PerlinNoise(0.2f + Time.fixedTime * 0.2f, 0.2f) - .5f) + (1f + Mathf.Sin(4f * Time.fixedTime * 0.3f * Mathf.PI))) * 20f+20f;
    }
}
