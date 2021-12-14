using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
public class ZombieController : MonoBehaviour
{
    public enum State {Idle, Chase, Attack, TakeDamage, Dead };
    
    public Action<ZombieController> GetTarget;
    public Action<ZombieController> OnDeath;

    private ObjectPool _objectPool;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private Health _health;
    [SerializeField] private GameObject _target;
    [SerializeField] private Transform _attackOrigin;
    [SerializeField] private float _speed;
    [SerializeField] private float _range;
    [SerializeField] private float _attackDistance;
    [SerializeField] private int _damage;
    [SerializeField] private float _attackTime = 1.0f;
    [SerializeField] private float _damageKnockback = 1.0f;
    [SerializeField] private float _damageStunDuration = 2.0f;
    private float _damageStunTimer;
    private float _timer;
    public State _state;


    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        switch (_state)
        {
            case State.Idle:
                _state = State.Chase;
                _animator.SetBool("isIdle", false);
                _animator.SetInteger("State", (int)_state);
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                break;
            case State.TakeDamage:
                DamageStun();
                break;
            case State.Dead:
                break;
            default: break;
        }
    }

    public void SetObjectPool(ObjectPool objectPool) => _objectPool = objectPool;

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void Chase()
    {
        _agent.SetDestination(_target.transform.position);

        if (_agent.path.corners.Length > 1)
            transform.LookAt(_agent.path.corners[1]);

        var input = transform.forward;

        if (transform.position.y > 0.01)
        {
            input += Vector3.down;
        }

        _controller.Move(input * _speed * Time.deltaTime);

        if (CanAttack())
        {
            _state = State.Attack;
            _animator.SetInteger("State", (int)_state);
        }
    }

    public void Attack()
    {
        if (_state == State.Dead)
            return;

        _timer = Time.time;

        if (Physics.Raycast(_attackOrigin.position, transform.forward, out RaycastHit hit, _range))
        {
            if (hit.transform.GetComponent<Health>())
            {
                hit.transform.GetComponent<Health>().TakeDamage(_damage);
                hit.transform.LookAt(new Vector3(transform.position.x, hit.transform.position.y, transform.position.z));
            }
        }

        _state = State.Chase;
        _animator.SetInteger("State", (int)_state);
    }

    public bool CanAttack()
    {
        if (Time.time - _timer > _attackTime)
        {
            if (Vector3.Distance(transform.position, _target.transform.position) < _attackDistance)
            {
                return true;
            }
        }
        return false;
    }

    public void DamageStun()
    {
        _controller.Move(-transform.forward * _damageKnockback * Time.deltaTime);

        if (Time.time - _damageStunTimer > _damageStunDuration)
        {
            _state = State.Chase;
            _animator.SetInteger("State", (int)_state);
        }
    }

    public void TakeDamage()
    {
        _objectPool.SpawnFromPool("BloodStain", new Vector3(transform.position.x, 0.01f, transform.position.z), transform.rotation);
        _state = State.TakeDamage;
        _animator.SetInteger("State", (int)_state);
        _damageStunTimer = Time.time;

        if (_health.HealthPoints <= 0)
        {
            _state = State.Dead;
            _animator.SetBool("isDead", true);
            _animator.SetInteger("State", (int)_state);
            Die();
        }
    }

    public void Die()
    {
        _controller.enabled = false;
        _agent.enabled = false;
        OnDeath?.Invoke(this);
    }
}
