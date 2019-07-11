using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed                   = 1.0f;
    [SerializeField] private float _dashDuration            = 0.2f;
    [SerializeField] private float _dashSpeed               = 3.0f;
    [SerializeField] private float _dashCost                = 15.0f;
    [SerializeField] private float _dashJaugeMax            = 30f;
    [SerializeField] private float _dashJaugeRecoverDelay   = 15f;
    [SerializeField] private float _dashRecoverAmount       = 1.0f;
    [SerializeField] private float _speedBoostFactor        = 2.0f;
    [SerializeField] private float _speedBoostDuration      = 2.0f;
    [SerializeField] private float _speedAppleCost          = 1.0f;

    private Animator    _animator;
    private bool        _isBoosted  = false;
    private bool        _dashing    = false;

    private Vector3     _wantedPosition;

    private float _currentDashJauge = 30f;
  

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_currentDashJauge < _dashJaugeMax)
            DashRecover();
    }

    ///////////////////////////////////
    ///// Move Section            /////
    ///////////////////////////////////
    public void Move(float angle, bool move = false)
    {
        if (!move)
            return;
        if (!_dashing)
            this.transform.rotation = Quaternion.Euler(0, angle-45, 0);

        this.transform.position += transform.forward * Time.deltaTime * (_dashing ? _dashSpeed : _speed);

        //Set le bon angle d'animation
        //De face si idle
    }

    ///////////////////////////////////
    ///// Speed boost Section     /////
    ///////////////////////////////////
    public void Eat()
    {
        if (_isBoosted)
            return;

        _isBoosted = true;
        SpeedBoost();
        ConsumeApple();
    }

    private void SpeedBoost()
    {
        _speed *= _speedBoostFactor;
        Invoke("UnSpeedBoost", _speedBoostDuration);
    }

    private void UnSpeedBoost()
    {
        _isBoosted = false;
        _speed /= _speedBoostFactor;
    }

    private void ConsumeApple()
    {
        
    }

    ///////////////////////////////////
    ///// Dash boost Section      /////
    ///////////////////////////////////
    public void Dash()
    {
        if (_dashing || _currentDashJauge < _dashCost)
            return;

        _dashing = true;
        _currentDashJauge -= _dashCost;
        Invoke("ResetDash", _dashDuration); 
    }

    private void ResetDash()
    {
        _dashing = false;
    }

    private void DashRecover()
    {
        _currentDashJauge = Mathf.Min(_currentDashJauge + _dashRecoverAmount * Time.deltaTime, _dashJaugeMax);
    }
}
