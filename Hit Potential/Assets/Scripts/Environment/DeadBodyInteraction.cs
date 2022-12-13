using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodyInteraction : MonoBehaviour
{
    [SerializeField] private GameObject corpseClothes;



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Instantiate(corpseClothes, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }
        }
    }
}
