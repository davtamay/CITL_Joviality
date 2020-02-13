using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleBehaviour : MonoBehaviour {

    private RaycastHit hit;
    private float distance;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
       
    }

    private void Update()
    {
        int layerMask = ~LayerMask.GetMask("IgnoreCanvas");
        if (Camera.main != null)
        {
            //When a collider is hit set the distance to the distance of the collider else set the distance to be far away
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000f, layerMask))
            {
                distance = hit.distance - 0.05f;
                //transform.rotation = Quaternion.LookRotation(hit.normal);
            }
            else
            {
                distance = Camera.main.farClipPlane * 0.95f;
                /*
                transform.LookAt(Camera.main.transform.position);
                transform.Rotate(0.0f, 0f, 0.0f, Space.Self);
                */
            }

            //Set the new cross hair position based on the distance
            transform.position = Camera.main.transform.position + (Camera.main.transform.forward * distance);
            
            //Scale the cross hair so it's the same size even when it's in the distance.
            transform.localScale = originalScale * distance;
        }
        
    }

    
	
}
