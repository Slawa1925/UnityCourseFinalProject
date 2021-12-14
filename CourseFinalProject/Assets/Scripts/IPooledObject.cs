using UnityEngine;

public interface IPooledObject
{
    void SetObjectPool(ObjectPool objectPool);
    void Despawn();
}
