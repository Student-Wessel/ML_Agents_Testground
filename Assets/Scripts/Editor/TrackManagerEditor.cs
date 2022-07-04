using System;
using RaceAgentScripts.TrackScripts;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TrackManager))]
    public class TrackManagerEditor : UnityEditor.Editor
    {
        private TrackManager trackManager;

        private void OnEnable()
        {
            trackManager = (TrackManager) target;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Space(15);

            
            if (GUILayout.Button("Get all checkpoints"))
            {
                trackManager.GetCheckPoints();
            }
            
            if (GUILayout.Button("Set Average Directions"))
            {
                trackManager.SetAverageDirections();
            }
            
            if (GUILayout.Button("Show Average Directions"))
            {
                trackManager.ShowAverageCheckPointDirection();
            }
            
            if (GUILayout.Button("Show Absolute Directions"))
            {
                trackManager.ShowAbsoluteCheckPointDirection();
            }

            if (GUILayout.Button("Show collider visual"))
            {
                trackManager.ShowCheckPointColliders();
            }
            
            if (GUILayout.Button("Hide collider visual"))
            {
                trackManager.HideCheckPointColliders();
            }
            
            if (GUILayout.Button("Show punish visual"))
            {
                trackManager.ShowPunishZones();
            }
            
            if (GUILayout.Button("Hide punish visual"))
            {
                trackManager.HidePunishZones();
            }
            
            GUILayout.Space(15);
            
            base.OnInspectorGUI();
        }
    }
}
