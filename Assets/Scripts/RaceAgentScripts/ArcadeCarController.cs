using System;
using UnityEngine;
using Utils;

namespace RaceAgentScripts
{
    public class ArcadeCarController : AbstractCarController
    {
        [Header("Car Variables for states")] 
        [SerializeField] private CarVariables AcceleratingVariables;
        [SerializeField] private CarVariables DecelerationVariables,NeutralVeriables,BreakingVeriables;

        [Header("Static car variables")] 
        [SerializeField] private float forwardAccel;
        [SerializeField] private float reverseAccel;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float maxTurnAngle = 20f;
        // Lower value means more slip
        [Range(0.0001f,1f)]
        [SerializeField] private float rotationSlip = 0.1f;
        
        [Header("Car components")]
        [SerializeField] private Rigidbody rb;

        [SerializeField] private Transform frontLeftWheel,frontRightWheel;

        // Variables
        private float turnStrength,turnAngle;
        private int forwardInput, sidewaysInput;

        public override float GetTurnAngle()
        {
            return turnAngle;
        }
        
        private const float forceTreshold = 0.001f;
        
        private void Start()
        {
            rb.transform.parent = null;
        }

        private void Update()
        {
            WheelAnimation();
        }

        private void WheelAnimation()
        {
            var rotationScale = Mathf.Clamp(turnAngle,-45f,45f);

            var localLeft = frontLeftWheel.localRotation;
            var localRight = frontRightWheel.localRotation;

            var leftTarget = Quaternion.Euler(0, rotationScale + 180f, 0);
            var rightTarget = Quaternion.Euler(0, rotationScale, 0);

            var leftMove =Quaternion.RotateTowards(localLeft, leftTarget, 220 * Time.deltaTime);
            var rightMove =Quaternion.RotateTowards(localRight, rightTarget, 220 * Time.deltaTime);
            
            frontLeftWheel.localRotation = leftMove;
            frontRightWheel.localRotation = rightMove;
        }

        private void FixedUpdate()
        {
            var ownTransform = transform;
            var velocityLenght = rb.velocity.magnitude;

            // if (sidewaysInput == -1)
            // {
            //     turnAngle = -90f;
            // }
            // else if (sidewaysInput == 1)
            // {
            //     turnAngle = 90f;
            // }
            // else
            // {
            //     turnAngle = 0;
            // }

            turnAngle += sidewaysInput * turnStrength;
            turnAngle = Mathf.Clamp(turnAngle, -maxTurnAngle, maxTurnAngle);
            
            // This ratio is used to scale down the rotation where we to stand still or drive really slow
            float ratio = Mathf.Clamp(velocityLenght*0.1f, 0f, 1f);
            float turnMultiplier = ratio;
            
            // We calculate the dot to see if we are moving forward or backwards relative to the rotation of the car model
            var dot = Vector3.Dot(ownTransform.forward, rb.velocity.normalized);
            
            // 1 if forward, -1 if backwards
            int direction = 0;
            if (dot > 0.05f)
                direction = 1;
            else if (dot < -0.05f)
                direction = -1;

            // We then create a new rotation for the car model to rotate to
            ownTransform.Rotate(Vector3.up,turnAngle*turnMultiplier*Time.fixedDeltaTime*direction);
            
            // var newRotation = Quaternion.Euler(ownTransform.rotation.eulerAngles + new Vector3(0f,(sidewaysInput * turnMultiplier * Time.fixedDeltaTime)*direction,0f));
            // ownTransform.rotation = newRotation;

            // We are moving more forward so we want to rotate the velocity a little bit to our forward vector
            // This makes it so when we are not applying force with the gas paddle, we can still steer to an direction
            if (dot > 0.05f && velocityLenght >= 1f)
            {
                rb.velocity = Vector3.RotateTowards(rb.velocity, ownTransform.forward, rotationSlip*turnMultiplier, Time.fixedDeltaTime);
            } // Same thing here but in reverse
            else if (dot < -0.05f && velocityLenght >= 1f)
            {
                rb.velocity = Vector3.RotateTowards(rb.velocity, -ownTransform.forward, rotationSlip*turnMultiplier, Time.fixedDeltaTime);
            }

            // Adding force to the position the car model is facing
            if (forwardInput > forceTreshold)
            {
                rb.AddForce(ownTransform.forward * forwardInput * forwardAccel * 500f * Time.fixedDeltaTime);
            } // Reverse
            else if (forwardInput < -forceTreshold)
            {
                rb.AddForce(ownTransform.forward * forwardInput * reverseAccel * 500f * Time.fixedDeltaTime);
            }

            // Clamp our velocity so we can not go beyond our max speed
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            
            // Finally set the car's model to the new rigidbody position
            ownTransform.position = rb.transform.position;
        }

        public override void StopCar()
        {
            rb.velocity = Vector3.zero;
            forwardInput = 0;
            sidewaysInput = 0;
            
            rb.mass = NeutralVeriables.mass;
            rb.drag = NeutralVeriables.drag;
            turnStrength = NeutralVeriables.turnStrength;
        }

        public override void WarpCar(Vector3 pPosition, bool pResetVelocity = true)
        {
            if (pResetVelocity)
                rb.velocity = Vector3.zero;

            rb.position = pPosition;
            turnAngle = 0;
        }
        public override void SetCarInput(CarInput pCarInput)
        {
            forwardInput = pCarInput.forward;
            sidewaysInput = pCarInput.sideways;
            
            if (forwardInput > forceTreshold) // Accelerating
            {
                rb.mass = AcceleratingVariables.mass;
                rb.drag = AcceleratingVariables.drag;
                turnStrength = AcceleratingVariables.turnStrength;
            }
            else if (forwardInput < -forceTreshold) // Decelerating
            {
                rb.mass = DecelerationVariables.mass;
                rb.drag = DecelerationVariables.drag;
                turnStrength = DecelerationVariables.turnStrength;
            }
            else
            {
                rb.mass = NeutralVeriables.mass;
                rb.drag = NeutralVeriables.drag;
                turnStrength = NeutralVeriables.turnStrength;
            }
        }

        public override Vector3 GetVelocity()
        {
            return rb.velocity;
        }

        public override Vector3 GetRigidbodyPosition()
        {
            return rb.position;
        }
    }

    [Serializable]
    public struct CarVariables
    {
        public float mass;
        public float drag;
        public float turnStrength;
    }

    public struct CarInput
    {
        public int forward;
        public int sideways;
    }
}
