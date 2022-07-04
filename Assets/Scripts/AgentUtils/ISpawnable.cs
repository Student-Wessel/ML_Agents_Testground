using UnityEngine;

namespace AgentUtils
{
    public interface ISpawnable
    {
        public void SpawnPosition(Vector3 pPosition);
        public void SpawnRotation(Quaternion pRotation);
        public void SpawnPositionRotation(Vector3 pPosition,Quaternion pRotation);
    }
}
