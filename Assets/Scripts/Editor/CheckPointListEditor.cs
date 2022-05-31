using System;
using RaceAgentScripts;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(CheckPointList))]
    public class CheckPointListEditor : UnityEditor.Editor
    {
        private CheckPointList list;
        private void OnEnable()
        {
            list = (CheckPointList) target;
            list.SetOrder();
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Set list order"))
                list.SetOrder();

            base.OnInspectorGUI();
        }
    }
}
