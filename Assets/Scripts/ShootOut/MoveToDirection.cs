using System;
using System.Collections;
using System.Collections.Generic;
using AgentUtils;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MoveToDirection : MonoBehaviour, ISpawnable
{
    public float speed;
    public Vector3 direction;

    private CharacterController ch;

    private void Awake()
    {
        ch = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        ch.Move(direction.normalized * Time.fixedDeltaTime * speed);
    }

    public void SpawnPosition(Vector3 pPosition)
    {
        ch.enabled = false;
        transform.position = new Vector3(pPosition.x, 0, pPosition.z);
        ch.enabled = true;
    }

    public void SpawnRotation(Quaternion pRotation)
    {
    }

    public void SpawnPositionRotation(Vector3 pPosition, Quaternion pRotation)
    {
    }
}
