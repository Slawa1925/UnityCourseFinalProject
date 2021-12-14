using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : MonoBehaviour, IPooledObject
{
    [SerializeField] private float _lifeTime = 1.0f;
    private float _timer;
    private readonly string _tag = "Grenade";
    private ObjectPool _objectPool;

    public void SetObjectPool(ObjectPool objectPool) => _objectPool = objectPool;

    public void OnEnable()
    {
        _timer = Time.time;
    }

    private void Update()
    {
        if (Time.time - _timer > _lifeTime)
        {
            _objectPool.SpawnFromPool("Explosion", transform.position, transform.rotation).GetComponent<ExplosionController>().Explode();
            Despawn();
        }
    }

    public void Despawn()
    {
        _objectPool.ReturnToPool(_tag, gameObject);
    }
}
