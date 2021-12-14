using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract int GetAmmo();

    public abstract void SetPlayerInput(PlayerInput input);
    public abstract void SetObjectPool(ObjectPool input);

    public abstract void ResetWeapon();

    public abstract void RefillAmmo();

    public abstract void Shoot();
}