using System;
using CustomAgent;
using TMPro;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Utils;

namespace BallUphold
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallUpholdAgent : EnvironmentAgent
    {
        [SerializeField] private float moveSpeed = 2f, rotationSpeed = 360;
        [SerializeField] private UpholdBall ball;
        
        private Rigidbody rb;

        private EpisodeEndResult latestResults;
        
        private bool jumpRequest = false;
        private bool isGrounded = false;

        private Vector3 startingPosition;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            startingPosition = transform.position;
        }

        public override void OnEpisodeBegin()
        {
            transform.position = startingPosition;
            rb.velocity = Vector3.zero;
            environment.ResetEnvironment();
            environment.ShowEpisodeResult(latestResults);
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(transform.position); // 3
            Vector3 ownVelocity = rb.velocity;
            sensor.AddObservation(ownVelocity.x); // 1
            sensor.AddObservation(ownVelocity.y); // 1
            sensor.AddObservation(transform.eulerAngles.y); // 1
            sensor.AddObservation(ball.transform); // 3
            sensor.AddObservation(ball.velocity); // 3
        }
        
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            // DiscreteActions
            // Jump         [0] = 0 or 1

            // actionsOut.DiscreteActions.Array[0] = 0;
            //
            // if (Input.GetKey(KeyCode.Space))
            // {
            //     actionsOut.DiscreteActions.Array[0] = 1;
            // }
            
            // ContinuousActions
            // Move forward [0]
            // Rotate       [1]

            actionsOut.ContinuousActions.Array[0] = 0;
            actionsOut.ContinuousActions.Array[1] = 0;
            
            if (Input.GetKey(KeyCode.W))
            {
                actionsOut.ContinuousActions.Array[0] = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                actionsOut.ContinuousActions.Array[0] = -1;
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                actionsOut.ContinuousActions.Array[1] = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                actionsOut.ContinuousActions.Array[1] = 1;
            }
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            float forwardValue = actions.ContinuousActions[0];
            float rotationValue = actions.ContinuousActions[1];

            Vector3 moveVector = transform.forward * forwardValue * moveSpeed;
            rb.AddForce(moveVector,ForceMode.Acceleration);
            transform.Rotate(Vector3.up,rotationValue * rotationSpeed * Time.fixedDeltaTime);
            
            if (MaxStep > 0)
            {
                AddReward(1f / MaxStep);
                latestResults.reward = GetCumulativeReward();
                latestResults.stepCount = StepCount;
                latestResults.maxStep = MaxStep;
                
                if (StepCount % 100 == 0)
                {
                    environment.ShowEpisodeResult(latestResults);
                }
            }
        }
        
        // This for some reason the rb.freeze rotations doesn't work
        protected void LateUpdate()
        {
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }

        public void OnBallGroundTouch()
        {
            EndEpisode();
        }
    }
}
