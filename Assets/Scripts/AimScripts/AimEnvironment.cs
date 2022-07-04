using System;
using AgentUtils;
using CustomAgent;
using UnityEngine;

namespace AimScripts
{
    public class AimEnvironment : LearningEnvironment
    {
        [SerializeField] private Spawner targetSpawner;

        private void Start()
        {
            targetSpawner.SpawnToRandomPosition();
        }

        public override void ResetEnvironment()
        {
            targetSpawner.SpawnToRandomPosition();
        }

        public override void ShowEpisodeResult(EpisodeEndResult result)
        {
            
        }
    }
}
