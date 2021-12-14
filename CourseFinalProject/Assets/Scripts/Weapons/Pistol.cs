using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public Action OutOfAmmo;
    private int _ammo;
    private PlayerInput _playerInput;
    private ObjectPool _objectPool;
    [SerializeField] private float _range;
    [SerializeField] private int _damage;
    [SerializeField] private float _fireTime;
    [SerializeField] private int _maxAmmo;
    private float _lastFireTime;
    [SerializeField] private Transform _shootOrigin;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _shotAudio;
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

        var direction = transform.forward + transform.right * UnityEngine.Random.Range(-_bulletScatter, _bulletScatter);

        if (Physics.Raycast(_shootOrigin.position, direction, out RaycastHit hit, _range))
        {
            if (hit.transform.GetComponent<Health>())
            {
                hit.transform.GetComponent<Health>().TakeDamage(_damage);
                hit.transform.LookAt(new Vector3(transform.position.x, hit.transform.position.y, transform.position.z));
                _objectPool.SpawnFromPool("Blood", hit.transform.position + Vector3.up * 0.5f, Quaternion.LookRotation(-hit.transform.forward));
            }
            SpawnBullet(hit.point);
        }
        else
            SpawnBullet(_shootOrigin.position + direction * _range);

        _lastFireTime = Time.time;
    }

    private void SpawnBullet(Vector3 target)
    {
        var bullet = _objectPool.SpawnFromPool("Bullet", _shootOrigin.position, _shootOrigin.rotation);
        bullet.GetComponent<BulletController>().SetTargetPosition(target);
    }
}
