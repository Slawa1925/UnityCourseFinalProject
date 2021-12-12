using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsController : MonoBehaviour
{
    private PlayerInput _playerInput;
    [SerializeField] private GameObject[] _weapon;
    [SerializeField] private int currentWeapon = 0;

    private void Start()
    {
        for (int i = 0; i < _weapon.Length; i++)
        {
            _weapon[i].GetComponent<Pistol>().PlayerInput = _playerInput;
            _weapon[i].GetComponent<Pistol>().ResetWeapon();
        }
        SwitchToWeapon(currentWeapon);
    }

    public void SetInput(int i)
    {
        if (i == 0)
            _playerInput = GetComponent<Player1Input>();
        else if (i == 1)
            _playerInput = GetComponent<Player2Input>();
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

    public void SwitchToWeapon(int weaponIndex)
    {
        currentWeapon = weaponIndex;
        for (int i = 0; i < _weapon.Length; i++)
        {
            if (i == weaponIndex)
                _weapon[weaponIndex].SetActive(true);
            else
                _weapon[i].SetActive(false);
        }
    }

    private void SwitchWeapon(int input)
    {
        currentWeapon += input;

        if (currentWeapon >= _weapon.Length)
            currentWeapon = 0;
        else if (currentWeapon < 0)
            currentWeapon = _weapon.Length-1;

        if (_weapon[currentWeapon].GetComponent<Pistol>().Ammo == 0)
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
        if (_weapon[currentWeapon].GetComponent<Pistol>().Ammo >= 0)
            weaponName += ":" + _weapon[currentWeapon].GetComponent<Pistol>().Ammo;
        return weaponName;
    }
}
