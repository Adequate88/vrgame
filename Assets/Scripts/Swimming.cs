using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Swimming : MonoBehaviour
{
    [Header("Values")] 
    [SerializeField] private float swimForce = 3.0f;
    [SerializeField] private float dragForce = 1.0f;
    [SerializeField] private float maxControllerDistance = 0.5f;
    [SerializeField] private bool isInWater = true;
    [SerializeField] private bool swimReady = false;
    // [SerializeField] private float minForce = 0.0f;


    [SerializeField] private InputActionReference leftControllerSwim;
    [SerializeField] private InputActionReference leftControllerVelocity;
    [SerializeField] private InputActionReference leftControllerPosition;
    [SerializeField] private InputActionReference rightControllerSwim;
    [SerializeField] private InputActionReference rightControllerVelocity;
    [SerializeField] private InputActionReference rightControllerPosition;
    
    private Rigidbody rb;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        isInWater = false;
    }

    void FixedUpdate()
    {

        if (leftControllerSwim.action.IsPressed() && rightControllerSwim.action.IsPressed() && isInWater)
        {
            
            var leftPosition = leftControllerPosition.action.ReadValue<Vector3>(); 
            var rightPosition = rightControllerPosition.action.ReadValue<Vector3>();
            float controllerDistance = Vector3.Distance(leftPosition, rightPosition);

            if (controllerDistance < maxControllerDistance)
            {
                swimReady = true;

            }

            if (swimReady && controllerDistance > maxControllerDistance)
            {
                
                swimReady = false;
                var leftVelocity = leftControllerVelocity.action.ReadValue<Vector3>();
                var rightVelocity = rightControllerVelocity.action.ReadValue<Vector3>();
                var leftDirection = leftVelocity.normalized;
                var rightDirection = rightVelocity.normalized;
                var direction = Vector3.Lerp(leftDirection, rightDirection, 0.5f);
                var force = swimForce * direction;
                rb.AddForce(force, ForceMode.Acceleration);
                
            }
        }

        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            
            rb.AddForce(-rb.linearVelocity * dragForce, ForceMode.Acceleration);
            
        }
    }
}   


