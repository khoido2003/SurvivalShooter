using UnityEngine;

public class PickUpWeapon : Interactable
{
    [SerializeField]
    private WeaponData weaponData;

    [SerializeField]
    private BackUpWeaponModel[] backUpWeaponModels;

    private PlayerWeaponController playerWeaponController;

    private void Start()
    {
        UpdateGameObject();
    }

    public void UpdateGameObject()
    {
        gameObject.name = "PickUpWeapon_" + weaponData.weaponType.ToString();

        UpdateItemModel();
    }

    public void UpdateItemModel()
    {
        foreach (BackUpWeaponModel backUpWeaponModel in backUpWeaponModels)
        {
            backUpWeaponModel.gameObject.SetActive(false);

            if (backUpWeaponModel.weaponType == weaponData.weaponType)
            {
                backUpWeaponModel.gameObject.SetActive(true);

                UpdateMeshAndMaterial(backUpWeaponModel.GetComponent<MeshRenderer>());
            }
        }
    }

    public override void Interaction()
    {
        playerWeaponController.PickUpWeapon(weaponData);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (playerWeaponController == null)
        {
            playerWeaponController = other.GetComponent<PlayerWeaponController>();
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
