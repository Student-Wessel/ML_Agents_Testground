using UnityEngine;

namespace AgentUtils
{
    [RequireComponent(typeof(BoxCollider))]
    public class SpawnZone : MonoBehaviour
    {
        [SerializeField] private bool hideZoneOnPlay = true;
        
        private BoxCollider boxCl;
        private void Awake()
        {
            boxCl = GetComponent<BoxCollider>();
            boxCl.isTrigger = true;

            if (hideZoneOnPlay)
            {
                var meshRender = GetComponent<MeshRenderer>();
                meshRender.enabled = false;                
            }
        }

        public Vector3 RandomPositionInBounds()
        {
            Bounds bounds = boxCl.bounds;
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }
    }
}
