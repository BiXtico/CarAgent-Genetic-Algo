using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Graph : MonoBehaviour
{
	
    [SerializeField]
    public List<Node> nodes = new List<Node> ();
    public  Path FindShortestPath ( Node source, Node destination )
    {
        Path path = new Path ();
        if (source == destination)
        {
            path.nodes.Add (source);
            return path;
        }
        List<Node> unvisited = new List<Node> ();
        Dictionary<Node, Node> previous = new Dictionary<Node, Node> ();
        Dictionary<Node, float> distances = new Dictionary<Node, float> ();
		
        for ( int i = 0; i < nodes.Count; i++ )
        {
            Node node = nodes [ i ];
            unvisited.Add ( node );
            distances.Add ( node, float.MaxValue );
        }
        distances [source] = 0f;
        while ( unvisited.Count != 0 )
        {
            unvisited = unvisited.OrderBy ( node => distances [ node ] ).ToList ();
            Node current = unvisited [ 0 ];
			
            unvisited.Remove ( current );
            if ( current == destination )
            {
                while ( previous.ContainsKey ( current ))
                {
                    path.nodes.Insert ( 0, current );
                    current = previous [ current ];
                }
                path.nodes.Insert ( 0, current );
                break;
            }
            for ( int i = 0; i < current.links.Count; i++ )
            {
                Node neighbor = current.links[ i ];
                float length = Vector3.Distance ( current.transform.position, neighbor.transform.position );
                float alt = distances [ current ] + length;
                if ( alt < distances [ neighbor ] )
                {
                    distances [ neighbor ] = alt;
                    previous [ neighbor ] = current;
                }
            }
        }
        path.CalculateDistance ();
        return path;
    }
	
}