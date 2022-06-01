using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace RaceAgentScripts
{
    [RequireComponent(typeof(Collider))]
    public class CheckPoint : MonoBehaviour
    {
        [SerializeField] private GameObject directionObj;
        public Vector3 AbsoluteDirection { get => absoluteDirection; }
        public Vector3 AverageDirection { get => averageDirection; }
        
        private Vector3 absoluteDirection;
        private Vector3 averageDirection;

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

        public void FlipAbsoluteDirectionDirection()
        {

        }

        public void ShowAverageDirection()
        {
            directionObj.SetActive(true);
            directionObj.transform.rotation = Quaternion.LookRotation(averageDirection);
        }

        public void ShowAbsoluteDirection()
        {
            directionObj.SetActive(true);
            directionObj.transform.rotation = Quaternion.LookRotation(absoluteDirection);
        }

        public void HideDirection()
        {
            directionObj.SetActive(false);
        }
        
        public void SetAverageDirection(Vector3 pAverageDirection)
        {
            
        }
    }
}
