using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float lookSensitivity = 3f;

    [SerializeField] GameObject fpsCamera;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float CameraUpDownRotation = 0f;
    private float CurrentCameraUpDownRotation = 0f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Calculate movement velocity as a 3D vector
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        Vector3 movementHorizontal = transform.right * xMovement;
        Vector3 movementVertical = transform.forward * zMovement;

        Vector3 movementVelocity = (movementHorizontal + movementVertical).normalized * speed;

        //Apply movement
        Move(movementVelocity);

        //Calculate rotation as a 3D vector (turning around)
        float yRotation = Input.GetAxis("Mouse X");

        Vector3 rotationVector = new Vector3(0f, yRotation, 0f) * lookSensitivity;

        //Apply rotation
        Rotate(rotationVector);

        //Calculate camera rotation as a 3D vector (turning around)
        float cameraUpDownRotation = Input.GetAxis("Mouse Y") * lookSensitivity;

        //Apply camera rotation
        RotateCamera(cameraUpDownRotation);

    }

    private void FixedUpdate()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (fpsCamera != null)
        {
            CurrentCameraUpDownRotation -= CameraUpDownRotation;
            CurrentCameraUpDownRotation = Mathf.Clamp(CurrentCameraUpDownRotation, -60f, 60f);
            fpsCamera.transform.localEulerAngles = new Vector3(CurrentCameraUpDownRotation, 0f, 0f);
        }

    }

    void Move(Vector3 movementVelocity)
    {
        velocity = movementVelocity;
    }

    void Rotate(Vector3 rotationVector)
    {
        rotation = rotationVector;
    }

    void RotateCamera(float cameraUpDownRotation)
    {
        CameraUpDownRotation = cameraUpDownRotation;
    }
}
