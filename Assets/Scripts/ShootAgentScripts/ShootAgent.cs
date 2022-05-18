using System;
using AgentUtils;
using CustomAgent;
using CustomPropertyDrawers;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace ShootAgentScripts
{
    [RequireComponent(typeof(CharacterController))]
    public class ShootAgent : EnvironmentAgent , ISpawnable
    {
        [SerializeField] private GameObject goal;
        
        [SerializeField] private float moveSpeed;
        [Range(1f, 360f)] 
        [SerializeField] private float rotationSpeed;
        [SerializeField] private LayerMask shotMask;

        private CharacterController ch;

        public void Awake()
        {
            ch = GetComponent<CharacterController>();
        }

        #region ML-Agents
        
        public override void OnEpisodeBegin()
        {
            
        }
        
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            // Rotation
            float rotationValue = 0f;
            if (Input.GetKey(KeyCode.Q))
                rotationValue = -0.5f;
            else if (Input.GetKey(KeyCode.E))
                rotationValue = 0.5f;

            actionsOut.ContinuousActions.Array[0] = rotationValue;

            if (Input.GetKey(KeyCode.W))
            {
                actionsOut.DiscreteActions.Array[1] = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                actionsOut.DiscreteActions.Array[1] = -1;
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                actionsOut.DiscreteActions.Array[2] = 1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                actionsOut.DiscreteActions.Array[2] = -1;
            }

            if (Input.GetKey(KeyCode.Space))
                actionsOut.DiscreteActions.Array[0] = 1;
        }
        
        public override void OnActionReceived(ActionBuffers actions)
        {
            // between -1 and 1
            float rotationRawValue = actions.ContinuousActions[0];

            float rotationValue = rotationRawValue * rotationSpeed * Time.fixedDeltaTime;
            transform.Rotate(Vector3.up,rotationValue);

            int shootValue = actions.DiscreteActions[0];
            float forwardValue = actions.DiscreteActions[1];
            float sidewaysValue = -actions.DiscreteActions[2];

            if (shootValue == 1)
                Shoot();

            forwardValue = Mathf.Clamp(forwardValue, -0.5f, 1f);

            Vector3 moveVector = Vector3.ClampMagnitude((transform.forward*forwardValue) + (transform.right*sidewaysValue),1f) * moveSpeed * Time.fixedDeltaTime;
            ch.Move(moveVector);
            
            if (MaxStep > 0)
                AddReward(-(1f / MaxStep));
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            // myPos = 3
            // myRot = 1
            // goalPos = 3
            // angle = 1

            sensor.AddObservation(transform.position);
            sensor.AddObservation(transform.rotation.y);
            sensor.AddObservation(goal.transform.position);
            
            Vector3 delta = goal.transform.position - transform.position;
            delta.Normalize();
            
            float angleDot = Vector3.Dot(delta,transform.forward);
            sensor.AddObservation(angleDot);
        }

        #endregion

        private void Shoot()
        {
            // RaycastHit hit = new RaycastHit();
            // bool hasHit = false;
            // if (Physics.Raycast(transform.position, transform.forward, out hit,float.MaxValue, shotMask))
            // {
            //     ShootAgent otherAgent = hit.transform.GetComponent<ShootAgent>();
            //
            //     // We shot another agent
            //     if (otherAgent != null && otherAgent != this)
            //     {
            //         //otherAgent.SetReward(-1f);
            //         //SetReward(1f);
            //         //EpisodeEndRequest();
            //     }
            //     hasHit = true;
            // }
            //
            // if (hasHit)
            // {
            //     Debug.DrawLine(transform.position,hit.point,Color.red,1f);
            // }
            // else
            // {
            //     Debug.DrawLine(transform.position,transform.forward*5f,Color.red,1f);
            // }
        }

        public void Spawn(Vector3 pPosition)
        {
            ch.enabled = false;
            transform.position = pPosition;
            ch.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == goal)
            {
                AddReward(2f);
                EpisodeEndResult result = new EpisodeEndResult();
                EndEpisode();
                
                environment.ShowEpisodeResult(result);
                environment.ResetEnvironment();
            }
        }
    }
}
