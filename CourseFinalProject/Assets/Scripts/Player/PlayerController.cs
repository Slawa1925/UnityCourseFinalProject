using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    enum State { Normal, TakingDamage, Dead };

    public event Action DeathEvent;

    public bool CanShoot { get; private set; }
    public int WeaponSwitch { get; private set; }
    public bool isAlive;

    private State _state;
    [SerializeField] private GameObject _model;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Animator _animator;
    [SerializeField] private Health _health;
    private ObjectPool _objectPool;
    [SerializeField] private float _speed;
    [SerializeField] private float _spawnInvincibilityTime = 2.0f;
    [SerializeField] private float _damageStunDuration = 2.0f;
    private float _damageStunTimer;
    private Vector3 _movementLimiter;
    private Vector3 _input;
    private float _spawnInvincibilityTimer;
    [SerializeField] private float _damageKnockback = 1.0f;


    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_health.IsInvincible)
        {
            if (Time.time - _spawnInvincibilityTimer > _spawnInvincibilityTime)
            {
                _health.IsInvincible = false;
                _animator.SetBool("Invincible", false);
            }
        }

        switch (_state)
        {
            case State.Normal:
                InputMovement();
                break;
            case State.TakingDamage:
                DamageStun();
                break;
            case State.Dead:
                break;
            default:
                break;
        }
    }

    public void SetObjectPool(ObjectPool objectPool) => _objectPool = objectPool;

    public void SetInput(int i)
    {
        if (i == 0)
            _playerInput = GetComponent<Player1Input>();
        else if (i == 1)
            _playerInput = GetComponent<Player2Input>();
    }

    public void Spawn()
    {
        _state = State.Normal;
        isAlive = true;
        _model.SetActive(true);
        _health.Heal();
        _spawnInvincibilityTimer = Time.time;
        _health.IsInvincible = true;
        _animator.SetBool("Invincible", true);
        ResetAnimator();
    }

    public void ResetAnimator()
    {
        _animator.SetBool("isTakingDamage", false);
        _animator.SetBool("isDead", false);
        _animator.SetBool("isWalking", false);
    }

    public void Die()
    {
        gameObject.transform.position += Vector3.up * 100;
        isAlive = false;
        DeathEvent?.Invoke();
    }

    public void TakeDamage()
    {
        _objectPool.SpawnFromPool("BloodStain", new Vector3(transform.position.x, 0.01f, transform.position.z), transform.rotation);
        _state = State.TakingDamage;
        _animator.SetBool("isTakingDamage", true);
        _damageStunTimer = Time.time;

        if (_health.HealthPoints <= 0)
        {
            if (!GetComponent<AudioSource>().isPlaying)
                GetComponent<AudioSource>().Play();
            _state = State.Dead;
            _animator.SetBool("isDead", true);
        }
    }

    public void DamageStun()
    {
        Move(-transform.forward, _damageKnockback);

        if (Time.time - _damageStunTimer > _damageStunDuration)
        {
            _state = State.Normal;
            _animator.SetBool("isTakingDamage", false);
        }
    }

    public void InputMovement()
    {
        _input = _playerInput.CurrentInput();

        Move(_input, _speed);
        Rotate();

        if (_input.x > 0 || _input.x < 0 || _input.z > 0 || _input.z < 0)
            _animator.SetBool("isWalking", true);
        else
            _animator.SetBool("isWalking", false);

        //CanShoot = _playerInput.CurrentWeaponInput();
        //WeaponSwitch = _playerInput.WeaponSwitchInput();
    }

    public void Move(Vector3 input, float speed)
    {
        if ((_movementLimiter.x > 0 && input.x > 0) || (_movementLimiter.x < 0 && input.x < 0)) // try to shorten this (static utilit script with isNotZero function with 0.001f margins)
            input = new Vector3(0, input.y, input.z);

        if ((_movementLimiter.z > 0 && input.z > 0) || (_movementLimiter.z < 0 && input.z < 0))
            input = new Vector3(input.x, input.y, 0);

        if (transform.position.y > 0.01)
        {
            input += Vector3.down;
        }

        _movementLimiter = Vector3.zero;
        _controller.Move(input * speed * Time.deltaTime);
    }

    private void Rotate()
    {
        if (_input.x > 0 && _input.z > 0)
            transform.eulerAngles = new Vector3(0, 45, 0);
        else if (_input.x > 0 && _input.z < 0)
            transform.eulerAngles = new Vector3(0, 135, 0);
        else if (_input.x < 0 && _input.z > 0)
            transform.eulerAngles = new Vector3(0, -45, 0);
        else if (_input.x < 0 && _input.z < 0)
            transform.eulerAngles = new Vector3(0, -135, 0);
        else if (_input.x > 0)
            transform.eulerAngles = new Vector3(0, 90, 0);
        else if (_input.x < 0)
            transform.eulerAngles = new Vector3(0, -90, 0);
        else if (_input.z > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        else if (_input.z < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
    }

    public void LimitMovement(Vector3 dir)
    {
        _movementLimiter = dir.normalized;
    }
}
