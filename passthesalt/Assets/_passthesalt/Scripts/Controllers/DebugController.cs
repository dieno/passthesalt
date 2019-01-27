using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTarget = null;

    private Rigidbody rb;

    [SerializeField]
    private float moveSpeed = 0.0f;

    private Plane groundPlane;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        Move(dt);
    }
    private void Move(float dt)
    {
        //rb.velocity = Vector3.zero;

        // get axes relative to camera along the ground plane
        Vector3 horizontalAxis = Camera.main.transform.right;
        Vector3 verticalAxis = Vector3.Normalize(Vector3.ProjectOnPlane(Camera.main.transform.forward, groundPlane.normal));

        // calculate final movement vector
        Vector3 horizontalAxisMovement = horizontalAxis * Input.GetAxis("Horizontal");
        Vector3 verticalAxisMovement = verticalAxis * Input.GetAxis("Vertical");
        Vector3 movementVec = Vector3.Normalize(horizontalAxisMovement + verticalAxisMovement);

        //// incorporate movement speed and frame time
        movementVec *= moveSpeed;// * dt;

        //Debug.Log(movementVec);

        //// allows us to set the position and bypass acceleration while keeping default collision response
        //rb.MovePosition(transform.position + movementVec);

        rb.AddForce(movementVec);

        // debug draw the movement axes
        //Debug.DrawRay(transform.position, horizontalAxis, Color.magenta);
        //Debug.DrawRay(transform.position, -horizontalAxis, Color.yellow);

        //Debug.DrawRay(transform.position, verticalAxis, Color.red);
        //Debug.DrawRay(transform.position, -verticalAxis, Color.blue);

        // follow player with camera
        cameraTarget.position = transform.position;
    }


}
