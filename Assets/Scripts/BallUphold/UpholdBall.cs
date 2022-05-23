using System;
using CustomPropertyDrawers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BallUphold
{
    [RequireComponent(typeof(Rigidbody),typeof(Collider))]
    public class UpholdBall : MonoBehaviour
    {
        [SerializeField] private BallUpholdAgent agent;
        [SerializeField] private float minHeadbuttForce = 3f, maxHeadbuttForce = 3f;
        [SerializeField] private float rewardCooldown = 0.5f;
        
        private Rigidbody rb;
        private Vector3 startingPosition;
        private float rewardCountdown;
        private bool canGetRewared = true;

        [TagSelector]
        [SerializeField] private string groundTag,agentHeadTag;

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

        private void FixedUpdate()
        {
            if (rewardCountdown >= 0)
            {
                rewardCountdown -= Time.fixedDeltaTime;
                if (rewardCountdown <= 0)
                {
                    canGetRewared = true;
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(groundTag))
            {
                agent.OnBallGroundTouch();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(agentHeadTag))
            {
                HeadbuttVelocity();
                if (canGetRewared)
                { 
                    agent.OnHeadButt();
                    rewardCountdown = rewardCooldown;
                    canGetRewared = false;
                }
            }
        }

        private void HeadbuttVelocity()
        {
            // var ownPosition = transform.position;
            // var ballPosition = agent.transform.position;
            //
            // var delta = (ownPosition - ballPosition);
            // delta.y = 0;
            // delta = delta.normalized;

            var delta2D = Random.insideUnitCircle;
            Vector3 delta = new Vector3(delta2D.x,0,delta2D.y);

            delta *= 2;
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

        public void ResetBall()
        {
            transform.position = startingPosition;
            rb.velocity = Vector3.zero;
            canGetRewared = true;
            rewardCountdown = 0;
        }
    }
}
