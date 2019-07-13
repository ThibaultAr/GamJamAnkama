using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterScriptableObject", order = 1)]
public class CharacterScriptableObject : ScriptableObject
{
    public GameObject sprite;
    public Sprite hitSprite;
    public Sprite avatarSprite;

    public string characterName;
}
