using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2p : MonoBehaviour
{
    public Transform Player0;
    public Transform Player1;
    public Queue<Transform> Player;
    [SerializeField] private float _offset;
    [SerializeField] private Vector3 _target;
    [SerializeField] private float _maxPlayerDistanceHorizontal;
    [SerializeField] private float _maxPlayerDistanceVertical;
    [SerializeField] private Vector3 _levelSize;
    [SerializeField] private float _speed = 1.0f;
    private Vector3 _lastPos;


    private void Start()
    {
        Follow();
    }

    private void Update()
    {
        if (Player0 == null || Player1 == null)
            return;

        if (DistanceHorizontal() > _maxPlayerDistanceHorizontal || DistanceVertical() > _maxPlayerDistanceVertical)
        {
            Vector3 movementLimiter1 = Vector3.zero;
            Vector3 movementLimiter2 = Vector3.zero;

            if (DistanceHorizontal() > _maxPlayerDistanceHorizontal)
            {
                movementLimiter1 = new Vector3(Player0.position.x - _target.x, 0, 0);
                movementLimiter2 = new Vector3(Player1.position.x - _target.x, 0, 0);
            }

            if (DistanceVertical() > _maxPlayerDistanceVertical)
            {
                movementLimiter1 = new Vector3(movementLimiter1.x, 0, Player0.position.z - _target.z);
                movementLimiter2 = new Vector3(movementLimiter2.x, 0, Player1.position.z - _target.z);
            }

            Player0.GetComponent<PlayerController>().LimitMovement(movementLimiter1);
            Player1.GetComponent<PlayerController>().LimitMovement(movementLimiter2);
        }

        Follow();
    }

    private void Follow()
    {
        if (!Player0.GetComponent<PlayerController>().isAlive)
            _target = Player1.position;
        else if (!Player1.GetComponent<PlayerController>().isAlive)
            _target = Player0.position;
        else
            _target = (Player0.position + Player1.position) / 2;

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


        var targetPosition = _target + transform.forward * _offset;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
        _lastPos = _target;
    }

    private float DistanceHorizontal()
    {
        if (Player0.GetComponent<PlayerController>().isAlive && Player1.GetComponent<PlayerController>().isAlive)
        {
            return Vector3.Distance(
                new Vector3(Player0.position.x, 0, 0),
                new Vector3(Player1.position.x, 0, 0));
        }
        else
        {
            return 0;
        }
    }

    private float DistanceVertical()
    {
        if (Player0.GetComponent<PlayerController>().isAlive && Player1.GetComponent<PlayerController>().isAlive)
        {
            return Vector3.Distance(
            new Vector3(0, 0, Player0.position.z),
            new Vector3(0, 0, Player1.position.z));
        }
        else
        {
            return 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(0, _levelSize.y / 2, 0), _levelSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(0, _levelSize.y / 2, 0), new Vector3(_levelSize.x, _levelSize.y, _levelSize.z - _maxPlayerDistanceVertical));
    }
}
