using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //[SerializeField]
    //private Transform cameraTarget = null;

    private Rigidbody rb = null;

    //[SerializeField]
    //private float moveSpeed = 0.0f;

    private Plane groundPlane;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        Aim();
    }

    private void Aim()
    {
        //if (isAttacking && Time.time > nextAttack)
        //{
        //    isAttacking = false;
        //}

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;
        if (groundPlane.Raycast(ray, out distance))
        {
            Vector3 target = ray.GetPoint(distance);

            //Vector3 direction = target - transform.position;
            Vector3 direction = transform.position - target;
            float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, rotation, 0);

            // draw debug crosshair
            Vector3 line1start = target;
            line1start.x -= 0.5f;

            Vector3 line1end = target;
            line1end.x += 0.5f;

            Vector3 line2start = target;
            line2start.z -= 0.5f;

            Vector3 line2end = target;
            line2end.z += 0.5f;
            Debug.DrawLine(line1start, line1end, Color.red);
            Debug.DrawLine(line2start, line2end, Color.red);

            Debug.Log(target);

            //if (Input.GetButtonDown("Fire1")  && Time.time > nextAttack)
            //{
            //    nextAttack = Time.time + attackRate;
            //    isAttacking = true;
            //    Attack();
            //}

            //if (Input.GetButton("Fire1"))
            //{
            //    flame.enabled = true;
            //}
            //else
            //{
            //    flame.enabled = false;
            //}
        }
    }
}
