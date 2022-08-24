using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private Node nextNode;

    public Node getNext()
    {
        return nextNode;
    }
}