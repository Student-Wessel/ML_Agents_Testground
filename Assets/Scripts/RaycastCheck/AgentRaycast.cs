using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

namespace RaycastCheck
{
    public class AgentRaycast : Agent
    {
        public GameObject goal;
        public float moveSpeed = 5f;
        private Vector3 startPosition;
        
        private void Awake()
        {
            startPosition = transform.localPosition;
        }

        
        public override void OnEpisodeBegin()
        {
            transform.localPosition = startPosition;
            if (Random.value >= 0.5f)
            {
                goal.transform.localPosition = new Vector3(-5, 0, 0);
            }
            else
            {
                goal.transform.localPosition = new Vector3(5, 0, 0);
            }
        }
        
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            actionsOut.DiscreteActions.Array[0] = 1;
            if (Input.GetKey(KeyCode.A))
            {
                actionsOut.DiscreteActions.Array[0] = 0;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                actionsOut.DiscreteActions.Array[0] = 2;
            }
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            var moveValue = actions.DiscreteActions[0] -1;
            Vector3 moveVector = Vector3.zero;
            moveVector.x = moveValue * Time.fixedDeltaTime * moveSpeed;

            transform.localPosition += moveVector;
            
            AddReward(-(1f/MaxStep));
        }

        private void OnTriggerEnter(Collider other)
        {
            AddReward(1f);
            EndEpisode();
        }
    }
}
