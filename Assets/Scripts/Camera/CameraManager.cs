using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform cameraOrigin;

    public float MovementSpeed;
    public float MovementTime;
    public float RotationAmount;
    public Vector3 zoomAmount;

    public Vector3 NewPosition;
    public Quaternion NewRotation;
    public Vector3 newZoom;

    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    [Header ("Testing")]
    //TESTING
    public Collider col;
    public float MaxLength;
    public Vector3 offset;

    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;
    public float minY;
    public float maxY;

    // Start is called before the first frame update
    void Start()
    {
        NewPosition = transform.position; //So that our transform doesn't automatically default to 0.
        NewRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
        HandleMouseInput();

        //Clamp of Vector NewPosition x & z.
        NewPosition.x = Mathf.Clamp(NewPosition.x, minX, maxX);
        NewPosition.z = Mathf.Clamp(NewPosition.z, minZ, maxZ);

        //Clamp of Vector NewZoom x & z.
        //cameraTransform.position.y = Mathf.Clamp(cameraTransform.position.y, minY, maxY);
        newZoom.y = Mathf.Clamp(newZoom.y, minY, maxY);

        //Debug.Log(NewPosition.x);
        //Debug.Log(NewPosition.magnitude);
    }

    void HandleMouseInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                NewPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            NewRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
    }

    void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            NewPosition += (transform.forward * MovementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            NewPosition += (transform.forward * -MovementSpeed);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            NewPosition += (transform.right * MovementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            NewPosition += (transform.right * -MovementSpeed);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            NewRotation *= Quaternion.Euler(Vector3.up * RotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            NewRotation *= Quaternion.Euler(Vector3.up * -RotationAmount);
        }

        if(Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }


        transform.position = Vector3.Lerp(transform.position, NewPosition, Time.deltaTime * MovementTime); // NEEDS TO BE CLAMPED (Vector NewPosition)
        transform.rotation = Quaternion.Lerp(transform.rotation, NewRotation, Time.deltaTime * MovementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * MovementTime); // NEEDS TO BE CLAMPED
    }
}
