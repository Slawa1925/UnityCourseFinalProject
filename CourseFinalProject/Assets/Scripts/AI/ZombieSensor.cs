using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSensor : MonoBehaviour
{
    [SerializeField] private ZombieController _zombieController;
    [SerializeField] private Animator _animator;
    [SerializeField] private Health _health;
    

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _zombieController = GetComponent<ZombieController>();
        _animator.SetInteger("Health", _health.HealthPoints);
    }

    private void Update()
    {
        if (_zombieController.CanAttack())
            _animator.SetBool("canAttack", true);
        else
            _animator.SetBool("canAttack", false);
    }

    public void TakeDamage()
    {
        _animator.SetBool("isTakingDamage", true);
        _animator.SetInteger("Health", _health.HealthPoints);
    }
}
