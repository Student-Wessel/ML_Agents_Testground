using System;
using UnityEngine;

public class JumpObstacle : MonoBehaviour
{
    [SerializeField] private float speed;
    
    public event Action<string> OnTriggerEnterEvent;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other.gameObject.tag);
    }

    public void SetVelocity()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * speed,ForceMode.VelocityChange);
    }
}
