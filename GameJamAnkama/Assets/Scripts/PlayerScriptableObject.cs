using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/PlayerScriptableObject", order = 2)]
public class PlayerScriptableObject : ScriptableObject
{
    public int index;
    public GameObject sprite;
    public Sprite hitSprite;
    public Sprite avatarSprite;
    public string characterName;
}
