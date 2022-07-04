using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using RaceAgentScripts.TrackScripts;
using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(CinemachineTargetGroup))]
    public class CinemachineTargetGroupCollector : MonoBehaviour
    {
        [SerializeField] private bool updateOnFixedUpdate = false;
        [SerializeField] private int updatesPerSecond;
        [SerializeField] private int FirstAmountOfTargets = 10;
        [SerializeField] private float firstWeight, secondWeight, thirdWeight, defaultWeight;
        [SerializeField] private Transform parentTargets;
        
        public List<CheckPointActor> checkPointActors;
        private CinemachineTargetGroup cinemachineGroup;

        private void Awake()
        {
            cinemachineGroup = GetComponent<CinemachineTargetGroup>();
            checkPointActors = new List<CheckPointActor>();

            foreach (Transform child in parentTargets)
            {
                var checkPointActor = child.GetComponent<CheckPointActor>();
                if (checkPointActor != null)
                    checkPointActors.Add(checkPointActor);
            }
            StartCoroutine(TargetGroupCoroutine());
        }

        private void FixedUpdate()
        {
            if (updateOnFixedUpdate)
            {
                SortCheckPointActors();
                SetTargetGroup();
            }
        }

        private IEnumerator TargetGroupCoroutine()
        {
            while (!updateOnFixedUpdate)
            {
                SortCheckPointActors();
                SetTargetGroup();
                yield return new WaitForSeconds(1f / updatesPerSecond);
            }
        }

        private void SortCheckPointActors()
        {
            checkPointActors = checkPointActors.OrderByDescending(c => c.CurrentTargetIndex).ToList();
        }
        
        private void SetTargetGroup()
        {
            int safeLoop = 100;
            while (cinemachineGroup.m_Targets.Length > 0)
            {
                cinemachineGroup.RemoveMember(cinemachineGroup.m_Targets[0].target);
                safeLoop--;
                if (safeLoop < 0)
                {
                    Debug.Log("doesn't work");
                    break;
                }
            }

            for (int i = 0; i < FirstAmountOfTargets; i++)
            {
                if (i >= checkPointActors.Count)
                    break;

                if (i == 0)
                {
                    cinemachineGroup.AddMember(checkPointActors[i].transform,firstWeight,0);
                }
                else if (i == 1)
                {
                    cinemachineGroup.AddMember(checkPointActors[i].transform,secondWeight,0);
                }
                else if (i == 2)
                {
                    cinemachineGroup.AddMember(checkPointActors[i].transform,thirdWeight,0);
                }
                else
                {
                    cinemachineGroup.AddMember(checkPointActors[i].transform,defaultWeight,0);
                }

            }
        }
    }
}
