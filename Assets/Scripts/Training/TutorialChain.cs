using Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChain : MonoBehaviour, ITutorialBlock
{
    [SerializeField] private GameObject nextBlock;
    [SerializeField] private List<Outline> outlinesToEnable;
    private ITutorialBlock _nextTutorialBlock;

    private void InitNextBlock()
    {
        if (TryGetComponent<ITutorialBlock>(out var tutorialBlock))
        {
           _nextTutorialBlock = tutorialBlock;
        }
        if (_nextTutorialBlock == null)
        {
            Debug.LogWarning("Next Tutorial Block is not set or does not implement ITutorialBlock");
        }
    }
    public void ActivateBlock()
    {
        if (outlinesToEnable.Count > 0)
        {
            foreach(Outline outline in outlinesToEnable)
            {
                outline.enabled = true;
            }
        }
        gameObject.SetActive(true);
    }
    public void NextBlock()
    {
        InitNextBlock();
        if (_nextTutorialBlock != null)
        {
            _nextTutorialBlock.ActivateBlock();
            Debug.Log("Next Tutorial Block Activated");
        } else Debug.Log("Next Tutorial Block No");
        if (outlinesToEnable.Count > 0)
        {
            foreach (Outline outline in outlinesToEnable)
            {
                outline.enabled = false;
            }
        }
        gameObject.SetActive(false);
    }
}
