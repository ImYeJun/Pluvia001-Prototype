using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }

    [SerializeField] private int _health;
    public int Health => _health;

    [SerializeField] private bool _isInvincible;
    [SerializeField] private float _invincibleTime;

    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _hitEffectMaterial;
    private Material _originalMaterial;

    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private float _shakeTime;
    [SerializeField] private float _shakeIntensity;

    private const int MAX_HEALTH = 100;
    private const int MIN_HEALTH = 0;

    private Action<int, int> _healthChangeObserver;
    public void SetHealthChangeObserver(Action<int, int> observer)
    {
        _healthChangeObserver = observer;

        _health = MAX_HEALTH;
        _healthChangeObserver(_health, MAX_HEALTH);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalMaterial = _spriteRenderer.material;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseHealth(int value = 1)
    {
        _health += value;
        if (_health > MAX_HEALTH)
        {
            _health = MAX_HEALTH;
        }

        _healthChangeObserver(_health, MAX_HEALTH);
    }

    public void DecreaseHealth(int value = 1)
    {
        if (!_isInvincible)
        {
            _health -= value;
            if (_health < MIN_HEALTH)
            {
                _health = MIN_HEALTH;
            }

            StartCoroutine(GetInvincible());
            StartCoroutine(ShakeCamera());

            _healthChangeObserver(_health, MAX_HEALTH);
        }
    }

    private IEnumerator GetInvincible()
    {
        _isInvincible = true;
        _originalMaterial = _spriteRenderer.material;

        _spriteRenderer.material = _hitEffectMaterial;
        yield return new WaitForSeconds(_invincibleTime);

        _spriteRenderer.material = _originalMaterial;
        _isInvincible = false;
    }

    private IEnumerator ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin sex = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        sex.m_AmplitudeGain = _shakeIntensity;

        yield return new WaitForSeconds(_shakeTime);

        sex.m_AmplitudeGain = 0;
    }

    //! Test
    // private void Update() {
    //     if (Input.GetKey(KeyCode.LeftArrow))
    //     {
    //         DecreaseHealth();
    //     }
    //     if (Input.GetKey(KeyCode.RightArrow))
    //     {
    //         IncreaseHealth();
    //     }
    // }
}
