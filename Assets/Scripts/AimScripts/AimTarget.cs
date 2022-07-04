using AgentUtils;
using UnityEngine;

namespace AimScripts
{
    [RequireComponent(typeof(Collider))]
    public class AimTarget : MonoBehaviour , ISpawnable
    {
        public void SpawnPosition(Vector3 pPosition)
        {
            transform.position = pPosition;
        }

        public void SpawnRotation(Quaternion pRotation)
        {
        }

        public void SpawnPositionRotation(Vector3 pPosition, Quaternion pRotation)
        {
            transform.position = pPosition;
        }
    }
}
