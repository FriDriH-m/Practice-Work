using Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChain : MonoBehaviour, ITutorialBlock
{
    [SerializeField] private GameObject nextBlock;
    [SerializeField] private List<Outline> outlinesToEnable;
    private ITutorialBlock nextTutorialBlock;

    private void Start()
    {
        if (nextBlock.TryGetComponent<ITutorialBlock>(out var tutorialBlock))
        {
            nextTutorialBlock = tutorialBlock;
        } else Debug.LogError("Next tutorial block does not implement ITutorialBlock interface");
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
        if (nextTutorialBlock != null)
        {
            nextTutorialBlock.ActivateBlock();
        }
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
