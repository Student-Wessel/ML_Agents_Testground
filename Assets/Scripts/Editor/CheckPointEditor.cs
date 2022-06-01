using System;
using RaceAgentScripts;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(CheckPoint))]
    [CanEditMultipleObjects]
    public class CheckPointEditor : UnityEditor.Editor
    {
        private CheckPoint checkPoint;
        
        private void OnEnable()
        {
            checkPoint = (CheckPoint) target;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Flip Direction"))
            {
                foreach (var targetObj in targets)
                {
                    CheckPoint targetCheckPoint = (CheckPoint) targetObj;
                    targetCheckPoint.FlipAbsoluteDirectionDirection();
                }
            }
            
            base.OnInspectorGUI();
        }
    }
}
