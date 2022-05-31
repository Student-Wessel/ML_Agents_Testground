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
        // Lower value means more slip
        [Range(0.0001f,1f)]
        [SerializeField] private float rotationSlip = 0.1f;
        
        [Header("Car components")]
        [SerializeField] private Rigidbody rb;

        [SerializeField] private Transform frontLeftWheel,frontRightWheel;

        // Variables
        private float turnStrength;
        private float forwardInput, sidewaysInput;
        private bool isBreaking = false;

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
            var rotationScale = MathExtension.Map(sidewaysInput, -1f, 1f, -35f, 35f);

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
            //DebugSetInput();

            var ownTransform = transform;

            var velocityLenght = rb.velocity.magnitude;
            
            // This ratio is used to scale down the rotation where we to stand still or drive really slow
            float ratio = Mathf.Clamp(velocityLenght*0.1f, 0f, 1f);
            float turnMultiplier = turnStrength * ratio;
            
            // We calculate the dot to see if we are moving forward or backwards relative to the rotation of the car model
            var dot = Vector3.Dot(ownTransform.forward, rb.velocity.normalized);
            
            // 1 if forward, -1 if backwards
            int direction = 0;
            if (dot > 0.05f)
                direction = 1;
            else if (dot < -0.05f)
                direction = -1;

            // We then create a new rotation for the car model to rotate to
            var newRotation = Quaternion.Euler(ownTransform.rotation.eulerAngles + new Vector3(0f,(sidewaysInput * turnMultiplier * Time.fixedDeltaTime)*direction,0f));
            ownTransform.rotation = newRotation;

            // We are moving more forward so we want to rotate the velocity a little bit to our forward vector
            // This makes it so when we are not applying force with the gas paddle, we can still steer to an direction
            if (dot > 0.05f && !isBreaking && velocityLenght >= 1f)
            {
                rb.velocity = Vector3.RotateTowards(rb.velocity, ownTransform.forward, rotationSlip*turnMultiplier, Time.fixedDeltaTime);
            } // Same thing here but in reverse
            else if (dot < -0.05f && !isBreaking && velocityLenght >= 1f)
            {
                rb.velocity = Vector3.RotateTowards(rb.velocity, -ownTransform.forward, rotationSlip*turnMultiplier, Time.fixedDeltaTime);
            }

            // Adding force to the position the car model is facing
            if (forwardInput > forceTreshold)
            {
                if (!isBreaking)
                {
                    rb.AddForce(ownTransform.forward * forwardInput * forwardAccel * 1000f * Time.fixedDeltaTime);
                }
                else
                {
                    rb.AddForce(ownTransform.forward * forwardInput * forwardAccel * 500f * Time.fixedDeltaTime);
                }
            } // Reverse
            else if (forwardInput < -forceTreshold)
            {
                if (!isBreaking)
                {
                    rb.AddForce(ownTransform.forward * forwardInput * reverseAccel * 1000f * Time.fixedDeltaTime);
                }
                else
                {
                    rb.AddForce(ownTransform.forward * forwardInput * reverseAccel * 500f * Time.fixedDeltaTime);
                }
            }

            // Clamp our velocity so we can not go beyond our max speed
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            
            // Finally set the car's model to the new rigidbody position
            ownTransform.position = rb.transform.position;
        }


        private void DebugSetInput()
        {
            forwardInput = 0f;
            sidewaysInput = 0f;
            isBreaking = false;

            // if (Input.GetKey(KeyCode.W))
            // {
            //     forwardInput = testForward;
            // }
            // else if (Input.GetKey(KeyCode.S))
            // {
            //     forwardInput = -testReverse;
            // }
            //
            // if (Input.GetKey(KeyCode.A))
            // {
            //     sidewaysInput = -testLeft;
            // }
            // else if (Input.GetKey(KeyCode.D))
            // {
            //     sidewaysInput = testRight;
            // }
            //
            // if (Input.GetKey(KeyCode.Space))
            // {
            //     isBreaking = true;
            // }

            if (isBreaking)
            {
                rb.mass = BreakingVeriables.mass;
                rb.drag = BreakingVeriables.drag;
                turnStrength = BreakingVeriables.turnStrength;
            } 
            else if (forwardInput > forceTreshold) // Accelerating
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
        }
        public override void SetCarInput(CarInput pCarInput)
        {
            forwardInput = pCarInput.forward;
            sidewaysInput = pCarInput.sideways;
            isBreaking = pCarInput.breaking;
            
            if (isBreaking)
            {
                rb.mass = BreakingVeriables.mass;
                rb.drag = BreakingVeriables.drag;
                turnStrength = BreakingVeriables.turnStrength;
            } 
            else if (forwardInput > forceTreshold) // Accelerating
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
        public float forward;
        public float sideways;
        public bool breaking;
    }
}
