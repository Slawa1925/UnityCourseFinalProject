using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float _offset;
    [SerializeField] private Transform _target;

    private void Update()
    {
        if (_target != null)
        {
            transform.position = _target.position + transform.forward * _offset;
        }
    }
}
