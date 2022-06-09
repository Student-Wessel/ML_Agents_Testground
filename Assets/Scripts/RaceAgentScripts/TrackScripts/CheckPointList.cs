using System.Collections.Generic;
using UnityEngine;

namespace RaceAgentScripts.TrackScripts
{
    public class CheckPointList : MonoBehaviour
    {
        [SerializeField] private List<CheckPoint> checkPointsInOrder;
        public int Count { get => checkPointsInOrder.Count; }
        public CheckPoint this[int index]
        {
            get => checkPointsInOrder[index];
            set => checkPointsInOrder[index] = value;
        }
    }
}
