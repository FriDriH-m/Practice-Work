using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _settingsPannel;

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("SampleScene");
        SceneManager.UnloadSceneAsync("Menu");
        DIContainer.Instance.Get<AudioManager>().Stop();
    }
    public void StartTutorial()
    {
        SceneManager.LoadSceneAsync("TrainingScene");
        SceneManager.UnloadSceneAsync("Menu");
        DIContainer.Instance.Get<AudioManager>().Stop();
    }
    public void Exit()
    {
        Application.Quit();
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
}
