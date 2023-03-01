using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private Node nextNode;

	//Change enemy's objective node
    private void OnTriggerEnter2D(Collider2D collision)
	{
		//Check collided is an enemy
		var enemy = collision.gameObject.GetComponent<Enemy>();
		if (enemy != null)
		{
			//Check that it was looking for this node
			var n = enemy.Node;
			if (n == this)
			{
				//Activate next Node
				nextNode.gameObject.SetActive(true);

				//Switch enemy's current objective node
				enemy.Node = nextNode;

				//Deactivate current Node
				this.gameObject.SetActive(false);
			}
		}
	}
}