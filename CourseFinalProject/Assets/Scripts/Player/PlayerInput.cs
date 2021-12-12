using UnityEngine;

public abstract class PlayerInput : MonoBehaviour
{
    public abstract Vector3 CurrentInput();

    public abstract bool CurrentWeaponInput();

    public abstract int WeaponSwitchInput();
}