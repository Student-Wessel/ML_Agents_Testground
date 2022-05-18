using AgentUtils;
using CustomAgent;
using UnityEngine;

namespace MoveAgentScripts
{
    public class MoveEnvironment : LearningEnvironment
    {
        [SerializeField] private Spawner agentSpawner,goalSpawner;
        [SerializeField] private Material mGreen, mRed;
        [SerializeField] private MeshRenderer groundRenderer;
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
            if (result.reward > 0)
            {
                groundRenderer.material = mGreen;
            }
            else
            {
                groundRenderer.material = mRed;
            }
        }
    }
}
