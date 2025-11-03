using Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public class TakeOffTutorial : MonoBehaviour, ITutorialBlock
{
    [SerializeField] private AirplanePhysics _airplanePhysics;
    [SerializeField] private GameObject _nextTutorial;
    [SerializeField] private GameObject[] _stagesBlocks;
    private ITutorialBlock _nextBlock;
    private int _currentStage = 0;

    /*
     * Добавить массив с объектами, реализующих интерфейс (к примеру)ITutorialStage с методом 
     * CheckProgress(), возвращающий bool значение. Если true, то тут переходим на след объект в массиве _stagesBlock, 
     * и теперь у него проверяем выполнение условия через CheckProgress()
     */
    public void ActivateBlock() { }

    public void NextBlock() { }

    private void Start()
    {
        if (_nextTutorial.TryGetComponent<ITutorialBlock>(out var tutorialBlock))
        {
            _nextBlock = tutorialBlock;
        }
        else Debug.LogWarning("Не найден ITutotialBlock");
    }

    private void Update()
    {
        CheckToTakeOff();
    }
    private void CheckToTakeOff()
    {
        if (_airplanePhysics.transform.position.y > 8)
        {
            _nextBlock.ActivateBlock();
            gameObject.SetActive(false);
        }
    }
}
