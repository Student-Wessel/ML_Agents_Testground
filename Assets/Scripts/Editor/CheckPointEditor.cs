using System;
using RaceAgentScripts;
using RaceAgentScripts.TrackScripts;
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
            base.OnInspectorGUI();
        }
    }
}
