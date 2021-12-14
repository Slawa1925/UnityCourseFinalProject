using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField] private float _shakeSpeed;
    [SerializeField] private float _magnitude;
    [SerializeField] private float _duration;
    private Vector3 _originalPos;


    private void Start()
    {
        _originalPos = _camera.transform.localPosition;
    }

    public void StartShaking()
    {
        StopCoroutine(Shake());
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        _originalPos = _camera.transform.localPosition;
        float dx, dy, dz, elapsed = 0.0f;

        while (elapsed < _duration)
        {
            dx = Random.Range(-1f, 1f) * _magnitude * (1 - elapsed / _duration);
            dy = Random.Range(-1f, 1f) * _magnitude * (1 - elapsed / _duration);
            dz = Random.Range(-1f, 1f) * _magnitude * (1 - elapsed / _duration);

            _camera.transform.localPosition = Vector3.MoveTowards(_camera.transform.localPosition, new Vector3(dx, dy, dz), _shakeSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _camera.transform.localPosition = _originalPos;
    }
}
