using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private Node nextNode;

    private void OnTriggerEnter2D(Collider2D collision)
	{
		var enemy = collision.gameObject.GetComponent<Enemy>();
		if (enemy != null)
		{
			var n = enemy.Node;
			if (n == this)
			{
				enemy.Node = nextNode;
			}
		}
	}
}