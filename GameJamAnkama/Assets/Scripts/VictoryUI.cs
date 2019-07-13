using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryUI : MonoBehaviour
{

    public TMP_Text score;
    public Image crown;
    public Image RyukSprite;
    public Image BNSprite;
    public Image SJSprite;
    public Image EveSprite;

    public void updateUI(int score, PlayerScriptableObject player)
    {
        this.score.text = score.ToString();
        switch (player.characterName)
        {
            case "Ryuk":
                display(true, false, false, false);
                break;
            case "Blanche":
                display(false, true, false, false);
                break;
            case "Eve":
                display(false, false, false, true);
                break;
            case "Steve":
                display(false, false, true, false);
                break;
        }
    }

    public void Crown()
    {
        crown.enabled = true;
    }

    private void display(bool ryuk, bool bn, bool sj, bool eve)
    {
        RyukSprite.enabled = ryuk;
        BNSprite.enabled = bn;
        SJSprite.enabled = sj;
        EveSprite.enabled = eve;
    }
}
