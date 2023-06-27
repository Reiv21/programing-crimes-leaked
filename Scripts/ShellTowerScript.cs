using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellTowerScript : MonoBehaviour
{
    float liveTime = 3f;
    void Update()
    {
        liveTime -= Time.deltaTime;
        if (liveTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
