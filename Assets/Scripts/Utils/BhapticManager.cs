using Bhaptics.SDK2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class BhapticManager
    {
        CoroutineStarter _coroutineStarter;
        Dictionary<string, float> _eventsDuration = new Dictionary<string, float>();
        HashSet<string> _playingEvents = new HashSet<string>();
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
            Debug.Log($"Start event: {eventName}");

            _playingEvents.Add(eventName);

            yield return new WaitForSeconds(_eventsDuration[eventName]);

            Debug.Log($"End event: {eventName}");
            _playingEvents.Remove(eventName);
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