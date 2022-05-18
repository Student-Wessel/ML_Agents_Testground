using UnityEngine;

namespace AgentUtils
{
    public class SimpleSpawnable : MonoBehaviour, ISpawnable
    {
        public void Spawn(Vector3 pPosition)
        {
            transform.position = pPosition;
        }
    }
}
