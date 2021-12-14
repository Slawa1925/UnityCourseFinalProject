using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decal : MonoBehaviour, IPooledObject
{
    private readonly string _tag = "Decal";
    private ObjectPool _objectPool;

    public void SetObjectPool(ObjectPool objectPool) => _objectPool = objectPool;

    public void Despawn()
    {
        _objectPool.ReturnToPool(_tag, gameObject);
    }
}
