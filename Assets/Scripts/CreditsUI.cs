using System;
using System.Collections;
using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    public GameObject CreditsScreen;
    public GameObject MenuScreen;

    public void openCredits()
    {
        CreditsScreen.SetActive(true);
        MenuScreen.SetActive(false);
    }

    public void closeCredits()
    {
        CreditsScreen.SetActive(false);
        MenuScreen.SetActive(true);
    }
}