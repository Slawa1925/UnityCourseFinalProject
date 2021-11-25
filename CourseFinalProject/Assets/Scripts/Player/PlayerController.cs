using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _speed;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private GameObject _model;
    [SerializeField] private float _maxPlayerDistance;
    private Vector3 _movementLimiter;
    private Vector3 _input;
    private bool _canShoot;
    public bool CanShoot => _canShoot;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _input = _playerInput.CurrentInput();

        if ((_movementLimiter.x > 0 && _input.x > 0) || (_movementLimiter.x < 0 && _input.x < 0))
        {
            _input = new Vector3(0, _input.y, _input.z);
        }

        if ((_movementLimiter.z > 0 && _input.z > 0) || (_movementLimiter.z < 0 && _input.z < 0))
        {
            _input = new Vector3(_input.x, _input.y, 0);
        }

        _movementLimiter = Vector3.zero;

        _controller.Move(_input * _speed * Time.deltaTime);
        Rotate();

        _canShoot = _playerInput.CurrentWeaponInput();
    }

    private void Rotate()
    {
        if (_input.x > 0 && _input.z > 0)
            transform.eulerAngles = new Vector3(0, 45, 0);
        else if (_input.x > 0 && _input.z < 0)
            transform.eulerAngles = new Vector3(0, 135, 0);
        else if (_input.x < 0 && _input.z > 0)
            transform.eulerAngles = new Vector3(0, -45, 0);
        else if (_input.x < 0 && _input.z < 0)
            transform.eulerAngles = new Vector3(0, -135, 0);
        else if (_input.x > 0)
            transform.eulerAngles = new Vector3(0, 90, 0);
        else if (_input.x < 0)
            transform.eulerAngles = new Vector3(0, -90, 0);
        else if (_input.z > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        else if (_input.z < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
    }

    public void LimitMovement(Vector3 dir)
    {
        _movementLimiter = dir.normalized;
    }
}
