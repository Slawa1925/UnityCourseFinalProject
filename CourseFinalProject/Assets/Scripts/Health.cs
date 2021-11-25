using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    private int _health;
    public int HealthPoints => _health;

    public void Start()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        print("damage");
        _health -= damage;
    }
}
