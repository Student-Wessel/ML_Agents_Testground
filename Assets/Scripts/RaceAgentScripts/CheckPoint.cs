using System;
using UnityEngine;

namespace RaceAgentScripts
{
    [RequireComponent(typeof(Collider))]
    public class CheckPoint : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var checkpointAgent = other.GetComponent<ICheckPointActor>();
            if (checkpointAgent != null)
                checkpointAgent.CheckPointReached(this);
        }
    }
}
