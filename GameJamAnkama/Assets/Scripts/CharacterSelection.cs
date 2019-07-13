using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public int id;
    public PlayerScriptableObject playerObject;
    public int index;
    public List<CharacterScriptableObject> characterList;
    public PlayerManager playerManager;

    public CharacterScriptableObject currentCharacter;

    public GameObject name;
    public GameObject AButtonUI;
    public GameObject readyUI;
    public Image avatar;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.FindObjectOfType<PlayerManager>();
        index = id-1;
        playerObject.index = id;
        currentCharacter = characterList[index];
        playerManager.charactersList[index] = this;
        UpdateUI();
    }

    public bool Validate(bool select)
    {
        if (select && playerManager.selectedCharacters.Contains(currentCharacter.characterName))
            return false;

        AButtonUI.SetActive(!select);
        readyUI.SetActive(select);

        if (select)
        {
            playerObject.sprite = currentCharacter.sprite;
            playerObject.hitSprite = currentCharacter.hitSprite;
            playerObject.avatarSprite = currentCharacter.avatarSprite;
            playerObject.characterName = currentCharacter.characterName;
            playerManager.selectedCharacters.Add(currentCharacter.characterName);
            playerManager.playerList.Add(playerObject.index, playerObject);

        }
        else
        {
            playerManager.selectedCharacters.Remove(currentCharacter.characterName);
            int tmpKey = -1;
            foreach(KeyValuePair<int, PlayerScriptableObject> kvp in playerManager.playerList)
            {
                if (kvp.Value == playerObject)
                    tmpKey = kvp.Key;
            }
            if(tmpKey != -1)
                playerManager.playerList.Remove(tmpKey);
        }

        return true;
    }

    private void UpdateUI()
    {
        name.GetComponent<TMP_Text>().text = currentCharacter.characterName;
        avatar.sprite = currentCharacter.avatarSprite;
    }

    public void NextCharacter()
    {
        index++;
        if (index > 3)
            index = 0;
        currentCharacter = characterList[index];
        UpdateUI();
    }

    public void PreviousCharacter()
    {
        index--;
        if (index < 0)
            index = 3;
       
        currentCharacter = characterList[index];
        UpdateUI();
    }
}
