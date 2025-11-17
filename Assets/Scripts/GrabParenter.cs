using UnityEngine;

public class GrabParenter : MonoBehaviour, IHandRotator
{
    [Header("Right hand offsets (local)")]
    [SerializeField] private Vector3 _rightPositionOffset;
    [SerializeField] private Vector3 _rightRotationOffsetEuler;

    [Header("Left hand offsets (local)")]
    [SerializeField] private Vector3 _leftPositionOffset;
    [SerializeField] private Vector3 _leftRotationOffsetEuler;

    [Header("Trackers")]
    [SerializeField] private GameObject _rightHandTracker;
    [SerializeField] private GameObject _leftHandTracker;

    private class HandState
    {
        public Transform Tracker;        // сам трекер
        public Transform SavedParent;    // исходный родитель
        public Vector3 SavedLocalPos;  // исходна€ локальна€ позици€
        public Quaternion SavedLocalRot; // исходна€ локальна€ ротаци€
        public bool IsActive;
    }

    private readonly HandState _right = new HandState();
    private readonly HandState _left = new HandState();

    // ≈динственный публичный метод
    public void SetHand(Transform hand)
    {
        // 1) hand == null -> отпустить обе руки
        if (hand == null)
        {
            Release(_right);
            Release(_left);
            return;
        }

        // 2) ќпределить, какую руку переключаем
        if (hand.CompareTag("RightHand"))
        {
            ToggleAttach(_right, _rightHandTracker, _rightPositionOffset, _rightRotationOffsetEuler);
        }
        else if (hand.CompareTag("LeftHand"))
        {
            ToggleAttach(_left, _leftHandTracker, _leftPositionOffset, _leftRotationOffsetEuler);
        }
        // иначе Ч неизвестна€ рука, ничего не делаем
    }

    // ===== ¬нутренн€€ логика =====
    private void ToggleAttach(HandState state, GameObject trackerGO, Vector3 posOffset, Vector3 rotOffsetEuler)
    {
        if (trackerGO == null) return;

        if (state.IsActive)
        {
            // уже закреплена Ч отпускаем
            Release(state);
            return;
        }

        // ещЄ не закреплена Ч закрепл€ем
        var tracker = trackerGO.transform;

        state.Tracker = tracker;
        state.SavedParent = tracker.parent;
        state.SavedLocalPos = tracker.localPosition;
        state.SavedLocalRot = tracker.localRotation;

        tracker.SetParent(transform, false); // будем задавать локальные офсеты
        tracker.localPosition = posOffset;
        tracker.localRotation = Quaternion.Euler(rotOffsetEuler);

        state.IsActive = true;
    }

    private void Release(HandState state)
    {
        if (!state.IsActive || state.Tracker == null) return;

        state.Tracker.SetParent(state.SavedParent, false);
        state.Tracker.localPosition = state.SavedLocalPos;
        state.Tracker.localRotation = state.SavedLocalRot;

        state.IsActive = false;
        state.Tracker = null;
        state.SavedParent = null;
    }
}


