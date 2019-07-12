using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprite : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 4.5f, 0f);

    private Transform _worldCenter;
    private GameObject _spriteGO;

    private void Start()
    {
        _target.GetComponent<PlayerController>().spriteGO = this.gameObject;
        _spriteGO = GetComponentInChildren<SpriteRenderer>().gameObject;
        _worldCenter = GameObject.FindGameObjectWithTag("WorldCenter").transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = _target.position + _offset;
        _spriteGO.transform.rotation = _worldCenter.rotation;
    }
}
