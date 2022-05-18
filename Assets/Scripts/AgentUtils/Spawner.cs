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
        [SerializeField] private GameObject spawnObject;
        private ISpawnable spawnable; 

        private void OnValidate()
        {
            RemoveNullZones();
        }
        private void Awake()
        {
            RemoveNullZones();
            spawnable = spawnObject.GetComponent<ISpawnable>();
            
            if (spawnable == null)
            {
                Debug.LogError("Game object is not an ISpawnable");
                return;
            }
        }

        public void SpawnToRandomPosition()
        {
            spawnable = spawnObject.GetComponent<ISpawnable>();

            if (spawnable == null)
            {
                Debug.LogError("Game object is not an ISpawnable");
                return;
            }

            SetToRandomPosition();
        }
        
        public void SpawnToRandomPositionAndRotation()
        {
            var spawnable = spawnObject.GetComponent<ISpawnable>();

            if (spawnable == null)
            {
                Debug.LogError("Game object is not an ISpawnable");
                return;
            }
            
            SetToRandomPosition();
            spawnObject.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        }
        
        public void SpawnToRandomPositionSetRotation(Quaternion pRotation)
        {
            var spawnable = spawnObject.GetComponent<ISpawnable>();

            if (spawnable == null)
            {
                Debug.LogError("Game object is not an ISpawnable");
                return;
            }
            
            SetToRandomPosition();
            spawnObject.transform.rotation = pRotation;
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

        private void SetToRandomPosition()
        {
            int randomIndex = Random.Range(0, spawnZones.Count);
            Vector3 randomPositionInBounds = spawnZones[randomIndex].RandomPositionInBounds();
            randomPositionInBounds.y = spawnObject.transform.position.y;
            spawnable.Spawn(randomPositionInBounds);
        }
    }
}
