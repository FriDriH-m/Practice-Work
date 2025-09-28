using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _startMenuLight;
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _settingsPannel;
    [SerializeField] private GameObject _pilotLanding;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _planeModel;
    [SerializeField] private SettingsData _settingsData;
    
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
        _player.transform.localPosition = new Vector3(0, _settingsData.PilotHeight, _settingsData.PilotWidth);

    }
}
