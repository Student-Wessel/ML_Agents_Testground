using System;
using System.Collections.Generic;
using UnityEngine;

namespace RaceAgentScripts
{
    public class CheckPointManager : MonoBehaviour
    {
        [SerializeField] private List<CheckPoint> checkPointOrder;
        public int HighestCheckpointIndex { get; private set; }

        private void Awake()
        {
            HighestCheckpointIndex = checkPointOrder.Count-1;
        }

        public int GetCheckPointIndex(CheckPoint pCheckPoint)
        {
            if (checkPointOrder.Contains(pCheckPoint))
            {
                return checkPointOrder.IndexOf(pCheckPoint);
            }

            return -1;
        }
    }
}
