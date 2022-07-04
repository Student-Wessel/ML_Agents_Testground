using System;
using AgentUtils;
using CustomAgent;
using CustomPropertyDrawers;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Utils;

namespace ShootOut
{
    [RequireComponent(typeof(CharacterController),typeof(GunHandler))]

    public class ShootOutAgent : EnvironmentAgent , ISpawnable , IShootable
    {
        [SerializeField] private float moveSpeed = 5f,rotationSpeed = 10f;

        private CharacterController chc;
        private GunHandler gunHandler;
        private Vector3 desiredLookRotation;
        private Vector3 desiredWalkDirection;
        
        private void Awake()
        {
            gunHandler = GetComponent<GunHandler>();
            chc = GetComponent<CharacterController>();
            
            gunHandler.onTargetHit += HandlerOnTargetHit;
        }

        private void HandlerOnTargetHit(GameObject pTarget)
        {
            SetReward(1f);
            environment.ResetEnvironment();
        }

        private void ShootRequest()
        {
            gunHandler.TryShoot();
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     var gun = other.GetComponent<Gun>();
        //     if (gun != null)
        //     {
        //         AddReward(1f);
        //         environment.ResetEnvironment();
        //         return;
        //         if (!gunHandler.HasGun())
        //             gunHandler.GiveGun(gun);
        //     }
        // }

        public void DropGun()
        {
            gunHandler.DropGun();
        }

        // ML Agent stuff
        public override void OnEpisodeBegin()
        {
            gunHandler.ResetGunInHandler();
            environment.ResetEnvironment();
        }
        
        private void FixedUpdate()
        {
            if (desiredLookRotation != Vector3.zero)
            {
                // Quaternion oldRotation = transform.rotation;
                // Quaternion desiredRotation = Quaternion.LookRotation(desiredLookRotation);
                // Quaternion newRotation = Quaternion.RotateTowards(oldRotation, desiredRotation, (rotationSpeed * Time.fixedDeltaTime)*10f);
                // transform.rotation = newRotation;   
            }
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(gunHandler.HasGun()); // 1
            sensor.AddObservation(gunHandler.BulletCount()); // 1
        }
        
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            actionsOut.DiscreteActions.Array[0] = 1;
            actionsOut.DiscreteActions.Array[1] = 1;
            
            if (Input.GetKey(KeyCode.W))
            {
                actionsOut.DiscreteActions.Array[0] = 2;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                actionsOut.DiscreteActions.Array[0] = 0;
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                actionsOut.DiscreteActions.Array[1] = 0;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                actionsOut.DiscreteActions.Array[1] = 2;                
            }

            if (Input.GetMouseButton(0))
            {
                actionsOut.DiscreteActions.Array[2] = 1;
            }

            var mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,21));
            var direction = (mouseWorld - transform.position).normalized;
            actionsOut.ContinuousActions.Array[0] = direction.x;
            actionsOut.ContinuousActions.Array[1] = direction.z;
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            var rawForwardInput = actions.DiscreteActions[0]; // 0_1_2
            var rawSideInput = actions.DiscreteActions[1]; // 0_1_2
            var rawShootInput = actions.DiscreteActions[2]; // 0_1

            var rawXRotationInput = actions.ContinuousActions[0];
            var rawZRotationInput = actions.ContinuousActions[1];

            var forwardInput = rawForwardInput - 1; // -1_0_1
            var sideInput = rawSideInput - 1; // -1_0_1

            ShootInput input = new ShootInput();
            input.forwardInput = forwardInput;
            input.sideInput = sideInput;
            input.wantsShoot = rawShootInput == 1;
            input.lookRotation = new Vector2(rawXRotationInput, rawZRotationInput);
            
            desiredLookRotation = new Vector3(input.lookRotation.x, 0, input.lookRotation.y);
            //desiredWalkDirection = new Vector3(input.sideInput, 0, input.forwardInput).normalized;
            desiredWalkDirection = (transform.forward * forwardInput + transform.right * sideInput).normalized;

            float dot = Vector3.Dot(transform.forward, desiredWalkDirection);
            float scale = MathExtension.Map(dot, -1f, 1f, 0.5f, 1f);
            //chc.Move(desiredWalkDirection * moveSpeed * scale * Time.fixedDeltaTime);
            
            if (input.wantsShoot)
                ShootRequest();

            AddReward(-(1f/MaxStep));
        }
        
        public void SpawnPosition(Vector3 pPosition)
        {
            chc.enabled = false;
            transform.position = new Vector3(pPosition.x,0,pPosition.z);
            chc.enabled = true;
        }

        public void SpawnRotation(Quaternion pRotation)
        {
            chc.enabled = false;
            transform.rotation = pRotation;
            chc.enabled = true;
        }

        public void SpawnPositionRotation(Vector3 pPosition, Quaternion pRotation)
        {
            chc.enabled = false;
            transform.position = new Vector3(pPosition.x,0,pPosition.z);
            transform.rotation = pRotation;
            chc.enabled = true;
        }

        public void OnShot()
        {
            int stepsLeft = MaxStep - StepCount;
            AddReward((-(1f/MaxStep))*stepsLeft);
        }
    }
    
    public struct ShootInput
    {
        public int forwardInput;
        public int sideInput;
        public bool wantsShoot;
        public Vector2 lookRotation;
    }
}
