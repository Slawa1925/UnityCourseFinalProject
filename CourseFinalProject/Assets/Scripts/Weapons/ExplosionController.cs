using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour, IPooledObject
{
    [SerializeField] private CameraShake _cameraShake;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClip;
    [SerializeField] private GameObject _explosionModel;
    [SerializeField] private float _startScale = 1.0f;
    [SerializeField] private float _maxScale = 2.0f;
    [SerializeField] private float _lifeTime = 1.0f;
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private int _damage = 10;
    private float _timer;
    private readonly string _tag = "Explosion";
    private ObjectPool _objectPool;

    public void SetObjectPool(ObjectPool objectPool) => _objectPool = objectPool;

    public void Explode()
    {
        _timer = Time.time;
        _explosionModel.transform.localScale = Vector3.one * _startScale;
        _audioSource.PlayOneShot(_audioClip[Random.Range(0, _audioClip.Length)]);

        if (_cameraShake == null)
        {
            _cameraShake = Camera.main.GetComponent<CameraShake>();
        }

        _cameraShake.StartShaking();

        var collidersHit = Physics.OverlapSphere(transform.position, _maxScale / 2);
        foreach (var collider in collidersHit)
        {
            if (collider.GetComponent<Health>())
            {
                collider.GetComponent<Health>().TakeDamage(_damage);
                collider.transform.LookAt(new Vector3(transform.position.x, collider.transform.position.y, transform.position.z));
                _objectPool.SpawnFromPool("Blood", collider.transform.position + Vector3.up * 0.5f, Quaternion.LookRotation(-collider.transform.forward));
            }
        }
    }

    private void Update()
    {
        _explosionModel.transform.localScale = Vector3.Lerp(_explosionModel.transform.localScale, Vector3.one * _maxScale, _speed * Time.deltaTime);

        if (Time.time - _timer > _lifeTime)
        {
            _objectPool.SpawnFromPool("Crator", new Vector3(transform.position.x, 0.01f, transform.position.z), Quaternion.identity);
            Despawn();
        }
    }

    public void Despawn()
    {
        _objectPool.ReturnToPool(_tag, gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _maxScale / 2);
    }
}
