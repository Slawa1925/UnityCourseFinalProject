using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _range;
    [SerializeField] private int _damage;
    [SerializeField] private float _fireTime;
    private float _lastFireTime;
    [SerializeField] private Transform _shootOrigin;
    [SerializeField] private AudioSource _shotAudio;
    [SerializeField] private LineRenderer _bulletTrail;


    private void Update()
    {
        if (_playerController.CanShoot)
        {
            Shoot();
        }  
    }

    public void Shoot()
    {
        if (Time.time - _lastFireTime < _fireTime)
            return;

        _shotAudio.Play();

        if (Physics.Raycast(_shootOrigin.position, transform.forward, out RaycastHit hit, _range))
        {
            if (hit.transform.GetComponent<Health>())
            {
                hit.transform.GetComponent<Health>().TakeDamage(_damage);
            }


            StartCoroutine(DrawBullet(hit.distance));
        }
        else
            StartCoroutine(DrawBullet(_range));

        _lastFireTime = Time.time;
    }

    
    private IEnumerator DrawBullet(float length)
    {
        _bulletTrail.SetPosition(1, new Vector3(0, 0, length));

        yield return new WaitForSeconds(0.1f);

        _bulletTrail.SetPosition(1, Vector3.zero);
    }
}
