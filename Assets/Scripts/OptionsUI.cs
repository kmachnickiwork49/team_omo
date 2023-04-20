using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    public GameObject OptionsScreen;
    public GameObject MenuScreen;

    public void openOptions()
    {
        OptionsScreen.SetActive(true);
        MenuScreen.SetActive(false);
    }

    public void closeOptions()
    {
        OptionsScreen.SetActive(false);
        MenuScreen.SetActive(true);
    }
}
