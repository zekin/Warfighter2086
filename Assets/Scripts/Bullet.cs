using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public float damagePerBullet = 5f;
    [SerializeField] public float lifetime = 3f;
    // Start is called before the first frame update
    private float lifetimeStart = 0f;
    private float duration;
    void Start()
    {
        lifetimeStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        duration = Time.time - lifetimeStart;

        GetComponent<Light>().intensity = ((Mathf.PerlinNoise(0.2f+duration*10f,0.2f)-.5f) + (1f+Mathf.Sin(4f*duration*2f*Mathf.PI)))*.5f;
        if (duration > lifetime)
            Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject creature = other.gameObject;
        if (creature.tag != "Monster")
            return;

        Unit unit = creature.gameObject.GetComponent<Unit>();
        unit.damage(damagePerBullet);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Level")
        {
            Destroy(this);
        }
    }
}
