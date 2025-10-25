using Bots;
using UnityEngine;

namespace States 
{ 
    public interface IState
    {
        public void Enter(BotAirplaneControl botControl);
        public void Exit(BotAirplaneControl botControl);
        public void Update(BotAirplaneControl botControl);
    }
    public class Agro : IState
    {
        private Vector3 _inputVector;
        public void Enter(BotAirplaneControl botControl)
        {

        }
        public void Exit(BotAirplaneControl botControl)
        {

        }
        public void Update(BotAirplaneControl botControl)
        {
            if (botControl.CrashAvoidChecker.CheckToAviod()) botControl.SwitchState(StatesList.AvoidCrash);
            botControl.AirplanePhysics.SetThrust(60);
            botControl.FollowToPlayer.Follow(ref _inputVector);
            botControl.AirplanePhysics.SetSteeringInput(_inputVector);
        }
    }
    public class AvoidCrash : IState
    {
        private Vector3 _inputVector;
        private float timer;
        public void Enter(BotAirplaneControl botControl)
        {

        }
        public void Exit(BotAirplaneControl botControl)
        {

        }
        public void Update(BotAirplaneControl botControl)
        {
            if (!botControl.CrashAvoidChecker.CheckToAviod())
            {
                timer += Time.deltaTime;
            }
            else timer = 0;

            if (timer > 1.5f)
            {
                botControl.SwitchState(StatesList.Agro);
            }

            botControl.AirplanePhysics.SetThrust(60);

            Vector3 aircraftRight = botControl.AirplanePhysics.transform.right;
            Vector3 worldUp = Vector3.forward;
            Vector3 horizontalRight = Vector3.ProjectOnPlane(aircraftRight, worldUp).normalized;

            float rollAngle = Mathf.Atan2(horizontalRight.y, horizontalRight.x) * Mathf.Rad2Deg;

            if (Mathf.Abs(rollAngle) > 2)
            {
                _inputVector.x = Mathf.Clamp(rollAngle, -10, 10);
            }
            else _inputVector.x = 0;
            _inputVector.z = -10;

            botControl.AirplanePhysics.SetSteeringInput(_inputVector);
        }
    }
    public enum StatesList
    {
        Agro, AvoidCrash
    }
}


