using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform cameraTransform;

    public float MovementSpeed;
    public float MovementTime;
    public float RotationAmount;
    public Vector3 zoomAmount;

    public Vector3 NewPosition;
    public Quaternion NewRotation;
    public Vector3 newZoom;

    // Start is called before the first frame update
    void Start()
    {
        NewPosition = transform.position; //So that our transforme doesn't automatically default to 0.
        NewRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
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

        transform.position = Vector3.Lerp(transform.position, NewPosition, Time.deltaTime * MovementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, NewRotation, Time.deltaTime * MovementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * MovementTime);
    }
}
