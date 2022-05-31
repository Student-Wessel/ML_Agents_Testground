using System;
using UnityEngine;

namespace RaceAgentScripts
{
    [RequireComponent(typeof(Collider),typeof(Rigidbody))]
    public class CheckPoint : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
            var rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            var checkpointAgent = other.GetComponent<ICheckPointAgent>();
            if (checkpointAgent != null)
                checkpointAgent.CheckPointReached(this);
        }
    }
}
