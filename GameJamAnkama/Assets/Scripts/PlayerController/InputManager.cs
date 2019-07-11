using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    protected PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        this._playerController = GetComponent<PlayerController>();       
    }

    // Update is called once per frame
    void Update()
    {
        //Appeler player Move en fonction du stick
        //Appeler player Idle si il faut
        //Appeler player Eat si boutton appuyé
        //Appeler player Dash si boutton appuyé
    }
}
