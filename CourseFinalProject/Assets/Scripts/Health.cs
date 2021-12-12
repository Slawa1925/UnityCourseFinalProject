using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int HealthPoints { get; private set; }
    [SerializeField] private int _maxHealth;
    public event Action DamageAction;
    public bool IsInvincible;

    public void Start()
    {
        HealthPoints = _maxHealth;
    }

    public void Heal()
    {
        HealthPoints = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (IsInvincible)
            return;

        HealthPoints -= damage;
        DamageAction?.Invoke();
    }
}
