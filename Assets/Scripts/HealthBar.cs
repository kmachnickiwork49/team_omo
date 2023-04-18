using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image healthBarImage;
    [SerializeField] GameObject player;
    private PlayerController playerScript;
    

    void Start()
    {
        playerScript = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        healthBarImage.fillAmount = Mathf.Clamp(playerScript.health / playerScript.maxHealth, 0, 1f);
    }
}
