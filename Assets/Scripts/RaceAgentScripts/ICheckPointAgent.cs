using CustomAgent;
using UnityEngine;

namespace RaceAgentScripts
{
    public interface ICheckPointAgent
    {
        public void CheckPointReached(CheckPoint pCheckPoint);
    }
}
