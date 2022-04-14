using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float VerticalInput;
    private float CurrentSteerAngle;
    public float currentbreakForce;
    public bool isBreaking;



    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float motorforce;
    [SerializeField] private float breakforce;

    [SerializeField] private WheelCollider frontLeftWheel;
    [SerializeField] private WheelCollider frontRightWheel;
    [SerializeField] private WheelCollider backLeftWheel;
    [SerializeField] private WheelCollider backRightWheel;

    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform frontRightTransform;
    [SerializeField] private Transform backLeftTransform;
    [SerializeField] private Transform backRightTransform;

    [SerializeField] private Joystick joystick;
    [SerializeField] private LongClickButton brakeButton;
    [SerializeField] private AccButton accelerate;
    [SerializeField] private AccButton reverse;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();

      //  Debug.Log(currentbreakForce + " " + isBreaking);
       
    }

   

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        // VerticalInput = Input.GetAxis(VERTICAL);
      

        if(!accelerate.isPressed)
        {
            VerticalInput = reverse.isPressed ? -1f : 0f;
        }
        else
        {
            VerticalInput = accelerate.isPressed ? 1f : 0f;
        }
        isBreaking = brakeButton.isPressed;

        horizontalInput = joystick.Horizontal;
       // VerticalInput = joystick.Vertical;
    }

    private void HandleMotor()
    {
        frontLeftWheel.motorTorque = VerticalInput * motorforce;
        frontRightWheel.motorTorque = VerticalInput * motorforce;

        currentbreakForce = isBreaking ? breakforce : 0f;

       
        ApplyBraking();
        

        
    }

    private void ApplyBraking()
    {
        frontLeftWheel.brakeTorque = currentbreakForce;
        frontRightWheel.brakeTorque = currentbreakForce;
        backLeftWheel.brakeTorque = currentbreakForce;
        backRightWheel.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        CurrentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheel.steerAngle = CurrentSteerAngle;
        frontRightWheel.steerAngle = CurrentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheel, frontLeftTransform);
        UpdateSingleWheel(frontRightWheel, frontRightTransform);
        UpdateSingleWheel(backLeftWheel, backLeftTransform);
        UpdateSingleWheel(backRightWheel, backRightTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelcollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelcollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
