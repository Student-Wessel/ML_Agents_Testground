using System;
using UnityEngine;

namespace TestAnimation
{
    public class MyAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private GeneralStateAnnouncer stateAnnouncer;
        
        private void Awake()
        {
            stateAnnouncer = animator.GetBehaviour<GeneralStateAnnouncer>();
            stateAnnouncer.stateEntered += stateEntered;
            stateAnnouncer.stateExited += stateExited;
        }
        
        private void stateEntered()
        {
            Debug.Log("Entered");
        }
        
        private void stateExited()
        {
            Debug.Log("Exited");
        }
    }
}
