using CustomPropertyDrawers;
using UnityEngine;

namespace BallUphold
{
    [RequireComponent(typeof(Rigidbody),typeof(Collider))]
    public class UpholdBall : MonoBehaviour
    {
        [SerializeField] private BallUpholdAgent agent;
        [SerializeField] private float minHeadbuttForce = 3f, maxHeadbuttForce = 3f;
        
        private Rigidbody rb;

        private Vector3 startingPosition;

        [TagSelector]
        [SerializeField] private string groundTag,agentTag;

        public Vector3 velocity
        {
            get => rb.velocity;
        }

        private void Awake()
        {
            startingPosition = transform.position;
            rb = GetComponent<Rigidbody>();
            RandomStartVelocity();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(groundTag))
            {
                transform.position = startingPosition;
                rb.velocity = Vector3.zero;
                //RandomStartVelocity();
                agent.OnBallGroundTouch();
            }
            else if (other.gameObject.CompareTag(agentTag))
            {
                HeadbuttVelocity();
            }
        }

        private void HeadbuttVelocity()
        {
            var ownPosition = transform.position;
            var ballPosition = agent.transform.position;

            var delta = (ownPosition - ballPosition).normalized;
            float upForce = Random.Range(minHeadbuttForce, maxHeadbuttForce);
            delta.y = upForce;
            rb.velocity = delta;
        }

        private void RandomStartVelocity()
        {
            var randomPosition = Random.insideUnitCircle;
            Vector3 newVelocity = new Vector3(randomPosition.x, 0, randomPosition.y);
            rb.AddForce(newVelocity,ForceMode.VelocityChange);
        }
    }
}
