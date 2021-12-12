using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float _speed = 50.0f;
    [SerializeField] private Vector3 _targetPositon;
    private float _timer;
    [SerializeField] private float _lifeTime = 1.0f;

    public void Spawn(Vector3 targetPosition)
    {
        _timer = Time.time;
        _targetPositon = targetPosition;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPositon, _speed * Time.deltaTime);

        if (Time.time - _timer > _lifeTime)
            Despawn();
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }
}
