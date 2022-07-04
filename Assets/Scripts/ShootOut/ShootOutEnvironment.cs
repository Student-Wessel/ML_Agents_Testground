using System;
using AgentUtils;
using CustomAgent;
using UnityEngine;

namespace ShootOut
{
    public class ShootOutEnvironment : LearningEnvironment
    {
        [SerializeField] private Spawner blueAgentSpawner, purpleAgentSpawner,gunSpawner;

        public override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            blueAgentSpawner.SpawnToRandomPosition();
            purpleAgentSpawner.SpawnToRandomPosition();
            //gunSpawner.SpawnToRandomPosition();
        }

        public override void ResetEnvironment()
        {
            blueAgentSpawner.SpawnToRandomPosition();
            purpleAgentSpawner.SpawnToRandomPosition();
        }

        public override void ShowEpisodeResult(EpisodeEndResult result)
        {
        }
    }
}
