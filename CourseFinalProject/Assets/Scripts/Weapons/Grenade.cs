using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Weapon
{
    public Action OutOfAmmo;

    private int _ammo;
    private PlayerInput _playerInput;
    private ObjectPool _objectPool;
    [SerializeField] private Transform _shootOrigin;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _shotAudio;
    [SerializeField] private float _range;
    [SerializeField] private float _fireTime;
    [SerializeField] private int _maxAmmo;
    private float _lastFireTime;
    [SerializeField] private float _bulletScatter;


    private void Start()
    {
        ResetWeapon();
    }

    public override void SetPlayerInput(PlayerInput input) => _playerInput = input;
    public override void SetObjectPool(ObjectPool input) => _objectPool = input;

    public override void ResetWeapon()
    {
        RefillAmmo();
    }

    public override void RefillAmmo() => _ammo = _maxAmmo;

    private void Update()
    {
        if (_playerInput.CurrentWeaponInput())
        {
            Shoot();
        }
    }

    public override int GetAmmo() => _ammo;

    public override void Shoot()
    {
        if (_ammo == 0)
        {
            OutOfAmmo?.Invoke();
            return;
        }

        if (Time.time - _lastFireTime < _fireTime)
            return;

        _audioSource.PlayOneShot(_shotAudio[UnityEngine.Random.Range(0, _shotAudio.Length)]);
        _ammo--;

        SpawnGrenade();
        _lastFireTime = Time.time;
    }

    private void SpawnGrenade()
    {
        var direction = _shootOrigin.forward + transform.right * UnityEngine.Random.Range(-_bulletScatter, _bulletScatter);
        var grenade = _objectPool.SpawnFromPool("Grenade", _shootOrigin.position, _shootOrigin.rotation);
        grenade.GetComponent<Rigidbody>().AddForce(direction * _range);
    }
}
