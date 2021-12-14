using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void ResetWeapon();

    void RefillAmmo();

    void Shoot();
}
