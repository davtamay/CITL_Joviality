using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingLeaf : MonoBehaviour {
    public GameObject riverColliders;
    public float floatYValue = -0.15f;

    private GameObject leafParent;

    private bool isFloating = false;
    public Vector3 floatDirection;
    private Vector3 floatStartPoint;
    private Quaternion floatStartRotation;
    public float floatForce = 5f;

	// Use this for initialization
	void Start () {
        leafParent = transform.parent.gameObject;

        transform.parent = null;
        Destroy(leafParent);

        floatStartRotation = Quaternion.Euler(0f, Random.Range(0, 360), 90f);
        floatStartPoint = new Vector3(transform.position.x, floatYValue, transform.position.z);
        gameObject.layer = 0;

        floatDirection = riverColliders.transform.GetChild(0).forward;
        //Invoke("PrepareFloating", 1f);
	}

    private void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, floatStartRotation, 90 * Time.deltaTime);
        if (!isFloating)
        {
            transform.position = Vector3.MoveTowards(transform.position, floatStartPoint, 10 * Time.deltaTime);
            if (transform.position == floatStartPoint)
            {
                isFloating = true;
            }
        }
        else
        {
            transform.position += floatDirection * floatForce * Time.deltaTime;
        }
        

        /*
        if (isFloating)
        {
            rb.AddForce(floatDirection * floatForce, ForceMode.Force);
        }*/
    }
}
