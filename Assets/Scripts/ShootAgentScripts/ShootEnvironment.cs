using System;
using AgentUtils;
using CustomAgent;
using CustomPropertyDrawers;
using UnityEngine;

namespace ShootAgentScripts
{
    public class ShootEnvironment : LearningEnvironment
    {
        [SerializeField] private Spawner agentSpawner,goalSpawner;
        private void Start()
        {
            agentSpawner.SpawnToRandomPositionAndRotation();
            goalSpawner.SpawnToRandomPosition();
        }

        public override void ResetEnvironment()
        {
            agentSpawner.SpawnToRandomPositionAndRotation();
            goalSpawner.SpawnToRandomPosition();
        }

        public override void ShowEpisodeResult(EpisodeEndResult result)
        {
            
        }
    }
}
