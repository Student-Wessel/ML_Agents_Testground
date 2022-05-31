using System;
using System.Collections.Generic;
using CustomAgent;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace RaceAgentScripts
{
    [RequireComponent(typeof(AbstractCarController),typeof(Rigidbody))]
    public class RaceAgent : EnvironmentAgent, ICheckPointAgent
    {
        [SerializeField] private CheckPointManager checkPointManager;

        // This rigidbody is only used for triggers, and is set to kinematic
        private Rigidbody rb;
        
        private AbstractCarController carController;
        private List<CheckPoint> reachedCheckPoints;
        private int latestCheckPointIndex = -1;
        
        private Vector3 startPosition;
        private Quaternion startRotation;

        private readonly string punishZoneString = "PunishZone";
        private int inZoneTriggers = 0;
        public bool isInPunishZone = false;
        
        // Debug

        [Range(0.01f, 1f)] 
        public float ContinuousActionsScale;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            carController = GetComponent<AbstractCarController>();
            reachedCheckPoints = new List<CheckPoint>();

            rb.isKinematic = true;
            rb.useGravity = false;
        }

        private void Start()
        {
            startPosition = carController.GetRigidbodyPosition();
            startRotation = carController.transform.rotation;
        }

        public override void OnEpisodeBegin()
        {
            carController.WarpCar(startPosition);
            carController.transform.rotation = startRotation;
            reachedCheckPoints = new List<CheckPoint>();
            latestCheckPointIndex = -1;
        }
        
        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(carController.transform.rotation.eulerAngles.y); // 1
            sensor.AddObservation(carController.GetVelocity()); // 3
        }
        
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            if (Input.GetKey(KeyCode.W))
            {
                actionsOut.ContinuousActions.Array[0] = ContinuousActionsScale;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                actionsOut.ContinuousActions.Array[0] = -ContinuousActionsScale;
            }
            else
            {
                actionsOut.ContinuousActions.Array[0] = 0;
            }

            if (Input.GetKey(KeyCode.A))
            {
                actionsOut.ContinuousActions.Array[1] = -ContinuousActionsScale;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                actionsOut.ContinuousActions.Array[1] = ContinuousActionsScale;
            }
            else
            {
                actionsOut.ContinuousActions.Array[1] = 0;
            }

            actionsOut.DiscreteActions.Array[0] = 0;
            if (Input.GetKey(KeyCode.Space))
            {
                actionsOut.DiscreteActions.Array[0] = 1;
            }
        }
        
        public override void OnActionReceived(ActionBuffers actions)
        {
            var forwardValue = actions.ContinuousActions[0];
            var sidewaysValue = actions.ContinuousActions[1];
            var breakValue = actions.DiscreteActions[0];

            CarInput carInput = new CarInput();
            carInput.forward = forwardValue;
            carInput.sideways = sidewaysValue;
            carInput.breaking = breakValue == 1;
            
            carController.SetCarInput(carInput);

            if (isInPunishZone)
            {
                AddReward(-1f);
                EndEpisode();
            }
        }
        public void CheckPointReached(CheckPoint pCheckPoint)
        {
            // We did not get to this checkpoint yet
            if (!reachedCheckPoints.Contains(pCheckPoint))
            {
                int reachedCheckPointIndex = checkPointManager.GetCheckPointIndex(pCheckPoint);
                
                // We got to the next checkpoint
                if (reachedCheckPointIndex == (latestCheckPointIndex + 1))
                {
                    // We reach the finish line
                    if (reachedCheckPointIndex == checkPointManager.HighestCheckpointIndex)
                    {
                        AddReward(1f / checkPointManager.HighestCheckpointIndex);
                        reachedCheckPoints = new List<CheckPoint>();
                        latestCheckPointIndex = -1;
                    }
                    else
                    {
                        AddReward(1f / checkPointManager.HighestCheckpointIndex);
                        reachedCheckPoints.Add(pCheckPoint);
                        latestCheckPointIndex = reachedCheckPointIndex;
                    }
                }
                else // We got to an wrong check point
                {
                    AddReward(-1f);
                    EndEpisode();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(punishZoneString))
            {
                isInPunishZone = true;
                inZoneTriggers++;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(punishZoneString))
            {
                inZoneTriggers--;

                if (inZoneTriggers <= 0)
                {
                    inZoneTriggers = 0;
                    isInPunishZone = false;
                }
            }
        }
    }
}
