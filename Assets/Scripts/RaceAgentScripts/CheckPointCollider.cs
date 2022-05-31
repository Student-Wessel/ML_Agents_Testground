using System;
using UnityEngine;

namespace RaceAgentScripts
{
    public class CheckPointCollider : MonoBehaviour, ICheckPointActor
    {
        [SerializeField] private GameObject checkPointAgent;
        
        private ICheckPointActor actor;
        private void Awake()
        {
            actor = checkPointAgent.GetComponent<ICheckPointActor>();

            if (actor == null)
            {
                Debug.LogWarning("Check point agent component not found");
            }
        }

        public void CheckPointReached(CheckPoint pCheckPoint)
        {
            actor.CheckPointReached(pCheckPoint);
        }
    }
}
