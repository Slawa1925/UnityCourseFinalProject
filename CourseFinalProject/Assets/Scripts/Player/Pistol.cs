using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public int Ammo { get; private set; }
    public PlayerInput PlayerInput;
    [SerializeField] private float _range;
    [SerializeField] private int _damage;
    [SerializeField] private float _fireTime;
    [SerializeField] private int _maxAmmo;
    private float _lastFireTime;
    [SerializeField] private Transform _shootOrigin;
    [SerializeField] private AudioSource _shotAudio;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _bulletScatter;


    private void Start()
    {
        ResetWeapon();
    }

    public void ResetWeapon()
    {
        Ammo = _maxAmmo;
    }

    public void RefillAmmo()
    {
        Ammo = _maxAmmo;
    }

    private void Update()
    {
        if (PlayerInput.CurrentWeaponInput())
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (Ammo == 0)
            return;

        if (Time.time - _lastFireTime < _fireTime)
            return;

        _shotAudio.Play();
        Ammo--;

        var direction = transform.forward + transform.right * Random.Range(-_bulletScatter, _bulletScatter);

        if (Physics.Raycast(_shootOrigin.position, direction, out RaycastHit hit, _range))
        {
            if (hit.transform.GetComponent<Health>())
            {
                hit.transform.GetComponent<Health>().TakeDamage(_damage);
                hit.transform.LookAt(new Vector3(transform.position.x, hit.transform.position.y, transform.position.z));
            }
            SpawnBullet(hit.point);
        }
        else
            SpawnBullet(_shootOrigin.position + direction * _range);

        _lastFireTime = Time.time;
    }

    private void SpawnBullet(Vector3 target)
    {
        var bullet = Instantiate(_bullet, _shootOrigin.position, _shootOrigin.rotation);
        bullet.GetComponent<BulletController>().Spawn(target);
    }
}
