using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaceAgentScripts
{
    public class CheckPointList : MonoBehaviour
    {
        [SerializeField] private List<CheckPoint> checkPointsInOrder;

        private void Awake()
        {
            
        }

        public void SetOrder()
        {
            checkPointsInOrder = new List<CheckPoint>();

            foreach (Transform child in transform)
            {
                var checkPoint = child.GetComponent<CheckPoint>();

                if (checkPoint != null)
                {
                    checkPointsInOrder.Add(checkPoint);
                    checkPoint.name = "CheckPoint_" + checkPointsInOrder.Count;
                }
            }
        }
        
        public int Count { get => checkPointsInOrder.Count; }
        public CheckPoint this[int index]
        {
            get => checkPointsInOrder[index];
            set => checkPointsInOrder[index] = value;
        }
    }
}
