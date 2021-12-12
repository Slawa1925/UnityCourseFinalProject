using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Input : PlayerInput
{
    public override Vector3 CurrentInput()
    {
        return new Vector3(Input.GetAxis("Horizontal1"), 0, Input.GetAxis("Vertical1"));
    }

    public override bool CurrentWeaponInput()
    {
        return Input.GetButton("Fire1");
    }

    public override int WeaponSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
            return 1;
        if (Input.GetKeyDown(KeyCode.Q))
            return -1;
        return 0;
    }
}
