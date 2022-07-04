using System;
using AimScripts;
using CustomAgent;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace AimScripts
{
    public class AimAgent : EnvironmentAgent
    {
        [SerializeField] private AimController controller;
        [SerializeField] private AimTargetCollector collector;
        [SerializeField] private GunHandler gunHandler;
        
        private int targetsHit = 0;
        
        private void Start()
        {
            gunHandler.onTargetHit += OnTargetHit;
        }

        private void OnTargetHit(GameObject obj)
        {
            AddReward(1f);
            EndEpisode();
        }

        public override void Initialize()
        {
        }
        
        public override void OnEpisodeBegin()
        {
            gunHandler.ResetGunInHandler();
            collector.ResetTargets();
            controller.ResetAim();
            environment.ResetEnvironment();
            targetsHit = 0;
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(controller.AimForward); // 3
            sensor.AddObservation(gunHandler.BulletCount()); // 1

            var closedTarget = collector.GetClosedTarget(); // 4
            if (closedTarget != null)
            {
                sensor.AddObservation(closedTarget.transform.position - transform.position);
                sensor.AddObservation(true);
            }
            else
            {
                sensor.AddObservation(Vector3.zero);
                sensor.AddObservation(false);
            }
            
        }
        
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            actionsOut.DiscreteActions.Array[0] = 0;
            if (Input.GetMouseButton(0))
            {
                actionsOut.DiscreteActions.Array[0] = 1;
            }

            mouseX = Mathf.Clamp(mouseX, -1f, 1f);
            mouseY = Mathf.Clamp(mouseY, -1f, 1f);

            actionsOut.ContinuousActions.Array[0] = mouseX;
            actionsOut.ContinuousActions.Array[1] = mouseY;
        }
        
        public override void OnActionReceived(ActionBuffers actions)
        {
            var xValue = actions.ContinuousActions[0];
            var yValue = actions.ContinuousActions[1];
            var shootValue = actions.DiscreteActions[0];
            
            controller.MoveAim(xValue,yValue);
            if (shootValue == 1)
                gunHandler.TryShoot();

            AddReward(-(1f/MaxStep));
        }
    }

    public struct TargetData
    {
        public bool inView; 
        public Vector3 position;
    }
}
