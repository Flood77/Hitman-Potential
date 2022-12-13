using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private BoxCollider2D mapBounds;

    private float xMin, xMax, yMin, yMax;
    private float camOrthsize;
    private float cameraRatio;

    private void Start()
    {
        //Set bounds of the camera to the collision box
        xMin = mapBounds.bounds.min.x;
        xMax = mapBounds.bounds.max.x;
        yMin = mapBounds.bounds.min.y;
        yMax = mapBounds.bounds.max.y;
        mapBounds.enabled = false;

        //store camera values
        camOrthsize = GetComponent<Camera>().orthographicSize;
        cameraRatio = (xMax + camOrthsize) / 2.0f;
    }

    void FixedUpdate()
    {
        //move camerafollow and lock with the bounds
        this.transform.position = Vector3.Lerp(this.transform.position, 
            new Vector3(Mathf.Clamp(player.position.x, xMin + cameraRatio, xMax - cameraRatio), 
                        Mathf.Clamp(player.position.y, yMin + camOrthsize, yMax - camOrthsize), 
                        this.transform.position.z), 0.02f);
    }
}
