using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils;

namespace RaceAgentScripts.TrackScripts
{
    public class TrackPiece : MonoBehaviour , ICheckPointCollection
    {
        [SerializeField] private List<CheckPoint> checkPoints;

        public List<CheckPoint> GetCheckPointsInOrder()
        {
            return new List<CheckPoint>(checkPoints);
        }

        public void FlipDirection()
        {
            checkPoints.Reverse();

            for (int i = 0; i < checkPoints.Count; i++)
            {
                checkPoints[i].name = "CheckPoint_"+(i+1);
            }

            foreach (var ck in checkPoints)
            {
                ck.FlipAbsoluteDirection();
            }
        }
    }
}
