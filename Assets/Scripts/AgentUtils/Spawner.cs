using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AgentUtils
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private List<SpawnZone> spawnZones;
        [SerializeField] private List<GameObject> spawnObjects;
        private List<ISpawnable> spawnables; 

        private void OnValidate()
        {
            RemoveNullZones();
        }
        private void Awake()
        {
            RemoveNullZones();
            
            spawnables = new List<ISpawnable>();

            for (int i = 0; i < spawnObjects.Count; i++)
            {
                var spawnable = spawnObjects[i].GetComponent<ISpawnable>();
                
                if (spawnable == null)
                {
                    Debug.LogError("Game object is not an ISpawnable");
                    spawnObjects.RemoveAt(i);
                    i--;
                }
                spawnables.Add(spawnable);
            }
        }

        public void SpawnToRandomPosition()
        {
            for (int i = 0; i < spawnables.Count; i++)
            {
                var spawnable = spawnables[i];
                int randomIndex = Random.Range(0, spawnZones.Count);
                if (spawnZones.Count == 1)
                    randomIndex = 0;
                Vector3 randomPositionInBounds = spawnZones[randomIndex].RandomPositionInBounds();
                spawnable.SpawnPosition(randomPositionInBounds);
            }
        }
        
        public void SpawnToRandomPositionAndRotation()
        {
            for (int i = 0; i < spawnables.Count; i++)
            {
                var spawnable = spawnables[i];
                int randomIndex = Random.Range(0, spawnZones.Count);
                if (spawnZones.Count == 1)
                    randomIndex = 0;
                Vector3 randomPositionInBounds = spawnZones[randomIndex].RandomPositionInBounds();

                var randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                spawnable.SpawnPositionRotation(randomPositionInBounds,randomRotation);
            }
        }

        public void SpawnToRandomPositionSetRotation(Quaternion pRotation)
        {
            for (int i = 0; i < spawnables.Count; i++)
            {
                var spawnable = spawnables[i];
                int randomIndex = Random.Range(0, spawnZones.Count);
                if (spawnZones.Count == 1)
                    randomIndex = 0;
                Vector3 randomPositionInBounds = spawnZones[randomIndex].RandomPositionInBounds();

                spawnable.SpawnPositionRotation(randomPositionInBounds,pRotation);
            }
        }
        public void AddSpawnZone(SpawnZone pSpawnZone)
        {
            if (!spawnZones.Contains(pSpawnZone))
                spawnZones.Add(pSpawnZone);
        }
        public void RemoveNullZones()
        {
            for (int i = 0; i < spawnZones.Count; i++)
            {
                if (spawnZones[i] == null)
                {
                    spawnZones.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
