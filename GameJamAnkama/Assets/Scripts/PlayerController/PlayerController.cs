using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed               = 1.0f;
    [SerializeField] private float _dashIntensity       = 1.0f;
    [SerializeField] private float _dashSpeed           = 3.0f;
    [SerializeField] private float _speedBoostFactor    = 2.0f;
    [SerializeField] private float _speedBoostDuration  = 2.0f;

    private Animator    _animator;
    private bool        _isBoosted  = false;
    private bool        _dashing    = false;

    private Vector3     _wantedPosition;
    private Vector3     _startPosition;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    ///////////////////////////////////
    ///// Move Section            /////
    ///////////////////////////////////
    public void Move(float angle, bool move = false)
    {
        if (!move)
            return;
        if (!_dashing)
            this.transform.rotation = Quaternion.Euler(0, angle, 0);
        this.transform.Translate(this.transform.forward * (_dashing ? _dashSpeed : _speed));

        //Set le bon angle d'animation
        //De face si idle
    }

    ///////////////////////////////////
    ///// Speed boost Section     /////
    ///////////////////////////////////
    public void Eat()
    {
        _isBoosted = true;
        SpeedBoost();
    }

    private void SpeedBoost()
    {
        _speed *= _speedBoostFactor;
    }

    private void UnSpeedBoost()
    {
        _speed /= _speedBoostFactor;
    }

    ///////////////////////////////////
    ///// Dash boost Section      /////
    ///////////////////////////////////
    public void Dash()
    {
        _dashing = true;
        _startPosition = this.transform.position;
        Invoke("ResetDash", _dashIntensity); 
    }

    private void ResetDash()
    {
        _dashing = false;
    }
}
