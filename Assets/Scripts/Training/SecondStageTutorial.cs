using Interfaces;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SecondStageTutorial : MonoBehaviour, ITutorialBlock
{
    [SerializeField] private MonoBehaviour[] _stagesBlocks;
    private ITutorialStage _currentTutorialStage;
    private int _currentStage = 0;

    /*
     * Добавить массив с объектами, реализующих интерфейс (к примеру)ITutorialStage с методом 
     * CheckProgress(), возвращающий bool значение. Если true, то тут переходим на след объект в массиве _stagesBlock, 
     * и теперь у него проверяем выполнение условия через CheckProgress()
     */
    public void ActivateBlock() { }

    public void NextBlock() { }

    private void OnEnable()
    {
        if (_stagesBlocks.Length > 0)
        {
            _currentTutorialStage = _stagesBlocks[_currentStage] as ITutorialStage;
            if (_currentTutorialStage == null)
            {
                Debug.LogWarning("One of the stages does not implement ITutorialStage");
            }
            else _currentTutorialStage.ActivateStage();
        }
    }

    private void Update()
    {
        if (_currentTutorialStage.CheckProgress())
        {
            _currentTutorialStage.CompleteStage();
            NextTutorialStage();
            _currentTutorialStage.ActivateStage();
        }
    }
    private void NextTutorialStage()
    {
        _currentStage++;
        if (_currentStage < _stagesBlocks.Length)
        {
            _currentTutorialStage = _stagesBlocks[_currentStage] as ITutorialStage;
            if (_currentTutorialStage == null)
            {
                Debug.LogWarning("One of the stages does not implement ITutorialStage");
            }
        }
    }
}
