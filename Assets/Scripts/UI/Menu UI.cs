using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _startMenuLight;
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _settingsPannel;
    [SerializeField] private GameObject _pilotLanding;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _planeModel;
    [SerializeField] private Scrollbar _xScrollbar;
    [SerializeField] private Scrollbar _yScrollbar;
    [SerializeField] private Scrollbar _zScrollbar;
    [SerializeField] private SettingsData _settingsData;
    private Vector3 _playerStartPos;

    private void Start()
    {
        _playerStartPos = _player.transform.localPosition;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
        SceneManager.UnloadSceneAsync("Menu");
    }
    public void OpenSettings()
    {
        _startPanel.SetActive(false);
        _settingsPannel.SetActive(true);
    }
    public void CloseSettings()
    {
        _startPanel.SetActive(true);
        _settingsPannel.SetActive(false);
    }
    public void OpenPilotLandingSettings()
    {
        _startMenuLight.SetActive(false);
        _pilotLanding.SetActive(true);

        _player.transform.SetParent(_planeModel.transform);

        Vector3 targetPos = new Vector3(0, _settingsData.PilotY, _settingsData.PilotZ);
        StartCoroutine(MovePlayerSmoothly(targetPos, 0.4f));

        _xScrollbar.value = Mathf.InverseLerp(-0.3f, 0.3f, _settingsData.PilotX);
        _yScrollbar.value = Mathf.InverseLerp(-1f, 0f, _settingsData.PilotY);
        _zScrollbar.value = Mathf.InverseLerp(0.2f, 0.8f, _settingsData.PilotZ);
    }
    public void SavePilotLanding()
    {
        float x = Mathf.Lerp(-0.3f, 0.3f, _xScrollbar.value);
        float y = Mathf.Lerp(-1f, 0f, _yScrollbar.value);
        float z = Mathf.Lerp(0.2f, 0.8f, _zScrollbar.value);
        _settingsData.SetPilotLanding(x, y, z);

        _startMenuLight.SetActive(true);
        _pilotLanding.SetActive(false);

        _player.transform.SetParent(null);

        _player.transform.position = _playerStartPos;
    }
    public void SetPilotPositionAccordingToScrollbars()
    {
        float x = Mathf.Lerp(-0.3f, 0.3f, _xScrollbar.value);
        float y = Mathf.Lerp(-1f, 0f, _yScrollbar.value);
        float z = Mathf.Lerp(0.2f, 0.8f, _zScrollbar.value);

        _player.transform.localPosition = new Vector3(x, y, z);
    }

    private IEnumerator MovePlayerSmoothly(Vector3 target, float duration)
    {
        Vector3 start = _player.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            _player.transform.localPosition = Vector3.Lerp(start, target, t);

            yield return null; 
        }

        _player.transform.localPosition = target; 
    }
}
