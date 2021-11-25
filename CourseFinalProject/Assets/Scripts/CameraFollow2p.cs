using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2p : MonoBehaviour
{
    [SerializeField] private float _offset;
    [SerializeField] private Transform _player1;
    [SerializeField] private Transform _player2;
    [SerializeField] private Vector3 _target;
    [SerializeField] private float _maxPlayerDistanceHorizontal;
    [SerializeField] private float _maxPlayerDistanceVertical;
    [SerializeField] private Vector3 _levelSize;
    private Vector3 _lastPos;

    private void Start()
    {
        Follow();
    }

    private void Update()
    {
        if (_player1 == null || _player2 == null)
            return;

        if (DistanceHorizontal() > _maxPlayerDistanceHorizontal || DistanceVertical() > _maxPlayerDistanceVertical)
        {
            Vector3 movementLimiter1 = Vector3.zero;
            Vector3 movementLimiter2 = Vector3.zero;

            if (DistanceHorizontal() > _maxPlayerDistanceHorizontal)
            {
                movementLimiter1 = new Vector3(_player1.position.x - _target.x, 0, 0);
                movementLimiter2 = new Vector3(_player2.position.x - _target.x, 0, 0);
            }

            if (DistanceVertical() > _maxPlayerDistanceVertical)
            {
                movementLimiter1 = new Vector3(movementLimiter1.x, 0, _player1.position.z - _target.z);
                movementLimiter2 = new Vector3(movementLimiter2.x, 0, _player2.position.z - _target.z);
            }

            _player1.GetComponent<PlayerController>().LimitMovement(movementLimiter1);
            _player2.GetComponent<PlayerController>().LimitMovement(movementLimiter2);
        }

        Follow();
    }

    private void Follow()
    {
        _target = (_player1.position + _player2.position) / 2;
       
        if (_target.x * _target.x > (_levelSize.x / 2 - _maxPlayerDistanceHorizontal / 2) * (_levelSize.x / 2 - _maxPlayerDistanceHorizontal / 2))
        {
            _target = new Vector3(
                _lastPos.x,
                _target.y,
                _target.z);
        }

        if (_target.z * _target.z > (_levelSize.z / 2 - _maxPlayerDistanceVertical / 2) * (_levelSize.z / 2 - _maxPlayerDistanceVertical / 2))
        {
            _target = new Vector3(
                _target.x,
                _target.y,
                _lastPos.z);
        }

        transform.position = _target + transform.forward * _offset;
        _lastPos = _target;
    }

    private float DistanceHorizontal()
    {
        return Vector3.Distance(
            new Vector3(_player1.position.x, 0, 0),
            new Vector3(_player2.position.x, 0, 0));
    }

    private float DistanceVertical()
    {
        return Vector3.Distance(
            new Vector3(0, 0, _player1.position.z),
            new Vector3(0, 0, _player2.position.z));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(0, _levelSize.y / 2, 0), _levelSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(0, _levelSize.y / 2, 0), new Vector3(_levelSize.x, _levelSize.y, _levelSize.z - _maxPlayerDistanceVertical));

    }
}
