using System;
using System.Collections.Generic;
using CustomPropertyDrawers;
using UnityEngine;
using Utils;

namespace RaceAgentScripts
{
    public class CheckPointActor : MonoBehaviour , ICheckPointActor
    {
        [TagSelector] 
        [SerializeField] 
        private string checkPointTag;

        [SerializeField] private bool hasTimeLimit;
        [SerializeField] private float maxTimeBetweenCheckpoints = 5f;
        
        [SerializeField] private CheckPointList checkPoints;
        
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
        
        public CheckPoint CurrentTarget { get => checkPoints[checkPointTargetIndex]; }
        public int CurrentTargetIndex { get => checkPointTargetIndex; }
        public int checkPointCount { get => checkPoints.Count; }

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
                    checkPointFailed?.Invoke(checkPoints[checkPointTargetIndex]);
                }
            }
        }

        // Value between 1-0 depending on how long the current time between checkpoints is
        public float NormalizedTime()
        {
            return MathExtension.Map(timeBetweenCheckpoints,0f,maxTimeBetweenCheckpoints,0f,1f);
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

        public void CheckPointReached(CheckPoint pCheckPoint)
        {
            // We reach the correct check point
            if (pCheckPoint == checkPoints[checkPointTargetIndex])
            {
                // We reach the finish line
                if (checkPointTargetIndex >= checkPoints.Count-1)
                {
                    checkPointTargetIndex = 0;
                    checkPointReachedFinish?.Invoke(pCheckPoint);
                }
                else
                {
                    checkPointTargetIndex++;
                    checkPointReachedCorrect?.Invoke(pCheckPoint);
                }
                timeBetweenCheckpoints = maxTimeBetweenCheckpoints;
            }
            // Reached wrong checkpoint
            else
            {
                checkPointReachedWrong?.Invoke(pCheckPoint);
            }
        }
    }
}
