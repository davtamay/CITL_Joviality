//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LeavesController : MonoBehaviour {
    
//    [Header("Leaf Settings")]
//    public int maxLeaves = 100;
//    public float startPercentages = 0.5f;
//    public float leavesThreshold = 0.4f;
//    public float minLeafDistance;
//    public GameObject leafPrefab;
//    public GameObject tree;
//    public GameObject[] optionalLeafPosistions;

//    [Header("Leaf Timer")]
//    public float minInitialLeafTimer = 2f;
//    public float maxInitialLeafTimer = 5f;
//    public float minLeafTimer = 5f;
//    public float maxLeafTimer = 10f;
//    public float minGrowBackTimer = 2f;
//    public float maxGrowBackTimer = 5f;

//    [Header("River Reference")]
//    public GameObject riverColliders;

//    private List<Vector3> vertices = new List<Vector3>();
//    private List<Vector3> normals = new List<Vector3>();
//    private List<GameObject> leaves = new List<GameObject>();

//    private float leafTimerChosen;
//    private float leafTimer;
//    private float growBackTimer;
//    private int growBackAmount;
//    private int leavesGrownBack;
//    private bool isLeafTimerRunning = false;
//    private bool isGrowBackTimerRunning = false;

//    public int amountOfLeaves;

//	// Use this for initialization
//	void Start () {
//		foreach (GameObject leafPosition in optionalLeafPosistions)
//        {
//            Mesh positionMesh = leafPosition.GetComponent<MeshFilter>().mesh;
//            Transform leafPositionTransform = leafPosition.transform;
//            Vector3[] meshVertices = positionMesh.vertices;
//            Vector3[] meshNormals = positionMesh.normals;
//            for(int i = 0; i < positionMesh.vertexCount; i++)
//            {
//                vertices.Add(leafPositionTransform.TransformPoint(meshVertices[i]));
//                normals.Add(leafPositionTransform.TransformDirection(meshNormals[i]));
//            }
//        }

//        int amountStartLeaves = (int)(maxLeaves * startPercentages);
//        for (int i = 0; i < amountStartLeaves; i++)
//        {
//            GrowLeaf();
//        }

//        amountOfLeaves = leaves.Count;
//        leafTimerChosen = Random.Range(minInitialLeafTimer, maxInitialLeafTimer);
//        leafTimer = leafTimerChosen;
//        growBackTimer = Random.Range(minGrowBackTimer, maxGrowBackTimer);
//	}
	
//	// Update is called once per frame
//	void Update () {
//		if(isLeafTimerRunning)
//        {
//            leafTimer -= Time.deltaTime;

//            if(leafTimer < 0)
//            {
//                Game.instance.DeactivateLoadCircle();
//                leafTimerChosen = Random.Range(minLeafTimer, maxLeafTimer);
//                leafTimer = leafTimerChosen;
//                Game.instance.ActivateTreeLoadCircle(leafTimerChosen);
//                MakeLeaveFall();
//            }
//        }
//        if(leaves.Count < (maxLeaves * leavesThreshold))
//        {
//            isGrowBackTimerRunning = true;
//            growBackAmount = Random.Range(0, maxLeaves - leaves.Count);
//            leavesGrownBack = 0;
//        }
//        if(isGrowBackTimerRunning)
//        {
//            growBackTimer -= Time.deltaTime;
//            if(growBackTimer < 0)
//            {
//                GrowLeaf();
//                leavesGrownBack++;
//                growBackTimer = Random.Range(minGrowBackTimer, maxGrowBackTimer);
//                if (leaves.Count < maxLeaves && leavesGrownBack >= growBackAmount)
//                {
//                    isGrowBackTimerRunning = false;
//                }
//            }
//        }
//	}

//    private void GrowLeaf()
//    {
//        if(amountOfLeaves <= maxLeaves)
//        {
//            int randomIndex = Random.Range(0, vertices.Count);
//            Vector3 leafPosition = vertices[randomIndex];
//            int watchdog = 0;
//            while (watchdog < 100 && IsLeafInNeighborhood(leafPosition))
//            {
//                randomIndex = Random.Range(0, vertices.Count);
//                leafPosition = vertices[randomIndex];
//                watchdog++;
//            }
//            Vector3 leafNormal = normals[randomIndex];
//            Quaternion leafRotation = Quaternion.FromToRotation(Vector3.up, leafNormal);
//            GameObject leaf = Instantiate(leafPrefab, leafPosition, leafRotation, tree.transform);
//            leaves.Add(leaf);
//            amountOfLeaves = leaves.Count;
//        }
//    }

//    private bool IsLeafInNeighborhood(Vector3 leafPosition)
//    {
//        foreach(GameObject existingLeaf in leaves)
//        {
//            if(Vector3.Distance(leafPosition, existingLeaf.transform.position) < minLeafDistance)
//            {
//                return true;
//            }
//        }
//        return false;
//    }

//    public void MakeLeaveFall()
//    {
//        int randomLeafIndex = Random.Range(0, leaves.Count);
//        GameObject leaf = leaves[randomLeafIndex];

//        Debug.Log("Chosen leaf y pos: " + leaf.transform.position.y);

//        FallingLeaf fallingController = leaf.AddComponent<FallingLeaf>();
//        fallingController.riverColliders = riverColliders;
//        leaves.Remove(leaf);
//        amountOfLeaves = leaves.Count;
//    }

//    public void OnTreeButtonEnter()
//    {
//        if (Game.instance.fallingLeavesEnabled)
//        {
//            int layerMask = LayerMask.GetMask("IgnoreCanvas");
//            RaycastHit[] hits;
//            hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, layerMask);

//            if (Game.ContainsSky(hits))
//            {
//                return;
//            }
//            Debug.Log("Tree: Entered the tree button!!!!");
//            Game.instance.ActivateTreeLoadCircle(leafTimerChosen);
//            isLeafTimerRunning = true;
//        }
//    }

//    public void OnTreeButtonExit()
//    {
//        Debug.Log("Exitted the tree button!!!");
//        Game.instance.DeactivateLoadCircle();
//        isLeafTimerRunning = false;
//        leafTimer = leafTimerChosen;
//    }
//}
