using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour, IPooledObject
{
    [SerializeField] private float _speed = 50.0f;
    [SerializeField] private float _lifeTime = 1.0f;
    private readonly string _tag = "Bullet";
    private ObjectPool _objectPool;
    private Vector3 _targetPositon;
    private float _timer;

    public void SetTargetPosition(Vector3 targetPosition) => _targetPositon = targetPosition;
    public void SetObjectPool(ObjectPool objectPool) => _objectPool = objectPool;

    public void OnEnable()
    {
        _timer = Time.time;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPositon, _speed * Time.deltaTime);

        if (Time.time - _timer > _lifeTime)
            Despawn();
    }

    public void Despawn()
    {
        _objectPool.ReturnToPool(_tag, gameObject);
    }
}
