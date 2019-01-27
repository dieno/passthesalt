using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MIN_CHARGE = 0.25f;
    private const float MAX_CHARGE = 1.5f;

    [SerializeField]
    private Transform cameraTarget = null;

    private Rigidbody rb = null;

    //[SerializeField]
    //private float moveSpeed = 0.0f;

    private Plane groundPlane;

    private bool isAiming = false;
    private bool isShooting = false;
    private bool isMouseDown = false;

    [SerializeField]
    private GameObject arrowPrefab = null;

    private GameObject arrow = null;

    private Vector3 shootPosStart;
    private Vector3 shootPosEnd;

    private float chargeFactor = 0.25f;

    private bool hasClicked = false;

    private Vector3 target;

    private float chargeDistance = 0.0f;

    private Vector3 direction;

    private bool isSliding = false;
    public bool IsSliding { get; }

    [SerializeField]
    private float speed = 0.0f; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        groundPlane = new Plane(Vector3.up, Vector3.zero);

        shootPosStart = Vector3.zero;
        shootPosEnd = Vector3.zero;
        target = Vector3.zero;

        if (arrowPrefab)
        {
            float halfLength = GetComponent<MeshRenderer>().bounds.size.z / 2.0f;

            Transform arrowTransform = this.transform;

            Vector3 arrowPosition = this.transform.position;
            arrowPosition.z += halfLength;

            arrow = Instantiate(arrowPrefab, arrowPosition, arrowPrefab.transform.rotation, this.transform);
            arrow.transform.localScale = new Vector3(0.5f, arrow.transform.localScale.y, chargeFactor);
        }

        isAiming = true;
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        // follow player with camera
        cameraTarget.position = transform.position;

        if(!isSliding)
        {
            float dt = Time.deltaTime;

            if (Input.GetButton("Fire1"))
            {
                isShooting = true;
                isMouseDown = true;
            }
            else
            {
                isMouseDown = false;
            }

            Aim();

            if (isShooting)
            {
                Charge(dt);
            }

            // scale arrow
            arrow.transform.localScale = new Vector3(arrow.transform.localScale.x, arrow.transform.localScale.y, chargeFactor);
        }
    }

    private void Aim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;
        if (groundPlane.Raycast(ray, out distance))
        {
            target = ray.GetPoint(distance);

            direction = transform.position - target;
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
        }
    }

    private void Charge(float deltaTime)
    {
        if (isMouseDown)
        {
            if(!hasClicked)
            {
                shootPosStart = target;
                hasClicked = true;
            }

            shootPosEnd = target;

            float reductionFactor = 2.0f;

            chargeDistance = Vector3.Distance(shootPosEnd, shootPosStart) / reductionFactor;

            if (chargeDistance < MIN_CHARGE)
            {
                chargeDistance = MIN_CHARGE;
            }
            else if (chargeDistance > MAX_CHARGE)
            {
                chargeDistance = MAX_CHARGE;
            }

            chargeFactor = chargeDistance;
        }
        else
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        isSliding = true;
        hasClicked = false;
        arrow.SetActive(false);

        Vector3 movementVec = direction.normalized * chargeFactor * speed;
        movementVec.y = 0f;
        rb.AddForce(movementVec);

        Debug.Log(movementVec);
    }

}
