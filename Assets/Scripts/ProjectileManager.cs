using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject projectileSpawner;
    public static List<GameObject> projectile = new List<GameObject>();

    private void OnEnable()
    {
        EventManager.EventFireGun.AddListener(HandleFireGun);
    }
    private void OnDisable()
    {
        EventManager.EventFireGun.RemoveListener(HandleFireGun);
    }
    private void HandleFireGun(GameObject source, Vector2 direction)
    {
        GameObject proj = Instantiate(projectileSpawner, source.transform.position+Vector3.up*1.5f, source.transform.rotation);

        projectile.Add(proj);

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(source.transform.forward.x*50f, 0f, source.transform.forward.z*50f), ForceMode.Impulse);
//        Invoke("remove", 3f);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
