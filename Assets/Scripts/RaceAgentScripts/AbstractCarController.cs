using UnityEngine;

namespace RaceAgentScripts
{
    public abstract class AbstractCarController : MonoBehaviour
    {
        public abstract void StopCar();
        public abstract void WarpCar(Vector3 pPosition,bool pResetVelocity = true);
        public abstract void SetCarInput(CarInput pCarInput);
        public abstract float GetTurnAngle();
        public abstract Vector3 GetVelocity();
        public abstract Vector3 GetRigidbodyPosition();
    }
}
