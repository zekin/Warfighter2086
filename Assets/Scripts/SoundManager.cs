using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    //[Serializable] Dictionary<string, AudioSource> sounds;
    [SerializeField] AudioSource alienDeath;
    [SerializeField] AudioSource projectileFire;

    private void OnEnable()
    {
        EventManager.EventDie.AddListener(handleDeath);
    }
    private void OnDisable()
    {
        EventManager.EventDie.RemoveListener(handleDeath);
    }
    private void handleDeath(GameObject obj)
    {
        AudioClip c = obj.GetComponent<Unit>().deathSound;

        GameObject audio = new GameObject("Audio");
        AudioSource asource = audio.AddComponent<AudioSource>();
        asource.clip = c;
        asource.Play();
        Destroy(audio, 3f);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
