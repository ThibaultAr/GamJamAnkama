using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject spriteGO;

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
    [SerializeField] private float _bumpDuration            = 0.5f;

    private Rigidbody   _rigidbody;
    private Animator    _animator;
    private CharacterUI   _characterUI;
    private bool        _isBoosted  = false;
    private bool        _dashing    = false;
    private bool        _bumped = false;
    private bool _shooting = false;
    private GameObject _projectile;
    private GameObject _sight;
    public BasketStack basket;
    public GameObject sight;
    public Sprite hitSprite;
    private Sprite _lastSprite;
    private int _playerIndex;



    private Vector3     _wantedPosition;

    private float _currentDashJauge = 30f;

    public enum AnimState {
        FRONT,
        BACK,
        RIGHT,
        LEFT,
        IDLE
    }

    public AnimState currentAnimState = AnimState.IDLE;


    // Start is called before the first frame update
    void Start()
    {
        basket = spriteGO.GetComponentInChildren<BasketStack>();
        _animator = spriteGO.GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _playerIndex = (int)GetComponent<KeyboardInputMgr>().playerIndex +1;
        _characterUI = GameObject.Find("P" + _playerIndex).GetComponent<CharacterUI>();


        basket.transform.eulerAngles = new Vector3(basket.transform.eulerAngles.x, 225f, basket.transform.eulerAngles.z);
        _animator.SetTrigger("Idle");
    }

    private void Update()
    {
        if (_currentDashJauge < _dashJaugeMax)
            DashRecover();

        _characterUI.jaugeImage.fillAmount = _currentDashJauge / _dashJaugeMax;
    }

    ///////////////////////////////////
    ///// Move Section            /////
    ///////////////////////////////////
    public void Move(float angle, bool move = false)
    {
        if (_bumped)
            return;
        
        if (!move && currentAnimState != AnimState.IDLE)
        {
            basket.transform.eulerAngles = new Vector3(basket.transform.eulerAngles.x, 225f, basket.transform.eulerAngles.z);
            currentAnimState = AnimState.IDLE;
            _animator.SetTrigger("Idle");
        }

        if (!move || _bumped)
            return;

        if (!_dashing)
        {
            this.transform.rotation = Quaternion.Euler(0, angle - 45, 0);

            if (!_shooting)
                UpdateAnimationAngle(angle);
            else if (currentAnimState != AnimState.IDLE)
            {
                basket.transform.eulerAngles = new Vector3(basket.transform.eulerAngles.x, 225f, basket.transform.eulerAngles.z);
                currentAnimState = AnimState.IDLE;
                _animator.SetTrigger("Idle");
            }
        }

        if (!_shooting)
            _rigidbody.MovePosition(transform.position + (transform.forward * Time.deltaTime * (_dashing ? _dashSpeed : _speed)));

        //Set le bon angle d'animation
        //De face si idle
    }

    private void UpdateAnimationAngle(float angle)
    {
        AnimState newAnimState = AnimState.IDLE;
        float clampedAngle = Clamp0360(angle);

        if (clampedAngle < 45f || clampedAngle > 315)
        {
            basket.transform.eulerAngles = new Vector3(basket.transform.eulerAngles.x, 45f, basket.transform.eulerAngles.z);
            newAnimState = AnimState.BACK;
        }
        else if (clampedAngle >= 45f && clampedAngle <= 135f)
        {
            basket.transform.eulerAngles = new Vector3(basket.transform.eulerAngles.x, 135f, basket.transform.eulerAngles.z);
            newAnimState = AnimState.LEFT;
        }
        else if (clampedAngle > 135f && clampedAngle < 225f)
        {
            basket.transform.eulerAngles = new Vector3(basket.transform.eulerAngles.x, 225f, basket.transform.eulerAngles.z);
            newAnimState = AnimState.FRONT;
        }
        else if (clampedAngle >= 225f && clampedAngle <= 315f)
        {
            basket.transform.eulerAngles = new Vector3(basket.transform.eulerAngles.x, 315f, basket.transform.eulerAngles.z);
            newAnimState = AnimState.RIGHT;
        }

        if(currentAnimState != newAnimState)
        {
            currentAnimState = newAnimState;
            if (currentAnimState == AnimState.BACK)
                _animator.SetTrigger("Back");
            else if (currentAnimState == AnimState.FRONT)
                _animator.SetTrigger("Front");
            else if (currentAnimState == AnimState.RIGHT)
                _animator.SetTrigger("Right");
            else if (currentAnimState == AnimState.LEFT)
                _animator.SetTrigger("Left");
            else
                _animator.SetTrigger("Idle");
        }

    }

    public float Clamp0360(float angle)
    {
        float result = angle - Mathf.CeilToInt(angle / 360f) * 360f;
        if (result < 0)
        {
            result += 360f;
        }
        return result;
    }

    ///////////////////////////////////
    ///// Speed boost Section     /////
    ///////////////////////////////////
    public void Eat()
    {
        if (_isBoosted || basket.getAppleCount() <= 0)
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
        basket.ConsumeApple();
    }

    ///////////////////////////////////
    ///// Dash boost Section      /////
    ///////////////////////////////////
    public void Dash()
    {
        if (_dashing || _currentDashJauge < _dashCost)
            return;

        SOUND.Dash();
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
            basket.OnStockCollision(collision.gameObject.GetComponent<StockSave>(), _playerIndex);
        if (collision.gameObject.name.Contains("apple") && !basket.IsFull() && !collision.gameObject.GetComponent<Projectile>())
            basket.OnAppleCollision(collision.gameObject);
        if (collision.gameObject.name.Contains("Tree") && _dashing)
            collision.gameObject.GetComponent<TreeApplesGrow>().DropApples();

        if (_dashing && collision.gameObject.tag == "Obstacle")
        {
            Bump(collision.contacts[0].point, true);
            SOUND.HitTree();
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
            basket.OnStopStockCollision();
    }

    private void SwitchToHitSprite(float duration)
    {
        _lastSprite = spriteGO.GetComponentInChildren<SpriteRenderer>().sprite;
        _animator.enabled = false;
        spriteGO.GetComponentInChildren<SpriteRenderer>().sprite = hitSprite;
    }

    

    private void BumpOtherPlayer(GameObject otherPlayer, Vector3 contactPoint)
    {
        SOUND.HitPlayer();
        otherPlayer.GetComponent<PlayerController>().BumpByPlayer(contactPoint);
        otherPlayer.GetComponent<PlayerController>().spriteGO.GetComponentInChildren<BasketStack>().Shake();
    }

    public void Bump(Vector3 contactPoint ,bool stun)
    {
        FindObjectOfType<FXManager>().instanciateHit(contactPoint);
        _bumped = true;
        _dashing = false;
        Vector3 direction = this.transform.position - new Vector3(contactPoint.x, this.transform.position.y, contactPoint.z);
        StartCoroutine(BumpCoroutine(direction, stun));
    }

    private IEnumerator BumpCoroutine(Vector3 direction, bool _stun)
    {
        float startTime = Time.time;
        float currentTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 desiredPosition = startPosition + Vector3.Normalize(direction) * (_stun ? _bumpForce : _bumpCasterForce);
        if(_stun)
            SwitchToHitSprite(_bumpDuration);
        while (currentTime < (_stun ? _stunDuration : _bumpDuration))
        {
            if (currentTime < _bumpDuration)
            {
                float fracComplete = (Time.time - startTime) / _bumpDuration;
                _rigidbody.position = Vector3.Lerp(transform.position, desiredPosition, fracComplete);
            }
            else
            {
                _animator.enabled = true;
            }

            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _animator.enabled = true;
        _bumped = false;
        SOUND.Stun();
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
        Vector3 startPosition = transform.position;
        Vector3 desiredPosition = startPosition + Vector3.Normalize(direction) * _bumpOnTargetForce;
        SwitchToHitSprite(_bumpDuration);
        while (currentTime < _stunOnTargetDuration)
        {
            if (currentTime < _bumpDuration)
            {
                float fracComplete = (Time.time - startTime) / _bumpDuration;
                _rigidbody.position = Vector3.Lerp(transform.position, desiredPosition, fracComplete);
            }
            else
            {
                _animator.enabled = true;
            }

            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _animator.enabled = true;
        _bumped = false;
        SOUND.Stun();
    }

    ///////////////////////////////////
    ///// shoot Section /////
    ///////////////////////////////////
    ///
    public void Aim()
    {
        if (!basket.IsEmpty())
        {
            _shooting = true;
            _projectile = basket.GetProjectile();
            if (_projectile != null)
            {
                _projectile.GetComponent<SphereCollider>().isTrigger = true;
                _projectile.transform.parent = transform;
                _projectile.transform.localPosition = transform.forward + new Vector3(0, 4, 4);
                _sight = Instantiate(sight, transform);
                _sight.transform.localPosition = transform.forward + new Vector3(0, 4, 8);
            }
        }
    }

    public void Shoot()
    {
        _shooting = false;
        if (_projectile != null)
        {
            Destroy(_sight);
            _projectile.GetComponent<Rigidbody>().isKinematic = false;
            _projectile.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            _projectile.GetComponent<Rigidbody>().AddForce((Vector3.Normalize(transform.forward) + new Vector3(0, 0.1f, 0)) * 1000);
            _projectile.transform.parent = null;
            StartCoroutine(DisableProjectileTrigger(_projectile));
            _projectile = null;
            SOUND.ThrowApple();
        }
    }

    private IEnumerator DisableProjectileTrigger(GameObject apple)
    {
        yield return new WaitForSeconds(0.1f);
        apple.GetComponent<SphereCollider>().isTrigger = false;
        apple.AddComponent<Projectile>();
    }

}
