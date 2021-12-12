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
    private Health _health;
    private PlayerWeaponsController _weaponsController;
    [SerializeField] private Transform _playerNameTransform;
    private Camera _camera;
    [SerializeField] private Vector3 _textOffset;

    public void SetCanvas(Transform canvas)
    {
        _canvas = canvas;
    }

    private void Start()
    {
        _camera = Camera.main;
        _health = GetComponent<Health>();
        _weaponsController = GetComponent<PlayerWeaponsController>();
        UpdateText();
        _infoUI.SetParent(_canvas);
    }

    private void Update()
    {
        var screenPos = _camera.WorldToScreenPoint(transform.position + _textOffset);
        _playerNameTransform.position = screenPos;
        _healthSlider.value = _health.HealthPoints;
        UpdateText();
    }

    public void UpdateText()
    {
            _playerNameText.text = gameObject.name + "\n[" + _weaponsController.CurrentWeaponName() + "]";
    }

    private void OnDisable()
    {
        if (_infoUI != null)
            _infoUI.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _infoUI.gameObject.SetActive(true);
    }
}
