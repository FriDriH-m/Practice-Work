using System.Collections;
using TMPro;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using Utils;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _speed;
    [SerializeField] private TextMeshProUGUI _gForce;
    [SerializeField] private TextMeshProUGUI _height;
    [SerializeField] private TextMeshProUGUI _ammo;
    [SerializeField] private TextMeshProUGUI _tops;
    [SerializeField] private TextMeshProUGUI _targets;
    [SerializeField] private GameObject _winnerUI;
    private AirplanePhysics _airplanePhysics;
    private GunsShootingSystem _gunShootingSystem;
    private MatchManager _matchManager;
    private float _planeSpeed;
    private float _timer;

    private void Start()
    {
        _winnerUI.SetActive(false);
        _airplanePhysics = DIContainer.Instance.Get<AirplanePhysics>("Player_Plane");
        _gunShootingSystem = DIContainer.Instance.Get<GunsShootingSystem>("Player_Bullets");
        _matchManager = DIContainer.Instance.Get<MatchManager>();
        _matchManager.PlayerWon += ActiveWinnerUI;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 0.2f)
        {
            UpdateUI();
            _timer = 0;
        }        
    }
    private void UpdateUI()
    {
        if (_airplanePhysics == null)
        {
            Debug.LogWarning("UI: _airpanePhysics is null");
            return;
        }
        _planeSpeed = _airplanePhysics.LocalVelocity.magnitude * 3.6f;
        _planeSpeed = (int)_planeSpeed;
        float gForce = Mathf.Clamp(float.Parse(_airplanePhysics.LocalGForce.magnitude.ToString("F1")), -5f, 20f);
        _speed.text = "Speed: " + _planeSpeed;
        _gForce.text = "G-force: " + gForce;
        _height.text = "Height: " + (int)transform.parent.position.y;
        if (_gunShootingSystem.IsReloading)
        {
            _ammo.color = Color.red;
            _ammo.text = "Ammo: " + $"({(int)_gunShootingSystem.ReloadTimer})";
        }
        else
        {
            _ammo.color = Color.white;
            _ammo.text = "Ammo: " + _gunShootingSystem.Ammo.Count;
        }
        _tops.text = "tops: 3/" + _matchManager.AffectedTops;
        _targets.text = "targets: 3/" + _matchManager.AffectedTargets;
    }
    public void ActiveWinnerUI()
    {
        _winnerUI.SetActive(true);
        StartCoroutine(WinnerUILifeTime());
    }
    private IEnumerator WinnerUILifeTime()
    {
        yield return new WaitForSeconds(3);

        _winnerUI.SetActive(false);
    }
    public void SetPlaneSpeed(float speed)
    {
        _planeSpeed = speed;
    }
}
