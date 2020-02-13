using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCollider : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        FloatingLeaf leaf = other.GetComponent<FloatingLeaf>();
        if (leaf != null)
        {
            Destroy(leaf.gameObject);
        }
    }
}
