using System;
using UnityEngine;

namespace TestVars
{
    [ExecuteInEditMode]
    public class TestSign : MonoBehaviour
    {
        [Range(0.0f, 3.14159265358979f)] public float inVar;
        
        public float sinResult;

        private void Update()
        {
            sinResult = Mathf.Sin(inVar);
        }
    }
}
