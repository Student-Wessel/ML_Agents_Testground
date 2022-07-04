using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace AimScripts
{
    public class AimController : MonoBehaviour
    {
        [SerializeField] private float sensX, sensY,maxVerticalAngle = 90f;
        [SerializeField] private bool lockMouse;
        [SerializeField] private GameObject bodyObject;
        private float xRotation = 0f;
        public Vector3 AimForward => transform.forward;

        private void Awake()
        {
            if (lockMouse)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public void ResetAim()
        {
            xRotation = 0f;
            transform.localRotation = Quaternion.Euler(xRotation,0f,0f);
            bodyObject.transform.rotation = Quaternion.identity;
        }

        public void MoveAim(float pX,float pY)
        {
            xRotation -= pY * sensY;
            xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);
            
            transform.localRotation = Quaternion.Euler(xRotation,0f,0f);
            bodyObject.transform.Rotate(Vector3.up* pX * sensX);
        }
    }
}
