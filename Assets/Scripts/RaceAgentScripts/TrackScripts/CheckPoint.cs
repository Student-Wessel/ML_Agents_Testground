using System;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using Utils;

namespace RaceAgentScripts.TrackScripts
{
    public class CheckPoint : MonoBehaviour
    {
        // The object that has the forward transform of the absolute direction
        [SerializeField] private GameObject visualObj;
        public Vector3 AbsoluteDirection => transform.forward;
        public Vector3 AverageDirection { get => averageDirection;}

        private Vector3 absoluteDirection;
        private Vector3 averageDirection;

        private void Awake()
        {
            //trigger.TriggerEntered += OnTriggerEntered; 
        }

        // private void OnTriggerEntered(Collider other)
        // {
        //     var checkpointAgent = other.GetComponent<ICheckPointActor>();
        //     if (checkpointAgent != null)
        //         checkpointAgent.CheckPointReached(this);
        // }

        private void OnValidate()
        {
            if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                ShowAbsoluteDirection();
            }
        }
        
        public void FlipAbsoluteDirection()
        {
            transform.Rotate(Vector3.up,180f);
        }
        public void ShowCollider()
        {
            GetComponent<MeshRenderer>().enabled = true;
        }

        public void HideCollider()
        {
            GetComponent<MeshRenderer>().enabled = false;
        }

        public void ShowAverageDirection()
        {
            visualObj.SetActive(true);
            SetAverageDirection(AverageDirection);
        }

        public void ShowAbsoluteDirection()
        {
            visualObj.SetActive(true);
            visualObj.transform.localRotation = Quaternion.identity;
        }

        public void HideDirection()
        {
            visualObj.SetActive(false);
        }
        
        public void SetAverageDirection(Vector3 pAverageDirection)
        {
            averageDirection = pAverageDirection;
            visualObj.transform.rotation = Quaternion.LookRotation(AverageDirection);
        }
    }
}
