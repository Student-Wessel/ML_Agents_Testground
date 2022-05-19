using System;
using UnityEngine;
using Utils;

namespace TestVars
{
    [ExecuteInEditMode]
    public class AngleCenter : MonoBehaviour
    {
        [SerializeField] private GameObject step, target;

        [SerializeField] private float dotReward;
        
        [SerializeField] private float subtractPerc = 0.01f;
        private void Update()
        {
            Vector3 ownPos = transform.position;
            Vector3 stepPos = step.transform.position;
            Vector3 targetPos = target.transform.position;

            Vector3 stepDelta = stepPos - ownPos;
            Vector3 targetDelta = targetPos - ownPos;

            stepDelta.Normalize();
            targetDelta.Normalize();

            float rawDot = Vector3.Dot(stepDelta, targetDelta);
            float reverseDot = rawDot * -1;
            float difference = MathExtension.Map(reverseDot, -1f, 1f, 0f, 1f);
            float diffSub = difference - (difference * subtractPerc);

            dotReward = Mathf.Clamp(rawDot - diffSub,-1f,1f);
        }
    }
}
