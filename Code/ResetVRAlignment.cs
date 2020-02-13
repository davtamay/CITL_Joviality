using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetVRAlignment : MonoBehaviour
{
    public Transform point;
    public Transform myTransformOffset;
    public Transform myTransform;
    public Vector3 newPos;
    public Vector3 newRot;
    public bool resetOnStart = true;
    public float resetOnStartDelay = .8f;
    public KeyCode resetKeyBind = KeyCode.Space;

    void Start()
    {
        //if (myTransform == null)
        //    myTransform = transform;

        Debug.Log("Start of ResetVRAlignment is running");
        if (newRot.y!=0)
        {
            myTransform.eulerAngles = newRot;
            myTransform.position = newPos;
        }
        else if (resetOnStart)
        {
            Invoke("ResetMe", resetOnStartDelay);
        }
    }

    // Update is called once per frame
    public void Update () {

        if(Input.GetKeyDown(resetKeyBind))
        {
            ResetMe();
        }
    }

    public void ResetMe()
    {
        Vector3 rot = myTransform.eulerAngles;
        rot.y -= myTransformOffset.eulerAngles.y-point.eulerAngles.y;

        newRot = rot;
        myTransform.eulerAngles = newRot;

        Rigidbody rb = myTransform.GetComponent<Rigidbody>();
        if (rb != null) rb.velocity *= 0;

        Vector3 dist = point.position-myTransformOffset.position;
        myTransform.position += dist;

        newPos = myTransform.position;
    }
}
