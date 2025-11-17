using States;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Bots
{
    public class BotAirplaneControl : MonoBehaviour
    {
        [SerializeField] private BotGunsControl gunsControl;
        private Vector3 _inputVector;
        private Transform _player;        
        private AirplanePhysics _airplanePhysics;
        private CrashAvoidChecker _avoidCrash;
        private FollowToPlayer _followToPlayer;
        private IState _currentState;
        private Dictionary<StatesList, IState> _states;
        public CrashAvoidChecker CrashAvoidChecker => _avoidCrash;
        public FollowToPlayer FollowToPlayer => _followToPlayer;
        public AirplanePhysics AirplanePhysics => _airplanePhysics;
        public Vector3 InputVector => _inputVector;

        private void Start()
        {
            _states = new Dictionary<StatesList, IState>()
            {
                {StatesList.Agro, new Agro() },
                {StatesList.AvoidCrash, new AvoidCrash() }
            };
            _player = DIContainer.Instance.Get<Transform>("Player");
            
            _airplanePhysics = GetComponent<AirplanePhysics>();

            _followToPlayer = new FollowToPlayer(_player, transform, gunsControl);
            _avoidCrash = new CrashAvoidChecker(_airplanePhysics);

            SwitchState(StatesList.Agro);
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(FollowToPlayer.predictPos, 1f);
        }

        private void Update()
        {            
            _currentState.Update(this);
        }
        public void SwitchState(StatesList newState)
        {
            if (_currentState != null)
            {
                _currentState.Exit(this);
            }
            _currentState = _states[newState];
            _currentState.Enter(this);
            Debug.ClearDeveloperConsole();
        }
    }
    public class CrashAvoidChecker
    {
        private AirplanePhysics _airplanePhysics;
        private Vector3 _velocity;

        public CrashAvoidChecker(AirplanePhysics airplanePhysics)
        {
            _airplanePhysics = airplanePhysics;
        }

        public bool CheckToAviod()
        {
            RaycastHit hit;
            _velocity = _airplanePhysics.transform.TransformDirection(_airplanePhysics.LocalVelocity);
            if (Physics.Raycast(_airplanePhysics.transform.position, _velocity.normalized, out hit, _velocity.magnitude * 3))
            {
                if (hit.collider.tag == "Plane")
                {
                    return false;
                }
                return true;                
            }
            else
            {
                return false;
            }
        }
    }
    public class FollowToPlayer
    {
        private Transform _player;
        private Transform _bot;
        private BotGunsControl _guns;
        private AirplanePhysics _airplanePhysics;
        public Vector3 predictPos = Vector3.zero;
        public FollowToPlayer(Transform player, Transform bot, BotGunsControl guns)
        {
            _guns = guns;
            _player = player;
            _bot = bot;
            _airplanePhysics = _player.GetComponent<AirplanePhysics>();
        }
        public void Follow(ref Vector3 inputVector)
        {
            float time = Vector3.Distance(_bot.position, _player.position) / (_guns.GetBulletSpeed() + 100);

            Vector3 playerPredictPosition = _player.position + _player.transform.forward * _airplanePhysics.LocalVelocity.z * time;

            predictPos = playerPredictPosition;

            Vector3 directionToPlayer = playerPredictPosition - _bot.position;
            Vector3 localDirection = _bot.transform.InverseTransformDirection(directionToPlayer);

            float tanX = Mathf.Atan2(-localDirection.y, localDirection.z) * Mathf.Rad2Deg;
            float tanZ = Mathf.Atan2(localDirection.x, localDirection.z) * Mathf.Rad2Deg;

            tanX = (tanX > 180f) ? tanX - 360f : tanX;
            tanZ = (tanZ > 180f) ? tanZ - 360f : tanZ;

            float targetPitch = 0f;
            float targetRoll = 0f;
            float targetYaw = 0f;

            if (Mathf.Abs(tanX) > 0.01f) targetPitch = Mathf.Clamp(tanX * 2, -10f, 10f);

            if (Mathf.Abs(tanZ) > 1f) targetRoll = Mathf.Clamp(tanZ * 3, -10f, 10f);
            else if (Mathf.Abs(tanZ) > 0.01f)
            {
                targetYaw = tanZ/3;
                targetRoll = tanZ;
            }
            else
            {
                targetYaw = 0f;
                targetRoll = 0f;
            }
            Vector3 _smoothedInput = Vector3.zero;

            _smoothedInput.z = targetPitch;
            _smoothedInput.y = targetYaw;
            _smoothedInput.x = targetRoll;

            inputVector = _smoothedInput;

            bool inCrosshair = Mathf.Abs(tanX) < 0.5f && Mathf.Abs(tanZ) < 0.5f;  
            if (inCrosshair && time < 6f)
            {
                _guns.Shoot();
            }

            //Debug.Log($"tanX: {tanX:F1}° tanZ: {tanZ:F1}° time: {time}");
        }
    }
}

