using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonsInteraction : MonoBehaviour
{

    [SerializeField] public string myScene;
    void Start()
    {
        //OnMouseDown();
        //OnMouseOver();
        //OnMouseExit();
    }

    void OnMouseDown()
    {
        Debug.Log("BUTTON PRESSED");
        SceneManager.LoadScene(myScene);
        //if play button, load game scene
        //if credits, open credit menu
        //if quit, quit the game
        //if options, load options menu 
    }

    void OnMouseOver()
    {
        Debug.Log("HIGHLIGHTED");
    }

    void OnMouseExit()
    {
        Debug.Log("UNHIGHLIGHTED");
    }

}