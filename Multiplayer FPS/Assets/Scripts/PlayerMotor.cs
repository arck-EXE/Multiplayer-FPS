using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    Vector3 Velocity = Vector3.zero;
    Vector3 rotation = Vector3.zero;
    Vector3 thrusterForce = Vector3.zero;


    float cameraRotationX = 0f;
    float currentcameraRotationX = 0f;
    float cameraRotationLimit = 85f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity)
    {
        Velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCamera(float _cameraRotaionX)
    {
        cameraRotationX = _cameraRotaionX;
    }

    public void ApplyThruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }

    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    private void PerformMovement()
    {
        if (Velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + Velocity * Time.fixedDeltaTime);
        }

        if(thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if(cam != null)
        {
            currentcameraRotationX -= cameraRotationX;
            currentcameraRotationX = Mathf.Clamp(currentcameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            cam.transform.localEulerAngles = new Vector3(currentcameraRotationX, 0f, 0f);

        }
    }
}
