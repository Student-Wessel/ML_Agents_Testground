using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Utils;

namespace AimScripts
{
    public class AimTargetCollector : MonoBehaviour
    {
        [SerializeField] private List<AimTarget> targets;
        [SerializeField] private float maxViewDistance = 5f,viewValue = 0.95f,distanceLeanValue = 0.10f;
        [SerializeField] private float scansPerSecond = 2f;

        private float timeBetweenScan;
        
        private List<AimTarget> targetsInView;
        private void Awake()
        {
            targetsInView = new List<AimTarget>();
            
            timeBetweenScan = 1f / scansPerSecond;
            StartCoroutine(ScanForTargets());
        }

        public ReadOnlyCollection<AimTarget> GetTargetsInView()
        {
            return targetsInView.AsReadOnly();
        }

        public AimTarget GetClosedTarget()
        {
            float closedTarget = float.MaxValue;
            AimTarget result = null;

            foreach (var target in targetsInView)
            {
                float distance = (target.transform.position - transform.position).sqrMagnitude;
                if (distance < closedTarget)
                {
                    closedTarget = distance;
                    result = target;
                }
            }

            return result;
        }

        public void ResetTargets()
        {
            SetAllTargetsActive();
        }
        
        private void SetAllTargetsActive()
        {
            foreach (var target in targets)
                target.gameObject.SetActive(true);
        }

        private IEnumerator ScanForTargets()
        {
            while (true)
            {
                var aimForward = transform.forward;
                var aimPosition = transform.position;

                foreach (var target in targets)
                {
                    var delta = target.transform.position - aimPosition;
                    var direction = delta.normalized;
                    float distance = delta.magnitude;
                    
                    if (distance < maxViewDistance)
                    {
                        float dot = Vector3.Dot(aimForward, direction);
                        float bonusValue = maxViewDistance - distance;
                        float leanValue = MathExtension.Map(bonusValue, 0f, 5f, 0f, distanceLeanValue);

                        // In view
                        if (dot > viewValue - leanValue)
                        {
                            if (!targetsInView.Contains(target))
                                targetsInView.Add(target);
                        }
                        else
                        {
                            if (targetsInView.Contains(target))
                                targetsInView.Remove(target);
                        }
                    }
                }
                yield return new WaitForSeconds(timeBetweenScan);
            }
        }
    }
}
