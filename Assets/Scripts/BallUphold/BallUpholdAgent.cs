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
        [SerializeField] private float moveSpeed = 2000f;
        [SerializeField] private UpholdBall ball;
        [SerializeField] private int headbuttGoal = 10; // The amount of time the agent should headbutt the ball
        
        private Rigidbody rb;

        private EpisodeEndResult latestResults;
        
        private bool jumpRequest = false;
        private bool isGrounded = false;

        private Vector3 startingPosition;

        private int headbuttCount = 0;
        private int highestHeadbuttCount = 0;

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
            headbuttCount = 0;
            ball.ResetBall();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(transform.localPosition); // 3
            Vector3 ownVelocity = rb.velocity;
            sensor.AddObservation(ownVelocity.x); // 1
            sensor.AddObservation(ownVelocity.y); // 1
            sensor.AddObservation(transform.eulerAngles.y); // 1
            sensor.AddObservation(ball.transform.localPosition); // 3
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
            float sideValue = actions.ContinuousActions[1];

            Vector3 moveVector = Vector3.ClampMagnitude(new Vector3(sideValue, 0, forwardValue),1f) * moveSpeed * Time.fixedDeltaTime;
            rb.AddForce(moveVector,ForceMode.Acceleration);
            
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

        public void OnHeadButt()
        {
            headbuttCount++;
            latestResults.reward = GetCumulativeReward();
            latestResults.stepCount = headbuttCount;
            latestResults.maxStep = MaxStep;

            if (headbuttCount > highestHeadbuttCount)
            {
                environment.ShowEpisodeResult(latestResults);
                highestHeadbuttCount = headbuttCount;
            }
            
            AddReward(1f/headbuttGoal);
            if (headbuttCount > headbuttGoal)
                EndEpisode();
        }
    }
}
