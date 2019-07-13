using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StockSave : MonoBehaviour
{
    public Dictionary<int, int> playerScore = new Dictionary<int, int>();

    private TMP_Text text;

    public void Start()
    {
        PlayerManager pm = FindObjectOfType<PlayerManager>();
        for (int i = 1; i < pm.playerList.Count+1; i++)
        {
            playerScore[i] = 0;
        }
    }

    public void AddApple(GameObject apple, int playerIndex)
    {
        SOUND.WinPoint();
        playerScore[playerIndex] += 1;

        GameObject.Find("P" + playerIndex).GetComponent<CharacterUI>().textScore.text = "x" + playerScore[playerIndex];
        Destroy(apple);
    }
}
