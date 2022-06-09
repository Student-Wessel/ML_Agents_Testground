using System;
using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(Collider))]
    public class ChildCollider : MonoBehaviour
    {
        public bool checkTriggerEnter = false, checkTriggerStay = false, checkTriggerExit = false,
            checkCollisionEnter= false ,checkCollisionStay = false,checkCollisionExit = false;

        public event Action<Collider> TriggerEntered, TriggerStayed, TriggerExited;
        public event Action<Collision> CollisionEnter, CollisionStayed, CollisionExited;

        private void OnTriggerEnter(Collider other)
        {
            if (!checkTriggerEnter)
                return;
            TriggerEntered?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!checkTriggerStay)
                return;
            TriggerStayed?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!checkTriggerExit)
                return;
            TriggerExited?.Invoke(other);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!checkCollisionEnter)
                return;
            CollisionEnter?.Invoke(other);
        }

        private void OnCollisionStay(Collision other)
        {
            if (!checkCollisionStay)
                return;
            CollisionStayed?.Invoke(other);
        }

        private void OnCollisionExit(Collision other)
        {
            if (!checkCollisionExit)
                return;
            CollisionExited?.Invoke(other);
        }
    }
}
