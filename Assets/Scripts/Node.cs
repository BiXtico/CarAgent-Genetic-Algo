using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Node : MonoBehaviour
{
    [SerializeField]
    public List<Node> links = new List<Node>();
}