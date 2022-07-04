using System;
using System.Collections.Generic;
using UnityEngine;

namespace RaceAgentScripts.TrackScripts
{
    public class TrackManager : MonoBehaviour
    {
        [Tooltip("Amount of check point directions is used to calculate the average")]
        [SerializeField] private int AverageDirectionSum = 2;
        [SerializeField] private List<CheckPoint> checkPointsInOrder;

        // The parent transform where all the track pieces are located in order
        [SerializeField] private GameObject trackPiecesParent;
        public int checkPointCount { get => checkPointsInOrder.Count; }
        public CheckPoint this[int i]
        {
            get => checkPointsInOrder[i];
        }
        
        private Dictionary<CheckPoint, int> checkPointIndexes;

        private void Awake()
        {
            SetAverageDirections();
        }

        private void Start()
        {
            checkPointIndexes = new Dictionary<CheckPoint, int>();

            // faster way to the the index of an checkpoint
            for (int i = 0; i < checkPointsInOrder.Count; i++)
                checkPointIndexes.Add(checkPointsInOrder[i],i);
        }

        public void GetCheckPoints()
        {
            checkPointsInOrder.Clear();

            foreach (Transform child in trackPiecesParent.transform)
            {
                var list = child.GetComponent<ICheckPointCollection>().GetCheckPointsInOrder();
                foreach (var cp in list)
                    checkPointsInOrder.Add(cp);
            }
        }
        
        public void SetAverageDirections()
        {
            GetCheckPoints();
            
            int checkPointCount = checkPointsInOrder.Count;
            int maxIndex = checkPointCount;
            
            for (int i = 0; i < checkPointCount; i++)
            {
                CheckPoint checkPoint = checkPointsInOrder[i];
                Vector3 sumVector = checkPoint.AbsoluteDirection;

                for (int forwardIndex = 1; forwardIndex < AverageDirectionSum; forwardIndex++)
                {
                    int index = (((i+forwardIndex) % maxIndex) + maxIndex) % maxIndex;
                    sumVector += checkPointsInOrder[index].AbsoluteDirection;
                }
                
                checkPoint.SetAverageDirection(sumVector.normalized);
            }
        }

        public int GetCheckPointIndex(CheckPoint pCheckPoint)
        {
            return checkPointIndexes[pCheckPoint];
        }
        
        public void ShowAverageCheckPointDirection()
        {
            foreach (var checkPoint in checkPointsInOrder)
            {
                checkPoint.ShowAverageDirection();
            }
        }
        
        public void ShowAbsoluteCheckPointDirection()
        {
            foreach (var checkPoint in checkPointsInOrder)
            {
                checkPoint.ShowAbsoluteDirection();
            }
        }
        
        public void ShowCheckPointColliders()
        {
            foreach (var checkPoint in checkPointsInOrder)
            {
                checkPoint.ShowCollider();
            }
        }

        public void HideCheckPointColliders()
        {
            foreach (var checkPoint in checkPointsInOrder)
            {
                checkPoint.HideCollider();
            }
        }

        public void ShowPunishZones()
        {
            RecursiveSetPunishZoneVisual(true, trackPiecesParent.transform);
        }

        public void HidePunishZones()
        {
            RecursiveSetPunishZoneVisual(false, trackPiecesParent.transform);
        }

        private void RecursiveSetPunishZoneVisual(bool show,Transform pTransform)
        {
            if (pTransform.CompareTag("PunishZone"))
            {
                var renderer = pTransform.GetComponent<MeshRenderer>();
                if (renderer != null)
                    renderer.enabled = show;
            }
            
            foreach (Transform child in pTransform)
            {
                RecursiveSetPunishZoneVisual(show,child);
            }
        }
        
    }
}
