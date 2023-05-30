using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    private double timeSinceDestroyed = 0;

    [SerializeField] FallingPlatform fp;
    //[SerializeField] private RigidBody2d rb;

    //[SerializeField] [Range(0, 10)] private float fallDelay = 1f;
    //[SerializeField] [Range(0, 10)] private float destroyDelay = 2f;

    [SerializeField] private bool respawn = false;
    [SerializeField][Range(0, 10)] private double respawnTimer = 10;

    void Start()
    {
        //Vector3 pos = transform.position;
        //enabled = true;
    }

    private void Update()
    {
        if (respawn == true && fp.getEnabeled() == false)
        {
            if (timeSinceDestroyed > respawnTimer)
            {
                EnablePlatform();
                timeSinceDestroyed = 0;
            }
            else
            {
                print("Time Since Destroyed: " + timeSinceDestroyed);
                timeSinceDestroyed += 0.001;
            }
        }
    }

    void EnablePlatform()
    {
        fp.gameObject.SetActive(true);
        fp.setEnabeled();
    }
}
