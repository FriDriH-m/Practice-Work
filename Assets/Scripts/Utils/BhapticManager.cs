using Bhaptics.SDK2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class BhapticManager
    {
        private CoroutineStarter _coroutineStarter;
        private readonly Dictionary<int, float> _activeMotors = new Dictionary<int, float>(); // Key: motor Type, Value: duration
        private readonly Dictionary<string, float> _eventsDuration = new Dictionary<string, float>();
        private readonly HashSet<string> _playingEvents = new HashSet<string>();
        public void Initialize()
        {
            _eventsDuration.Add(BhapticsEvent.AIRPLANETAKEOFF, 1);
            _eventsDuration.Add(BhapticsEvent.GETDAMAGE, 0.5f);
            _eventsDuration.Add(BhapticsEvent.PLANECRASH, 1.5f);
            _eventsDuration.Add(BhapticsEvent.ENGINESTOP, 1);
            _eventsDuration.Add(BhapticsEvent.ENGINESTART, 1); 
            _eventsDuration.Add(BhapticsEvent.PLANEFIRELEFTHAND, 0.26f);
            _eventsDuration.Add(BhapticsEvent.PLANEFIRERIGHTHAND, 0.26f);

            _coroutineStarter = DIContainer.Instance.Get<CoroutineStarter>();
        }
        public void RequestStartEvent(string eventName)
        {
            if (_playingEvents.Contains(eventName)) return;
            _coroutineStarter.StartCoroutine(StartEvent(eventName));
        }
        private IEnumerator StartEvent(string eventName)
        {
            BhapticsLibrary.Play(eventId: eventName, intensity: 1, duration: 1);

            _playingEvents.Add(eventName);

            yield return new WaitForSeconds(_eventsDuration[eventName]);

            //Debug.Log($"End event: {eventName}");
            BhapticsLibrary.StopByEventId(eventName);
            _playingEvents.Remove(eventName);
        }
        public void ActiveMotor(PositionType type, int millis, int[] array)
        {
            //foreach (KeyValuePair<int, float> pair in _activeMotors)
            //{
            //    if (pair.Value <= 0)
            //    {
            //        _activeMotors.Remove(pair.Key);
            //    } 
            //    _activeMotors[pair.Key] -= Time.deltaTime * 1000; 
            //}
            if (_playingEvents.Count > 0) return;

            BhapticsLibrary.PlayMotors((int)type, motors: array, durationMillis: millis);
            //_activeMotors.Add((int)type, millis);
        }
        public void Debugging()
        {
            string a = "";
            foreach (var e in _playingEvents)
            {
                a += e + " ";
            }
            Debug.Log($"[BhapticManager] Playing events: {a}");
        }
    }
}