using UnityEngine;
using Interfaces;
using UnityEngine.SceneManagement;

public class EndTutorial : MonoBehaviour, ITutorialStage
{
    private float _timer = 0;
    public void ActivateStage()
    {
        gameObject.SetActive(true);
    }

    public bool CheckProgress()
    {
        _timer += Time.deltaTime;
        if (_timer > 5)
        {
            SceneManager.LoadScene("SampleScene");
        }
        return false;
    }

    public void CompleteStage()
    {
        throw new System.NotImplementedException();
    }
}
