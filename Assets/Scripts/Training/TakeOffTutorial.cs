using Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class TakeOffTutorial : MonoBehaviour, ITutorialStage
{
    [SerializeField] private GameObject _airplane;
    [SerializeField] private Outline _outline;

    public void ActivateStage()
    {
        if (_airplane == null) Debug.LogError("Airplane reference is missing in TakeOffTutorial");   
        gameObject.SetActive(true);
        _outline.enabled = true;
    }

    public bool CheckProgress()
    {
        if (_airplane.transform.position.y > 15f)
        {
            Debug.Log("TakeOffTutorial completed");
            return true;
        }
        else return false;
    }

    public void CompleteStage()
    {
        Debug.Log("TakeOffTutorial CompleteStage called");
        _outline.enabled = false;
        gameObject.SetActive(false);
    }
}
