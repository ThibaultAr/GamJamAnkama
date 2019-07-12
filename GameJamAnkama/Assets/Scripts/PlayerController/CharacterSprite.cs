using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprite : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 4.5f, 0f);

    private Transform _worldCenter;

    private void Start()
    {
        _worldCenter = GameObject.FindGameObjectWithTag("WorldCenter").transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = _target.position + _offset;
        this.transform.rotation = _worldCenter.rotation;
    }
}
