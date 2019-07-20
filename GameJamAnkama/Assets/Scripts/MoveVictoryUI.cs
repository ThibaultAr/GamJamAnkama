using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVictoryUI : MonoBehaviour
{
    [SerializeField] private float[] twoPlayersPosition = { -82.5f, 82.5f };
    [SerializeField] private float[] threePlayersPosition = { -165f, 0f, 165f };
    [SerializeField] private float[] fourPlayersPosition = { -247.5f, -82.5f, 82.5f, 247.5f };

    [Header("UI Player slot")]
    [SerializeField] private GameObject firstPlayerUIScore;
    [SerializeField] private GameObject secondPlayerUIScore;
    [SerializeField] private GameObject thirdPlayerUIScore;
    [SerializeField] private GameObject fourthPlayerUIScore;

    private PlayerManager _playerManager;

    // Start is called before the first frame update
    void Start()
    {
        _playerManager = FindObjectOfType<PlayerManager>();
        MovePlayerScoreUI();
    }

    private void MovePlayerScoreUI()
    {

        if ((!_playerManager.debugMode ? _playerManager.nbOfPlayers : _playerManager.playerDebugNumber) == 2)
        {
            firstPlayerUIScore.GetComponent<RectTransform>().anchoredPosition = new Vector3(twoPlayersPosition[0], 0f, 0f);
            secondPlayerUIScore.GetComponent<RectTransform>().anchoredPosition = new Vector3(twoPlayersPosition[1], 0f, 0f);
            thirdPlayerUIScore.GetComponent<CanvasGroup>().alpha = 0;
            fourthPlayerUIScore.GetComponent<CanvasGroup>().alpha = 0;
        }
        else if((!_playerManager.debugMode ? _playerManager.nbOfPlayers : _playerManager.playerDebugNumber) == 3)
        {
            firstPlayerUIScore.GetComponent<RectTransform>().anchoredPosition = new Vector3(threePlayersPosition[0], 0f, 0f);
            secondPlayerUIScore.GetComponent<RectTransform>().anchoredPosition = new Vector3(threePlayersPosition[1], 0f, 0f);
            thirdPlayerUIScore.GetComponent<RectTransform>().anchoredPosition = new Vector3(threePlayersPosition[2], 0f, 0f);
            fourthPlayerUIScore.GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            firstPlayerUIScore.GetComponent<RectTransform>().anchoredPosition = new Vector3(fourPlayersPosition[0], 0f, 0f);
            secondPlayerUIScore.GetComponent<RectTransform>().anchoredPosition = new Vector3(fourPlayersPosition[1], 0f, 0f);
            thirdPlayerUIScore.GetComponent<RectTransform>().anchoredPosition = new Vector3(fourPlayersPosition[2], 0f, 0f);
            fourthPlayerUIScore.GetComponent<RectTransform>().anchoredPosition = new Vector3(fourPlayersPosition[3], 0f, 0f);
        }
    }
}
