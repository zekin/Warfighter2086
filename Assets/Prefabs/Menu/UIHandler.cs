using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
//    private Label menuLab;
    private Button startButton;
    private Button exitButton;

    private int count;
    private void OnEnable()
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        startButton = rootVisualElement.Q<Button>("start-button");
        exitButton = rootVisualElement.Q<Button>("exit-button");
        startButton.RegisterCallback<ClickEvent>(ev => StartGame());
        exitButton.RegisterCallback<ClickEvent>(ev => ExitGame());
    }

    private void ExitGame()
    {
        Debug.Log("Exiting...");
        Application.Quit();
    }
    private void StartGame()
    {
        SceneManager.LoadScene("game");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
