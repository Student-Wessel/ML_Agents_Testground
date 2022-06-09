using CustomAgent;
using RaceAgentScripts.TrackScripts;
using UnityEngine;

namespace RaceAgentScripts
{
    public interface ICheckPointActor
    {
        public void CheckPointReached(CheckPoint pCheckPoint);
    }
}
