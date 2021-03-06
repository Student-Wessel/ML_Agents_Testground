using System;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using Utils;

namespace RaceAgentScripts.TrackScripts
{
    [ExecuteInEditMode]
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

        // private void Update()
        // {
        //     GetComponentInChildren<Collider>().tag = "CheckPoint";
        // }

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
            var renderer = GetComponent<MeshRenderer>();
            
            if (renderer != null)
                renderer.enabled = true;

            foreach (Transform child in transform)
            {
                var childRenderer =child.GetComponent<MeshRenderer>();

                if (childRenderer != null)
                    childRenderer.enabled = true;
            }
        }

        public void HideCollider()
        {
            var renderer = GetComponent<MeshRenderer>();
            
            if (renderer != null)
                renderer.enabled = false;

            foreach (Transform child in transform)
            {
                var childRenderer =child.GetComponent<MeshRenderer>();

                if (childRenderer != null)
                    childRenderer.enabled = false;
            }
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
