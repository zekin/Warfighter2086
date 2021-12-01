using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("BackToMainMenu", 20f);    
    }
    void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<Text>().transform.Translate(0, -50f * Time.deltaTime, 0);

    }
}
