using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using UnityEngine;

namespace CustomAgent
{
     public abstract class LearningEnvironment : MonoBehaviour
     {
          public virtual void Awake()
          {
               AttachAllAgents();
          }
          private void AttachAllAgents()
          {
               var agents = GetComponentsInChildren<EnvironmentAgent>().ToList();
               foreach (var agent in agents)
               {
                    agent.AttachToEnvironment(this);
               }
          }

          public abstract void ResetEnvironment();

          public virtual void ShowEpisodeResult(EpisodeEndResult result) { }
     }
}
