using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestPathFinder : MonoBehaviour
{
    [Header("Start And Destination ")]
    public Node Startnode;
    public Node EndNode;
    
    private Path p;
    private Graph G;
    // Start is called before the first frame update
    private void Awake()
    {
        G = gameObject.AddComponent<Graph>();
        Startnode = gameObject.AddComponent<Node>();
        EndNode = gameObject.AddComponent<Node>();
        p=new Path();
    }

    // Update is called once per frame
    void Update()
    {
        if (Startnode != null && EndNode != null)
        {
            p.printpath(G.FindShortestPath(Startnode, EndNode));
        }    
    }
    
    
}
