using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SimpleCarController : MonoBehaviour {
   private float horizontalInput;
    private float verticalInput;
    private float steerAngle;
    private bool isBreaking;

    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    private float maxSteeringAngle = 30f;
    private float motorForce = 300f;
    private float brakeForce = 0f;
    private float reverseForce = -300f;

    private void FixedUpdate()
    {
       //GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    public void setInput(float _horizontalInput, float _verticalInput)
    {
        horizontalInput = _horizontalInput;
        verticalInput = _verticalInput;

    }

    public void reset() {

        frontLeftWheelCollider.brakeTorque = Mathf.Infinity;
        frontRightWheelCollider.brakeTorque = Mathf.Infinity;
        rearLeftWheelCollider.brakeTorque = Mathf.Infinity;
        rearRightWheelCollider.brakeTorque = Mathf.Infinity;

    }

    private void HandleSteering()
    {
        steerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steerAngle;
        frontRightWheelCollider.steerAngle = steerAngle;
    }

    private void HandleMotor()
    {
        float force = (verticalInput==-1f)? reverseForce : motorForce;
        frontLeftWheelCollider.motorTorque = force;
        frontRightWheelCollider.motorTorque = force;

        brakeForce = isBreaking ? 3000f : 0f;
        frontLeftWheelCollider.brakeTorque = brakeForce;
        frontRightWheelCollider.brakeTorque = brakeForce;
        rearLeftWheelCollider.brakeTorque = brakeForce;
        rearRightWheelCollider.brakeTorque = brakeForce;
    }

    private void UpdateWheels()
    {
        UpdateWheelPos(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPos(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelPos(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheelPos(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateWheelPos(WheelCollider wheelCollider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        trans.rotation = rot;
        trans.position = pos;
    }

    public float getVelocity() {
        
        Rigidbody rb = GetComponent<Rigidbody>();
        return rb.velocity.magnitude;
    }
}