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

            SwitchState(StatesList.AvoidCrash);
        }

        private void Update()
        {            
            _currentState.Update(this);
            //_airplanePhysics.SetSteeringInput(new Vector3(10, 0, 5));
            //_avoidCrash.CheckToAviod();
        }
        public void SwitchState(StatesList newState)
        {
            if (_currentState != null)
            {
                _currentState.Exit(this);
            }
            _currentState = _states[newState];
            _currentState.Enter(this);
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
            Debug.Log(_velocity.magnitude * 5);
            if (Physics.Raycast(_airplanePhysics.transform.position, _velocity.normalized, out hit, _velocity.magnitude * 3))
            {
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
        public FollowToPlayer(Transform player, Transform bot, BotGunsControl guns)
        {
            _guns = guns;
            _player = player;
            _bot = bot;
        }

        public void Follow(ref Vector3 inputVector)
        {
            Vector3 directionToPlayer = _player.transform.position - _bot.transform.position;
            Vector3 botForward = _bot.transform.forward;
            Vector3 localDirection = _bot.transform.InverseTransformDirection(directionToPlayer);

            float tanX = Mathf.Atan2(-localDirection.y, localDirection.z) * Mathf.Rad2Deg;
            float tanZ = Mathf.Atan2(localDirection.x, localDirection.z) * Mathf.Rad2Deg;

            tanX = (tanX > 180f) ? tanX - 360f : tanX;
            tanZ = (tanZ > 180f) ? tanZ - 360f : tanZ;

            if (Mathf.Abs(tanX) > 0f)
            {
                inputVector.z = Mathf.Clamp(tanX * 5, -10f, 10f);
            }
            if (Mathf.Abs(tanX) < 0.5f) _guns.Shoot();

            if (Mathf.Abs(tanZ) > 3f)
            {
                inputVector.x = Mathf.Clamp(tanZ, -10f, 10f);
            }
            else
            {
                inputVector.x = 0;
            }

            Debug.Log($"tanX: {tanX:F1}° tanZ: {tanZ:F1}° → input: {inputVector}");         
        }
    }
}

