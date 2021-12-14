using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private ObjectPool _objectPool;
    [SerializeField] private Weapon[] _weapon;
    [SerializeField] private int currentWeapon = 0;

    private void Start()
    {
        for (int i = 0; i < _weapon.Length; i++)
        {
            _weapon[i].SetPlayerInput(_playerInput);
            _weapon[i].SetObjectPool(_objectPool);
            _weapon[i].ResetWeapon();

            if (_weapon[i].GetComponent<Pistol>())
                _weapon[i].GetComponent<Pistol>().OutOfAmmo += SwitchToPistol;
            else if (_weapon[i].GetComponent<Shotgun>())
                _weapon[i].GetComponent<Shotgun>().OutOfAmmo += SwitchToPistol;
            else if (_weapon[i].GetComponent<Grenade>())
                _weapon[i].GetComponent<Grenade>().OutOfAmmo += SwitchToPistol;
        }
        SwitchToWeapon(currentWeapon);
    }

    private void Update()
    {
        if (_playerInput.WeaponSwitchInput() != 0)
        {
            if (_playerInput.WeaponSwitchInput() == 1)
                NextWeapon();
            else
                PreviousWeapon();
        }
    }

    public void SetInput(int i)
    {
        if (i == 0)
            _playerInput = GetComponent<Player1Input>();
        else if (i == 1)
            _playerInput = GetComponent<Player2Input>();
    }

    public void SetPool(ObjectPool objectPool)
    {
        _objectPool = objectPool;
    }

    public void SwitchToPistol()
    {
        SwitchToWeapon(0);
    }

    public void SwitchToWeapon(int weaponIndex)
    {
        currentWeapon = weaponIndex;
        for (int i = 0; i < _weapon.Length; i++)
        {
            if (i == weaponIndex)
                _weapon[weaponIndex].gameObject.SetActive(true);
            else
                _weapon[i].gameObject.SetActive(false);
        }
    }

    private void SwitchWeapon(int input)
    {
        currentWeapon += input;

        if (currentWeapon >= _weapon.Length)
            currentWeapon = 0;
        else if (currentWeapon < 0)
            currentWeapon = _weapon.Length-1;

        if (_weapon[currentWeapon].GetAmmo() == 0)
        {
            SwitchWeapon(input);
            return;
        }
    }

    public void NextWeapon()
    {
        SwitchWeapon(1);
        SwitchToWeapon(currentWeapon);
    }

    public void PreviousWeapon()
    {
        SwitchWeapon(-1);
        SwitchToWeapon(currentWeapon);
    }

    public string CurrentWeaponName()
    {
        var weaponName = _weapon[currentWeapon].name;
        if (_weapon[currentWeapon].GetAmmo() >= 0)
            weaponName += ":" + _weapon[currentWeapon].GetAmmo();
        return weaponName;
    }

    public void GiveRandomAmmo()
    {
        var randomWeapon = Random.Range(1, _weapon.Length);
        _weapon[randomWeapon].RefillAmmo();
        print(_weapon[randomWeapon].name + " Ammo " + name);
    }
}
