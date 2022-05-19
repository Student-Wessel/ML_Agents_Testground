using AgentUtils;
using CustomAgent;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Utils;

namespace MoveAgentScripts
{
    [RequireComponent(typeof(CharacterController))]
    public class MoveAgent : EnvironmentAgent , ISpawnable
    {
        [SerializeField] private float moveSpeed = 5f,strafeMultiplier = 0.75f,backwardMultiplier = 0.5f;
        
        [Range(1f, 360f)] 
        [SerializeField] private float rotationSpeed;

        private CharacterController ch;
        private EpisodeEndResult lastResults;
        
        // goal
        [SerializeField] private GameObject goal;

        public void Awake()
        {
            ch = GetComponent<CharacterController>();
        }

        #region ML-Agents
        
        public override void OnEpisodeBegin()
        {
            environment.ResetEnvironment();
            environment.ShowEpisodeResult(lastResults);
            lastResults = new EpisodeEndResult();
            lastResults.reward = -1f;
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

            actionsOut.DiscreteActions.Array[0] = 1;
            actionsOut.DiscreteActions.Array[1] = 1;


            if (Input.GetKey(KeyCode.W))
            {
                actionsOut.DiscreteActions.Array[0] = 2;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                actionsOut.DiscreteActions.Array[0] = 0;
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                actionsOut.DiscreteActions.Array[1] = 2;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                actionsOut.DiscreteActions.Array[1] = 0;
            }
        }
        
        public override void OnActionReceived(ActionBuffers actions)
        {
            // TO-DO: Make the step we take be an ovale shape so we move fasted forward
            // Currently the AI can go both back and sides to get max step size
            
            // ContinuousActions
            // [0] rotation value | between -1 and 1
            
            // DiscreteActions
            // forward Value [0] | 0 or 1 or 2
            // side Value [1] | 0 or 1 or 2
            
            var oldPosition = transform.position;

            float rotationRawValue = actions.ContinuousActions[0];

            float rotationValue = rotationRawValue * rotationSpeed * Time.fixedDeltaTime;
            transform.Rotate(Vector3.up,rotationValue);

            // Maps values from 0,1,2 to -1,0,1
            float forwardValue = (actions.DiscreteActions[0]-1);
            float sidewaysValue = (-(actions.DiscreteActions[1]-1));

            forwardValue = Mathf.Clamp(forwardValue, -backwardMultiplier, 1f);
            sidewaysValue = Mathf.Clamp(sidewaysValue, -strafeMultiplier, strafeMultiplier);

            Vector3 moveVector = Vector3.ClampMagnitude((transform.forward*forwardValue) + (transform.right*sidewaysValue),1f) * moveSpeed * Time.fixedDeltaTime;
            ch.Move(moveVector);
            
            var stepPosition = transform.position;
            var targetPosition = goal.transform.position;

            Vector3 bestPossibleStep = ((targetPosition - oldPosition).normalized) * moveSpeed * Time.fixedDeltaTime;
            Vector3 bestPossibleStepPosition = oldPosition + bestPossibleStep;

            float distanceBetweenStepAndBest = (bestPossibleStepPosition - stepPosition).magnitude;

            float bestStepRatioScaled = MathExtension.Map(-distanceBetweenStepAndBest,-((moveSpeed * Time.fixedDeltaTime)*2), 0, -1, 1);

            // If we use max step, we want to give negative points each step
            if (MaxStep > 0)
            {
                AddReward(bestStepRatioScaled / MaxStep);
                AddReward(-(1f / MaxStep));
            }
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            // myPos = 3
            // myRot = 1
            // goalPos = 3
            // angle = 1

            var ownPos = transform.position;
            var goalPos = goal.transform.position;

            sensor.AddObservation(ownPos);
            sensor.AddObservation(transform.rotation.y);
            sensor.AddObservation(goalPos);
            
            Vector3 delta = goalPos - ownPos;
            delta.Normalize();
            
            float angleDot = Vector3.Dot(delta,transform.forward);
            sensor.AddObservation(angleDot);
        }

        #endregion

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
                int stepsLeft = MaxStep - StepCount;
                // reward the agent as if he did a perfect step for each step left
                AddReward((1/MaxStep)*stepsLeft);
                lastResults.reward = 1f;
                EndEpisode();
            }
        }
    }
}
