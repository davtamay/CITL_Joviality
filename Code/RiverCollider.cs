using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverCollider : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        FloatingLeaf leaf = other.GetComponent<FloatingLeaf>();
        if (leaf != null)
        {
            leaf.floatDirection = transform.forward;
        }
    }
}
