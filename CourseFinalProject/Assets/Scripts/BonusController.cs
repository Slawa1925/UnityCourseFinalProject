using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController : MonoBehaviour
{
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _respawnTime;
    private float _timer;


    private void Update()
    {
        if (!_collider.enabled)
        {
            if (Time.time - _timer >= _respawnTime)
            {
                _collider.enabled = true;
                _renderer.enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            if (!other.GetComponent<Health>().IsAtMaxHealth())
            {
                other.GetComponent<Health>().Heal();
                print("Healed " + other.name);
            }
            else
            {
                other.GetComponent<PlayerWeaponsController>().GiveRandomAmmo();
            }
            _audioSource.Play();
            _collider.enabled = false;
            _renderer.enabled = false;
            _timer = Time.time;
        }
    }
}
