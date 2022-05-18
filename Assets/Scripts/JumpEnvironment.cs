using System;
using CustomAgent;
using CustomPropertyDrawers;
using UnityEditor.UIElements;
using UnityEngine;

public class JumpEnvironment : LearningEnvironment
{
    [TagSelector] public string ObstacleClearTag,PlayerTag = "";

    [SerializeField] private Material green, red, standard;
    [SerializeField] private JumpObstacle obstacleTrigger;
    [SerializeField] private Vector3 obstacleStartPosition;
    [SerializeField] private MeshRenderer successVisual;

    private JumpAgent jumpAgent;
    public override void Awake()
    {
        base.Awake();
        
        obstacleTrigger.OnTriggerEnterEvent += ObstacleTriggerEnter;
        obstacleTrigger.transform.localPosition = obstacleStartPosition;
        //jumpAgent = (JumpAgent)agents[0];
        obstacleTrigger.SetVelocity();
    }

    public override void ResetEnvironment()
    {
        
    }

    private void ObstacleTriggerEnter(string collisionTag)
    {
        // if (collisionTag == ObstacleClearTag)
        // {
        //     SetRewardAllAgents(1f);
        //     EndEpisodeForAll();
        //     jumpAgent.ResetPositionVelocity(Vector3.zero,Vector3.zero);
        //     obstacleTrigger.transform.localPosition = obstacleStartPosition;
        //     obstacleTrigger.SetVelocity();
        //     successVisual.material = green;
        // }
        // else if (collisionTag == PlayerTag)
        // {
        //     SetRewardAllAgents(-1f);
        //     EndEpisodeForAll();
        //     jumpAgent.ResetPositionVelocity(Vector3.zero,Vector3.zero);
        //     obstacleTrigger.transform.localPosition = obstacleStartPosition;
        //     obstacleTrigger.SetVelocity();
        //     successVisual.material = red;
        // }
    }

    public void Update()
    {
        
    }
}
