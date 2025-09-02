using System;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoBoxType
{
    SmallBox,
    BigBox,
}

[Serializable]
public struct AmmoData
{
    public WeaponType weaponType;
    public int amount;

    [Range(10, 100)]
    public int minAmount;

    [Range(10, 100)]
    public int maxAmount;
}

public class PickupAmmo : Interactable, IObjectItemPoolable
{
    [SerializeField]
    private AmmoBoxType ammoBoxType;

    [SerializeField]
    private List<AmmoData> smallBoxAmmo;

    [SerializeField]
    private List<AmmoData> bigBoxAmmo;

    [SerializeField]
    private GameObject[] boxModels;

    private void Start()
    {
        SetupBoxModels();
    }

    private void OnEnable() { }

    private void SetupBoxModels()
    {
        for (int i = 0; i < boxModels.Length; i++)
        {
            boxModels[i].SetActive(false);

            if (i == (int)ammoBoxType)
            {
                boxModels[i].SetActive(true);

                UpdateMeshAndMaterial(boxModels[i].GetComponent<MeshRenderer>());
            }
        }
    }

    public override void Interaction()
    {
        List<AmmoData> currentAmmoList = smallBoxAmmo;

        if (ammoBoxType == AmmoBoxType.BigBox)
        {
            currentAmmoList = bigBoxAmmo;
        }

        foreach (AmmoData ammoData in currentAmmoList)
        {
            Weapon weapon = playerWeaponController.HasWeaponTypeInventory(ammoData.weaponType);

            AddBulletsToWeapon(weapon, GetBulletAmount(ammoData));
        }

        PoolManager.Instance.Return<PickupAmmo>(this);
    }

    private void AddBulletsToWeapon(Weapon weapon, int amount)
    {
        if (weapon == null)
        {
            return;
        }

        weapon.totalReserveAmmo += amount;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    private int GetBulletAmount(AmmoData ammoData)
    {
        float min = Mathf.Min(ammoData.minAmount, ammoData.maxAmount);
        float max = Mathf.Max(ammoData.minAmount, ammoData.maxAmount);

        float randomAmount = UnityEngine.Random.Range(min, max);

        return Mathf.RoundToInt(randomAmount);
    }

    public void OnSpawn() { }

    public void OnDespawn() { }
}
