using UnityEngine;

public enum HangType
{
    LowBackHang,
    BackHang,
    SideHang,
}

public class BackUpWeaponModel : MonoBehaviour
{
    public WeaponType weaponType;

    [SerializeField]
    private HangType hangType;

    public void Activate(bool activated) => gameObject.SetActive(activated);

    public bool HangTypeIs(HangType hangType) => this.hangType == hangType;

    public HangType GetHangType()
    {
        return hangType;
    }
}
