using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{
    [Header("Transforms")]
    public Transform cameraTransform;
    public Transform cameraOrigin;

    [Header("Movement speed")]
    public float MovementSpeed;
    public float MovementTime;
    public float RotationAmount;
    public Vector3 zoomAmount;

    [Header("Vectors")]
    //IF YOU WANT TO TWEAK THE CAMERA BOUNDS, PUT THESE IN PUBLIC !!!
    private Vector3 NewPosition;
    private Quaternion NewRotation;
    private Vector3 newZoom;

    [Header("Checks position")]
    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;
    private Vector3 rotateStartPosition;
    private Vector3 rotateCurrentPosition;

    [Header ("Limits")]
    private Vector3 offset;
    private float offsetLength;

    //Clamp of position X
    public float minX;
    public float maxX;
    //Clamp of position Z
    public float minZ;
    public float maxZ;

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

        //Limits of Vector NewZoom x & z.
        Vector3 offset = cameraTransform.position - cameraOrigin.position;
        offsetLength = offset.magnitude;

        //These if conditions act as a way to prevent the camera from speeding outside the limits put in the HandleMovementInput().
        if (offsetLength <= 27)
        {
            newZoom -= zoomAmount;
        }

        if (offsetLength >= 110)
        {
            newZoom += zoomAmount;
        }
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
            if(offsetLength >= 28)
            {
                newZoom += zoomAmount;
            }

        }
        if (Input.GetKey(KeyCode.F))
        {
            if (offsetLength <= 110)
            {
                newZoom -= zoomAmount;
            }
        }

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagniude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = prevMagniude - currentMagnitude; //I can try to compare the base position

            if (prevMagniude > currentMagnitude || offsetLength <= 110)
            {
                //Dézoom
                newZoom -= zoomAmount;
            }

            if (prevMagniude < currentMagnitude || offsetLength >= 28)
            {
                //Zoom
                newZoom += zoomAmount;
            }

            //ZoomMobile(difference * 0.01f);
        }


        transform.position = Vector3.Lerp(transform.position, NewPosition, Time.deltaTime * MovementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, NewRotation, Time.deltaTime * MovementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * MovementTime);
    }

    /*void ZoomMobile(float increment)
    {
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * MovementTime); // NEEDS TO BE CLAMPED
    }*/
}
