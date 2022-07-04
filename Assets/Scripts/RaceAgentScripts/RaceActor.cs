using System;
using System.Collections.Generic;
using CustomAgent;
using RaceAgentScripts.TrackScripts;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace RaceAgentScripts
{
    [RequireComponent(typeof(AbstractCarController),typeof(Rigidbody),typeof(CheckPointActor))]
    public class RaceActor : EnvironmentAgent
    {
        private CheckPointActor checkPointActor;

        // This rigidbody is only used for triggers, and is set to kinematic
        private Rigidbody rb;
        
        private AbstractCarController carController;
        
        private Vector3 startPosition;
        private Quaternion startRotation;

        private readonly string punishZoneString = "PunishZone";
        private int inZoneTriggers = 0;
        public bool isInPunishZone = false;

        private int finishCount = 0;
        
        // Debug

        [Range(0.01f, 1f)] 
        public float ContinuousActionsScale;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            carController = GetComponent<AbstractCarController>();
            checkPointActor = GetComponent<CheckPointActor>();

            rb.isKinematic = true;
            rb.useGravity = false;
            
            checkPointActor.checkPointReachedCorrect += OnCorrectCheckPoint;
            checkPointActor.checkPointReachedWrong += OnWrongCheckPoint;
            checkPointActor.checkPointFailed += OnCheckPointFailed;
            checkPointActor.checkPointReachedFinish += OnFinishReached;
        }
        
        private void OnCorrectCheckPoint(CheckPoint pCheckPoint)
        {
            AddReward((1f / checkPointActor.checkPointCount)*5f);
        }
        
        private void OnWrongCheckPoint(CheckPoint pCheckPoint)
        {

        }
        
        private void OnCheckPointFailed(CheckPoint pCheckPoint)
        {
            int stepsLeft = MaxStep - StepCount;
            AddReward((-(1f/MaxStep)*stepsLeft));
            EndEpisode();
        }

        private void OnFinishReached(CheckPoint pCheckPoint)
        {
            finishCount++;
            AddReward((1f / checkPointActor.checkPointCount)*5f);
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
            checkPointActor.ResetActor();
            finishCount = 0;
        }
        
        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(transform.forward); // 3
            sensor.AddObservation(carController.GetVelocity()); // 3
            sensor.AddObservation(carController.GetTurnAngle()); // 1
            sensor.AddObservation(checkPointActor.CurrentTarget.AverageDirection); // 3
        }
        
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            if (Input.GetKey(KeyCode.W))
            {
                actionsOut.DiscreteActions.Array[0] = 2;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                actionsOut.DiscreteActions.Array[0] = 0;
            }
            else
            {
                actionsOut.DiscreteActions.Array[0] = 1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                actionsOut.DiscreteActions.Array[1] = 0;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                actionsOut.DiscreteActions.Array[1] = 2;
            }
            else
            {
                actionsOut.DiscreteActions.Array[1] = 1;
            }
        }
        
        public override void OnActionReceived(ActionBuffers actions)
        {
            var forwardValue = actions.DiscreteActions[0]-1;
            var sidewaysValue = actions.DiscreteActions[1]-1;

            CarInput carInput = new CarInput();
            carInput.forward = forwardValue;
            carInput.sideways = sidewaysValue;
            
            carController.SetCarInput(carInput);

            // if (isInPunishZone)
            // {
            //     AddReward(-0.001f);
            // }
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
