using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private UnityEvent _event;
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
        if (_event != null)
            _event.Invoke();
    }
}
