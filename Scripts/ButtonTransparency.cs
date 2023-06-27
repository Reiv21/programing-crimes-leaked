using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonTransparency : MonoBehaviour
{
    public bool isTransparency = true;
    public bool isFollowingPlayerCam = true;
    Camera cameraObj;
    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        cameraObj = FindObjectOfType<Camera>();

    }
    private void Update()
    {
        if(isFollowingPlayerCam)
        {
            transform.LookAt(cameraObj.transform); 
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        }
    }
}
