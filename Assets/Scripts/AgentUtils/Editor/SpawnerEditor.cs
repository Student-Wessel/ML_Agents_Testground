using System;
using UnityEditor;
using UnityEngine;

namespace AgentUtils.Editor
{
    [CustomEditor(typeof(Spawner))]
    public class SpawnerEditor : UnityEditor.Editor
    {
        private Spawner spawner;
        
        private void OnEnable()
        {
            spawner = (Spawner) target;
            spawner.RemoveNullZones();
        }

        public override void OnInspectorGUI()
        {
            
            if (GUILayout.Button("Get all spawn zones in children"))
            {
                var SpawnZones = spawner.GetComponentsInChildren<SpawnZone>();
                foreach (var spawnZone in SpawnZones)
                    spawner.AddSpawnZone(spawnZone);
            }
            base.OnInspectorGUI();
        }
    }
}
