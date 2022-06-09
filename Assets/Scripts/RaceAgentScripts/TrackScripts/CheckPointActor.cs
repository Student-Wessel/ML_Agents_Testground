using System;
using System.Collections.Generic;
using CustomPropertyDrawers;
using UnityEngine;

namespace RaceAgentScripts.TrackScripts
{
    public class CheckPointActor : MonoBehaviour
    {
        [TagSelector] 
        [SerializeField] 
        private string checkPointTag;

        [SerializeField] private bool hasTimeLimit;
        [SerializeField] private float maxTimeBetweenCheckpoints = 5f;
        
        [SerializeField] private TrackManager trackManager;
        
        // Called when you reached the correct check point
        public event Action<CheckPoint> checkPointReachedCorrect;
        // Called when you reach the wrong check point
        public event Action<CheckPoint> checkPointReachedWrong;
        // Called when you reach the finish
        public event Action<CheckPoint> checkPointReachedFinish;
        // Called when you reach the maximum time that is set, and time limit is true
        public event Action<CheckPoint> checkPointFailed;

        private int checkPointTargetIndex = 0;
        private float timeBetweenCheckpoints;
        public CheckPoint CurrentTarget { get => trackManager[checkPointTargetIndex]; }
        public int CurrentTargetIndex { get => checkPointTargetIndex; }
        public int checkPointCount { get => trackManager.checkPointCount; }

        private void Awake()
        {
            timeBetweenCheckpoints = maxTimeBetweenCheckpoints;
        }

        private void FixedUpdate()
        {
            if (hasTimeLimit)
            {
                timeBetweenCheckpoints -= Time.fixedDeltaTime;

                if (timeBetweenCheckpoints <= 0)
                {
                    checkPointFailed?.Invoke(trackManager[checkPointTargetIndex]);
                }
            }
        }

        public void ResetTime()
        {
            timeBetweenCheckpoints = maxTimeBetweenCheckpoints;
        }
        
        public void ResetActor()
        {
            timeBetweenCheckpoints = maxTimeBetweenCheckpoints;
            checkPointTargetIndex = 0;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag(checkPointTag))
                return;
            
            var checkPoint = other.GetComponentInParent<CheckPoint>();
            int checkPointIndex = trackManager.GetCheckPointIndex(checkPoint);

            if (checkPoint == trackManager[checkPointTargetIndex])
            {
                // We reach the finish line
                if (checkPointTargetIndex >= trackManager.checkPointCount-1)
                {
                    checkPointTargetIndex = 0;
                    checkPointReachedFinish?.Invoke(checkPoint);
                }
                else
                {
                    checkPointTargetIndex++;
                    checkPointReachedCorrect?.Invoke(checkPoint);
                }
                timeBetweenCheckpoints = maxTimeBetweenCheckpoints;
            }
            else
            {
                if (checkPointIndex - 3 < checkPointTargetIndex)
                {
                    checkPointReachedWrong?.Invoke(checkPoint);
                }
            }
            
        }
    }
}
