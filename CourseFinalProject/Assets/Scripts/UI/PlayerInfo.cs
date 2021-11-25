using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private Transform _canvas;
    [SerializeField] private Transform _infoUI;
    [SerializeField] private Text _playerNameText;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Health _health;
    [SerializeField] private Transform _playerNameTransform;
    [SerializeField] private GameObject _player;
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector3 _textOffset;

    private void Start()
    {
        _playerNameText.text = _player.name;
        _infoUI.SetParent(_canvas);
    }

    private void Update()
    {
        var screenPos = _camera.WorldToScreenPoint(_player.transform.position + _textOffset);
        _playerNameTransform.position = screenPos;
        _healthSlider.value = _health.HealthPoints;
    }
}
