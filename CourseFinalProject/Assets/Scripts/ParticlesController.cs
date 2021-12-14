using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour, IPooledObject
{
    [SerializeField] private ParticleSystem _particleSystem;
    private readonly string _tag = "Blood";
    private ObjectPool _objectPool;

    private void OnEnable()
    {
        _particleSystem.Play();
    }

    public void SetObjectPool(ObjectPool objectPool) => _objectPool = objectPool;

    public void Despawn()
    {
        _objectPool.ReturnToPool(_tag, gameObject);
    }
}
