using Futurift.DataSenders;
using Futurift.Options;
using UnityEngine;

namespace Futurift
{
    public class FuturiftController : MonoBehaviour
    {
        [SerializeField] private string ipAddress = "127.0.0.1";
        [SerializeField] private int port = 6065;

        private FutuRiftController _controller;
        private AirplanePhysics _airplanePhysics;
        private Vector3 _preAngularVector;
        
        private void Awake()
        {
            var udpOptions = new UdpOptions
            {
                ip = ipAddress,
                port = port
            };

            _controller = new FutuRiftController(new UdpPortSender(udpOptions));
        }
        private void Start()
        {
            _airplanePhysics = GetComponent<AirplanePhysics>();
        }

        private void FixedUpdate()
        {
            var euler = _airplanePhysics.LocalAngularVelocity / Time.fixedDeltaTime;

            _controller.Pitch = (euler.x > 180 ? euler.x - 360 : euler.x) / 2;
            _controller.Roll = (euler.z > 180 ? euler.z - 360 : euler.z) / 7;
        }

        private void OnEnable()
        {
            _controller?.Start();
        }

        private void OnDisable()
        {
            _controller?.Stop();
        }
    }
}
