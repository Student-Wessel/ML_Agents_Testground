using System;
using UnityEngine;

namespace Utils
{
    [ExecuteInEditMode]
    public class TestForward : MonoBehaviour
    {
        public Vector3 debugForward;

        private void Update()
        {
            debugForward = transform.forward;
        }
    }
}
