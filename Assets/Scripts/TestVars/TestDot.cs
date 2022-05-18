using System;
using UnityEngine;

namespace TestVars
{
    [ExecuteInEditMode]
    public class TestDot : MonoBehaviour
    {
        [SerializeField] private GameObject objA, objB;
        [SerializeField] private float HowGoodIsTheAngle;

        private void Update()
        {
            Vector3 delta = objB.transform.position - objA.transform.position;
            delta.Normalize();
            
            HowGoodIsTheAngle = Vector3.Dot(delta,transform.forward);
        }
    }
}
