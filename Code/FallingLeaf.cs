using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingLeaf : MonoBehaviour {
    public GameObject leafParent;
    public GameObject riverColliders;

    private Vector3 leafParentOffset = Vector3.up * 5.0f;

    private float minRotationSpeed = 1f;
    private float maxRotationSpeed = 2f;
    private float rotationSpeed;

    private float maxRotation = 45f;

    private Vector3 destinationPoint;

    private float fallingSpeed = 10f;

    //private float minDownForce = 0.05f;
    //private float maxDownForce = 0.2f;
    //private float downForce;

    private float startTime;
    private float destroyTime = 2f;

    //private float minLeafFadeHeight = 5f;
    //private float maxLeafFadeHeight = 10f;
    //public float leafFadeHeight;
    //public float leafFadeRange;
    //public float leafHeight;

    private Material leafMaterial;

    private bool isFalling;

    // Use this for initialization
    void Start () {
        leafParent = new GameObject("FallingLeaf");

        leafParent.transform.position = gameObject.transform.position + leafParentOffset;

        destinationPoint = GetRandomDestinationPoint();
        leafParent.transform.LookAt(new Vector3(destinationPoint.x, leafParent.transform.position.y, destinationPoint.z));
        Debug.Log("StartRot:" + leafParent.transform.rotation.eulerAngles);
        //leafParent.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        gameObject.transform.SetParent(leafParent.transform, true);
        
        /*
        leafHeight = leafParent.transform.position.y - leafFadeHeight - leafParentOffset.y;
        leafFadeHeight = Random.Range(minLeafFadeHeight, maxLeafFadeHeight);
        leafFadeRange = leafHeight - leafFadeHeight;
        */
        leafMaterial = transform.GetChild(0).GetComponent<Renderer>().material;
        
        startTime = Time.time;
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        //downForce = Random.Range(minDownForce, maxDownForce);

        isFalling = true;
    }
	
	// Update is called once per frame
	void Update () {
        if(isFalling)
        {
            float deltaTime = Time.time - startTime;
            leafParent.transform.rotation = Quaternion.Euler(maxRotation * Mathf.Sin(deltaTime * rotationSpeed), leafParent.transform.rotation.eulerAngles.y, leafParent.transform.rotation.eulerAngles.z);

            leafParent.transform.position = Vector3.MoveTowards(leafParent.transform.position, destinationPoint, fallingSpeed * Time.deltaTime);

            /*leafParent.transform.position += Vector3.down * downForce * deltaTime;
            
            leafHeight = leafParent.transform.position.y - leafFadeHeight - leafParentOffset.y;
            float leafAplha = (leafHeight / leafFadeRange);
            Color color = leafMaterial.GetColor("_Color");
            color.a = leafAplha;
            leafMaterial.SetColor("_Color", color);
            */
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isFalling = false;
        gameObject.GetComponent<Collider>().isTrigger = false;
        FloatingLeaf floatingController = gameObject.AddComponent<FloatingLeaf>();
        floatingController.riverColliders = riverColliders;
        Destroy(this);
        //Destroy(leafParent, destroyTime);
    }

    private Vector3 GetRandomDestinationPoint()
    {
        Collider startCollider = riverColliders.transform.GetChild(0).GetComponent<Collider>();
        Bounds colliderBounds = startCollider.bounds;
        Vector3 destinationPoint = new Vector3(
            Random.Range(colliderBounds.min.x, colliderBounds.max.x),
            colliderBounds.min.y,
            Random.Range(colliderBounds.min.z, colliderBounds.max.z)
        );
        destinationPoint = startCollider.ClosestPoint(destinationPoint);

        destinationPoint = destinationPoint + leafParentOffset;

        return destinationPoint;
    }
}
