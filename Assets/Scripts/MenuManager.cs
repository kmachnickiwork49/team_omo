using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject HUDMenu;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject option1Button;
    [SerializeField] private GameObject option2Button;

    public void PauseButton()
    {
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
        HUDMenu.SetActive(false);
    }

    public void ResumeButton()
    {
        Time.timeScale = 1.0f;
        PauseMenu.SetActive(false);
        HUDMenu.SetActive(true);
    }
}
