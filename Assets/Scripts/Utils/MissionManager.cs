using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPositions;
    [SerializeField] private GameObject _botExample;
    [SerializeField] private GameObject _winCanvas;
    private List<GameObject> _bots = new List<GameObject>();
    private void Awake()
    {
        SpawnBots();
    }
    public void SpawnBots()
    {
        for (int i = 0; i < _spawnPositions.Length; i++)
        {
            var bot = Instantiate(_botExample, _spawnPositions[i].position, Quaternion.Euler(90, 0, 0));
            bot.name = "bot" + i.ToString();
            bot.GetComponent<BotCrashChecker>().OnCrash += RemoveBot;
            _bots.Add(bot);
        }
    }
    public void RemoveBot(string botName)
    {
        _bots.RemoveAll(e => e.name == botName);
    }
    private void Update()
    {
        if (_bots.Count == 0 && !_winCanvas.activeInHierarchy) StartCoroutine(ActivateWinCanvas());

    }
    private IEnumerator ActivateWinCanvas()
    {
        _winCanvas.SetActive(true);
        yield return new WaitForSeconds(4f);
        _winCanvas.SetActive(false);
    }
}
