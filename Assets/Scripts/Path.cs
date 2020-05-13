using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Path
{
    public List<Node> nodes = new List<Node>();
    public float PathLength = 0f;
    public void CalculateDistance()
    {
        List<Node> calculatedNodes = new List<Node>();
        PathLength = 0f;
        for (int i = 0; i < nodes.Count; i++)
        {
            Node node = nodes[i];
            for (int j = 0; j < node.links.Count; j++)
            {
                Node connection = node.links[j];
                if (nodes.Contains(connection) && !calculatedNodes.Contains(connection))
                {
                    PathLength += Vector3.Distance(node.transform.position, connection.transform.position);
                }
            }
            calculatedNodes.Add(node);
        }
    }

    public void printpath(Path p)
    {
        foreach (Node i in p.nodes)
        {
            Debug.Log(i.transform.position + i.name);
        }
    }
}