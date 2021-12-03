using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
public class ZombieController : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _range;
    [SerializeField] private Transform _attackOrigin;
    [SerializeField] private float _attackDistance;
    [SerializeField] private int _damage;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _attackTime = 1.0f;
    private float _timer;


    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void Chase()
    {
        _agent.SetDestination(_target.transform.position);

        if (_agent.path.corners.Length > 1)
            transform.LookAt(_agent.path.corners[1]);
        _controller.Move(transform.forward * _speed * Time.deltaTime);
    }

    public void Attack()
    {
        _timer = Time.time;

        if (Physics.Raycast(_attackOrigin.position, transform.forward, out RaycastHit hit, _range))
        {
            if (hit.transform.GetComponent<Health>())
            {
                hit.transform.GetComponent<Health>().TakeDamage(_damage);
            }
        }
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

    public void Die()
    {
        _controller.enabled = false;
    }
}
