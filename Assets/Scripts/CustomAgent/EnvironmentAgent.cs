using System;
using Unity.MLAgents;
using UnityEngine;

// Use EpisodeEndRequest instead of EndEpisode
namespace CustomAgent
{
    public class EnvironmentAgent : Agent
    {
        protected LearningEnvironment environment;
        public void AttachToEnvironment(LearningEnvironment pEnvironment)
        {
            environment = pEnvironment;
        }
    }
}
