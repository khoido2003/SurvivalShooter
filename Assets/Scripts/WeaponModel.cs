using UnityEngine;
using UnityEngine.PlayerLoop;

public enum GrabType
{
    SideGrab,
    BackGrab,
}

public enum HoldType
{
    CommonHold = 1,
    LowHold,
    HighHold,
}

public class WeaponModel : MonoBehaviour
{
    public WeaponType weaponType;
    public GrabType grabType;
    public HoldType holdType;

    public Transform gunPoint;
    public Transform holdPoint;
}
