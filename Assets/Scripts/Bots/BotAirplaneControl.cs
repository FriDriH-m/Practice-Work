using UnityEngine;
using Utils;

namespace Bots
{
    public class BotAirplaneControl : MonoBehaviour
    {
        [SerializeField] private BotGunsControl gunsControl;
        private AirplanePhysics _airplanePhysics;
        private AvoidCrash _avoidCrash;
        private FollowToPlayer _followToPlayer;
        private Transform _player;
        private Vector3 _inputVector;

        private void Start()
        {
            _player = DIContainer.Instance.Get<Transform>("Player");
            
            _airplanePhysics = GetComponent<AirplanePhysics>();

            _followToPlayer = new FollowToPlayer(_player, transform, gunsControl);
            _avoidCrash = new AvoidCrash(_airplanePhysics);
        }

        private void Update()
        {
            _airplanePhysics.SetThrust(60);
            _followToPlayer.Follow(ref _inputVector);
            _airplanePhysics.SetSteeringInput(_inputVector);
            //_airplanePhysics.SetSteeringInput(new Vector3(10, 0, 5));
            //_avoidCrash.CheckToAviod();
        }
    }
    public class AvoidCrash
    {
        private AirplanePhysics _airplanePhysics;
        private Vector3 _velocity;
        public AvoidCrash(AirplanePhysics airplanePhysics)
        {
            _airplanePhysics = airplanePhysics;
        }

        public void CheckToAviod(Vector3 inputVector)
        {
            RaycastHit hit;
            _velocity = _airplanePhysics.transform.TransformDirection(_airplanePhysics.LocalVelocity);

            if (Physics.Raycast(_airplanePhysics.transform.position, _velocity.normalized, out hit, _velocity.magnitude * 15))
            {
                inputVector.z = -10;
                _airplanePhysics.SetSteeringInput(new Vector3(0, 0, -10));
            }
            else
            {
                _airplanePhysics.SetSteeringInput(new Vector3(0, 0, 0));
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

            if (Mathf.Abs(tanZ) > 0f)
            {
                inputVector.x = Mathf.Clamp(tanZ, -10f, 10f);
            }
            else
            {
                inputVector.x = 0;
            }
            

            Debug.Log($"tanX: {tanX:F1}° tanZ: {tanZ:F1}° → input: {inputVector}");

            //var projectX = Vector3.ProjectOnPlane(directionToPlayer, _bot.transform.forward);
            //var projectY = Vector3.ProjectOnPlane(directionToPlayer, _bot.transform.right);
            //var projectZ = Vector3.ProjectOnPlane(directionToPlayer, _bot.transform.up);

            // Углы для каждой проекции
            //float angleX = Vector3.SignedAngle(_bot.transform.right, projectX, _bot.transform.forward);
            //float angleY = Vector3.SignedAngle(_bot.transform.up, projectY, _bot.transform.right);
            //float angleZ = Vector3.SignedAngle(_bot.transform.forward, projectZ, _bot.transform.up);

            // Приведение углов к диапазону [-180, 180]
            //float signedX = (angleX > 180f) ? angleX - 360f : angleX;
            //float signedY = (angleY > 180f) ? angleY - 360f : angleY;
            //float signedZ = (angleZ > 180f) ? angleZ - 360f : angleZ;

            //Debug.Log($"Signed X: {signedX}° | Signed Y: {signedY}° | Signed Z: {signedZ}°");            
        }
    }
}

