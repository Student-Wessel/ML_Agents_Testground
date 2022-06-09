using System.Collections.Generic;
using UnityEngine;

namespace RaceAgentScripts.TrackScripts
{
    public interface ICheckPointCollection
    {
        public List<CheckPoint> GetCheckPointsInOrder();
    }
}
