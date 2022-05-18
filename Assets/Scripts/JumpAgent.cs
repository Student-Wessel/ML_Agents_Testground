using System;
using CustomAgent;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(Collider))]
public class JumpAgent : EnvironmentAgent
{
    [SerializeField] private float jumpPower,fallAcceleration = 5f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckHeight;
    [SerializeField] private GameObject distanceTarget;

    private Rigidbody rb;
    
    // Agent logic
    private bool jumpRequest = false;
    private bool isGrounded = false;
    private bool doJump = false;
    private float targetDistance = 10;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        targetDistance = (distanceTarget.transform.position - transform.position).magnitude;

        if (doJump)
        {
            rb.AddForce(Vector3.up*jumpPower,ForceMode.VelocityChange);
            doJump = false;
        }

        if (rb.velocity.y < 0)
        {
            rb.AddForce(Vector3.down*fallAcceleration,ForceMode.Acceleration);
        }
    }

    public override void OnEpisodeBegin()
    {
        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> newAction = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.Space))
        {
            newAction[0] = 1;
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        jumpRequest = actions.DiscreteActions[0] == 1;
        
        if (isGrounded && jumpRequest && rb.velocity.y <= 0)
            doJump = true;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(targetDistance);
    }

    public void ResetPositionVelocity(Vector3 pLocalPosition,Vector3 pVelocity)
    {
        transform.localPosition = pLocalPosition;
        rb.velocity = pVelocity;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckHeight,groundMask);
    }
}
