using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowChanger : MonoBehaviour
{
    public float cameraDisplace;
    public int musicIndex;
    public delegate void FlowDelegate(float camDis,int musicIndex);
    public static event FlowDelegate flowDelegate;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            flowDelegate.Invoke(cameraDisplace,musicIndex);
        }
    }
}
