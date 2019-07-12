using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject basketPrefab;

    [SerializeField] private float _speed                   = 1.0f;
    [Header("Dash")]
    [SerializeField] private float _dashDuration            = 0.2f;
    [SerializeField] private float _dashSpeed               = 3.0f;
    [SerializeField] private float _dashCost                = 15.0f;
    [SerializeField] private float _dashJaugeMax            = 30f;
    [SerializeField] private float _dashRecoveryAmount      = 1.0f;
    [Header("Speed Boost")]
    [SerializeField] private float _speedBoostFactor        = 2.0f;
    [SerializeField] private float _speedBoostDuration      = 2.0f;
    [SerializeField] private float _speedAppleCost          = 1.0f;
    [Header("Bump by an Obstacle")]
    [SerializeField] private float _stunDuration            = 0.5f;
    [SerializeField] private float _bumpForce               = 5.0f;
    [Header("Bump by a player")]
    [SerializeField] private float _stunOnTargetDuration    = 1.0f;
    [SerializeField] private float _bumpOnTargetForce       = 10.0f;
    [SerializeField] private float _bumpCasterForce         = 3.0f;

    private Rigidbody   _rigidbody;
    private Animator    _animator;
    private bool        _isBoosted  = false;
    private bool        _dashing    = false;
    private bool        _bumped = false;
    private BasketStack _basket;

    private Vector3     _wantedPosition;

    private float _currentDashJauge = 30f;
  

    // Start is called before the first frame update
    void Start()
    {
        _basket = Instantiate(basketPrefab, gameObject.transform).GetComponent<BasketStack>();
        _basket.gameObject.transform.Translate(new Vector3(0, 3f, 0));
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
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
        if (!move || _bumped)
            return;
        if (!_dashing)
            this.transform.rotation = Quaternion.Euler(0, angle-45, 0);
        
        _rigidbody.MovePosition( transform.position + (transform.forward * Time.deltaTime * (_dashing ? _dashSpeed : _speed)));

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
        _currentDashJauge = Mathf.Min(_currentDashJauge + _dashRecoveryAmount * Time.deltaTime, _dashJaugeMax);
    }


    ///////////////////////////////////
    ///// Collision Section       /////
    ///////////////////////////////////
    ///
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<StockSave>() != null)
            _basket.OnStockCollision(collision.gameObject.GetComponent<StockSave>());
        if (collision.gameObject.name.Contains("apple"))
            _basket.OnAppleCollision(collision.gameObject);
        if (collision.gameObject.name.Contains("Tree") && _dashing)
            collision.gameObject.GetComponent<TreeApplesGrow>().DropApples();
        if (_dashing && collision.gameObject.tag == "Obstacle")
        {
            Bump(collision.contacts[0].point, true);
        }
        else if(_dashing && collision.gameObject.tag == "Player")
        {
            BumpOtherPlayer(collision.gameObject, collision.contacts[0].point);
            Bump(collision.contacts[0].point, false);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<StockSave>() != null)
            _basket.OnStopStockCollision();
    }

    private void BumpOtherPlayer(GameObject otherPlayer, Vector3 contactPoint)
    {
        otherPlayer.GetComponent<PlayerController>().BumpByPlayer(contactPoint);
    }

    private void Bump(Vector3 contactPoint ,bool stun)
    {
        _bumped = true;
        _dashing = false;
        Vector3 direction = this.transform.position - new Vector3(contactPoint.x, this.transform.position.y, contactPoint.z);
        StartCoroutine(BumpCoroutine(direction, stun));
    }

    private IEnumerator BumpCoroutine(Vector3 direction, bool _stun)
    {
        float startTime = Time.time;
        float currentTime = 0f;
        float duration = 0.5f;
        Vector3 startPosition = transform.position;
        Vector3 desiredPosition = startPosition + Vector3.Normalize(direction) * (_stun ? _bumpForce : _bumpCasterForce);
        while (currentTime < (_stun ? _stunDuration : duration))
        {
            if (currentTime < duration)
            {
                float fracComplete = (Time.time - startTime) / duration;
                _rigidbody.position = Vector3.Slerp(transform.position, desiredPosition, fracComplete);
            }

            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _bumped = false;
    }

    public void BumpByPlayer(Vector3 contactPoint)
    {
        _bumped = true;
        _dashing = false;
        Vector3 direction = this.transform.position - new Vector3(contactPoint.x, this.transform.position.y, contactPoint.z);
        StartCoroutine(BumpByPlayerCoroutine(direction));
    }

    private IEnumerator BumpByPlayerCoroutine(Vector3 direction)
    {
        float startTime = Time.time;
        float currentTime = 0f;
        float duration = 0.5f;
        Vector3 startPosition = transform.position;
        Vector3 desiredPosition = startPosition + Vector3.Normalize(direction) * _bumpOnTargetForce;
        while (currentTime < _stunOnTargetDuration)
        {
            if (currentTime < duration)
            {
                float fracComplete = (Time.time - startTime) / duration;
                _rigidbody.position = Vector3.Slerp(transform.position, desiredPosition, fracComplete);
            }
                
            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _bumped = false;
    }


}
