using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class WheelEngine : MonoBehaviour
{
    [Header("Torque/break")]
    public float torqueForce = 1000;
    public float maxSpeed = 1;
    public float stoppingCoef = 5;

    [Header("Suspension")]
    public float wheelRadius;
    public float stringStrength;
    public float stringDumper;

    private Vector3 springDir = Vector3.up;

    [Header("Steering")]
    public AnimationCurve steeringCurve;
    public float maxRotationAngle = 30;
    public float wheelMass = 20;
    public bool isSteering = true;


    private Rigidbody carRb;
    private Transform carTransform;
    private Transform wheelTransform;
    private float horizontalInput;
    private float verticalInput;
    private StickControl stick;

    private void Start()
    {
        wheelTransform = transform;
        carRb = GetComponentInParent<Rigidbody>();
        carTransform = carRb.transform;
        stick = Gamepad.current.leftStick;
    }

    private void Update()
    {
        if (Gamepad.current.IsActuated())
        {
            horizontalInput = stick.x.ReadValue();
            verticalInput = stick.y.ReadValue();
        }
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(wheelTransform.position, -Vector3.up, out hit, wheelRadius))
        {
            Suspension(hit);
            Acceleration();
            Steering();
        }
    }

    private void Steering()
    {
        if (isSteering)
            wheelTransform.transform.eulerAngles = new Vector3(0,
                carRb.rotation.eulerAngles.y + horizontalInput * maxRotationAngle, 0);
        Vector3 wheelRightDir = wheelTransform.right;
        Vector3 wheelWorldVel = carRb.GetPointVelocity(wheelTransform.position);
        float wheelRightVel = Vector3.Dot(wheelWorldVel, wheelRightDir);
        float wheelRightAccel = wheelRightVel / Time.fixedDeltaTime;
        float force = wheelRightAccel * wheelMass * (1 - steeringCurve.Evaluate(wheelRightVel / wheelWorldVel.magnitude));
        carRb.AddForceAtPosition(-force * wheelRightDir, wheelTransform.position);
    }

    private void Acceleration() 
    {
        Vector3 wheelDir = wheelTransform.forward;
        float force = torqueForce * verticalInput;
        float forwardVelocity = Vector3.Dot(carRb.velocity, wheelDir);
        if (!(forwardVelocity < -maxSpeed && force < 0 || forwardVelocity > maxSpeed && force > 0))
            carRb.AddForceAtPosition(force * wheelDir, wheelTransform.position);
        float stoppingForce = forwardVelocity / maxSpeed / stoppingCoef * torqueForce;
        carRb.AddForceAtPosition(stoppingForce * -wheelDir, wheelTransform.position);
    }

    private void Suspension(RaycastHit hit)
    {
            float offset = wheelRadius - hit.distance;
            float force = offset * stringStrength;

            Vector3 wheelVel = carRb.GetPointVelocity(wheelTransform.position);
            float wheelVertVel = Vector3.Dot(springDir, wheelVel);
            force -= wheelVertVel * stringDumper;

            carRb.AddForceAtPosition(force * springDir, wheelTransform.position);   
    }
}
