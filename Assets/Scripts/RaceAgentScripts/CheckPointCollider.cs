using System;
using UnityEngine;

namespace RaceAgentScripts
{
    public class CheckPointCollider : MonoBehaviour, ICheckPointAgent
    {
        [SerializeField] private GameObject checkPointAgent;
        
        private ICheckPointAgent agent;
        private void Awake()
        {
            agent = checkPointAgent.GetComponent<ICheckPointAgent>();

            if (agent == null)
            {
                Debug.LogWarning("Check point agent component not found");
            }
        }

        public void CheckPointReached(CheckPoint pCheckPoint)
        {
            agent.CheckPointReached(pCheckPoint);
        }
    }
}
