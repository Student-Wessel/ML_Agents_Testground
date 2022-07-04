using UnityEngine;

namespace AgentUtils
{
    public class SimpleSpawnable : MonoBehaviour, ISpawnable
    {

        public void SpawnPosition(Vector3 pPosition)
        {
            transform.localPosition = new Vector3(pPosition.x,0,pPosition.z);
        }

        public void SpawnRotation(Quaternion pRotation)
        {
        }

        public void SpawnPositionRotation(Vector3 pPosition, Quaternion pRotation)
        {
            transform.localPosition = new Vector3(pPosition.x,0,pPosition.z);
        }
    }
}
