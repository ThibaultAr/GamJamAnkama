using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject meteorite;
    public GameObject meteoriteImpact;
    public Canvas canvas;

    public TMP_Text timerText;

    public int delayMeteorite;
    public int varTimeMeteorite;

    public int timer;

    private PlayerController[] _players;
    private int _nextMeteorite;
    private int _seconds;

    public bool gameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        SOUND.StartAmbiance();
        _players = FindObjectsOfType<PlayerController>();
        _seconds = 0;
        InvokeRepeating("AddSecond", 1f, 1f);
        _nextMeteorite = delayMeteorite + Random.Range(-1 * varTimeMeteorite, varTimeMeteorite);
    }

    private void AddSecond()
    {
        _seconds += 1;
        if (_nextMeteorite == _seconds)
        {
            SpawnMeteorite();
            _nextMeteorite += delayMeteorite + Random.Range(-1 * varTimeMeteorite, varTimeMeteorite);
        }

        if (timer == _seconds)
        {
            Win();
        }

        /// Update Timer
        string minutes = Mathf.Floor((timer - _seconds) / 60).ToString("00");
        string seconds = Mathf.RoundToInt((timer - _seconds) % 60).ToString("00");
        timerText.text = string.Format("{0}:{1}", minutes, seconds);
    }

    private void SpawnMeteorite()
    {
        PlayerController victim = _players[Random.Range(0, _players.Length - 1)];
        Vector3 v = victim.transform.position;
        GameObject met = Instantiate(meteorite);
        met.transform.position = v + new Vector3(0, 100, 0);
        GameObject metImpact = Instantiate(meteoriteImpact);
        metImpact.transform.position = v + new Vector3(0, -1*v.y, 0);
    }

    public void Win()
    {
        canvas.enabled = true;
        gameOver = true;
        StockSave ss = FindObjectOfType<StockSave>();
        int bestPlayer = 1;
        int actualBestScore = 0;
        PlayerManager pm = FindObjectOfType<PlayerManager>();
        foreach(KeyValuePair<int, int> kvp in ss.playerScore)
        {
            if (kvp.Value > actualBestScore)
            {
                bestPlayer = kvp.Key;
                actualBestScore = kvp.Value;
            }
            GameObject.Find("PV"+kvp.Key).GetComponent<VictoryUI>().updateUI(kvp.Value, pm.playerList[kvp.Key]);
        }

        GameObject.Find("PV" + bestPlayer).GetComponent<VictoryUI>().Crown();

        for (int i = 0; i < 4 - pm.playerList.Count; i++)
        {
            GameObject.Find("PV" + (4 - i).ToString()).gameObject.SetActive(false);
        }
    }
}
