using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _range;
    [SerializeField] private int _damage;
    [SerializeField] private float _fireTime;
    private float _fireTimer;
    [SerializeField] private Transform _shootOrigin;


    private void Update()
    {
        if (_fireTimer >= 0)
            _fireTimer -= Time.deltaTime;

        if (_playerController.CanShoot)
        {
            Shoot();
        }

        
    }

    public void Shoot()
    {
        if (_fireTimer > 0)
            return;

        RaycastHit hit;

        //Debug.DrawRay(_shootOrigin.position, transform.forward, Color.blue, 1.0f);

        if (Physics.Raycast(_shootOrigin.position, transform.forward, out hit, _range))
        {
            if (hit.transform.GetComponent<Health>())
            {
                hit.transform.GetComponent<Health>().TakeDamage(_damage);
            }
        }

        print("shot");

        _fireTimer = _fireTime;
    }
}
