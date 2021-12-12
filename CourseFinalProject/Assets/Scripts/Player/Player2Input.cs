using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Input : PlayerInput
{
    public override Vector3 CurrentInput()
    {
        return new Vector3(Input.GetAxis("Horizontal2"), 0, Input.GetAxis("Vertical2"));
    }

    public override bool CurrentWeaponInput()
    {
        return Input.GetButton("Fire2");
    }

    public override int WeaponSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.Period))
            return 1;
        if (Input.GetKeyDown(KeyCode.Comma))
            return -1;
        return 0;
    }
}
