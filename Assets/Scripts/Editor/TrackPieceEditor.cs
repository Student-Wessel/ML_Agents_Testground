using System;
using RaceAgentScripts.TrackScripts;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TrackPiece))]
    public class TrackPieceEditor : UnityEditor.Editor
    {
        private TrackPiece trackPiece;
        
        private void OnEnable()
        {
            trackPiece = (TrackPiece) target;
        }

        public override void OnInspectorGUI()
        {
            // if (GUILayout.Button("Flip Direction"))
            // {
            //     trackPiece.FlipDirection();
            // }

            base.OnInspectorGUI();
        }
    }
}
