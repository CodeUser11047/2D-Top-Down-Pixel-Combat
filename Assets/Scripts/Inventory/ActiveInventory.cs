using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNumber = 0;

    private PlayerControls playerControls;
    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        playerControls.Inventory.KeyBoard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
        playerControls.Inventory.Tab.performed += _ => ToggleActiveHighlightTab();



        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }
        this.transform.GetChild(activeSlotIndexNumber).GetChild(0).gameObject.SetActive(true);
    }

    public void EquipStartingWeapon()
    {
        ToggleActiveHighlight(0);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void ToggleActiveHighlightTab()
    {
        activeSlotIndexNumber++;
        if (activeSlotIndexNumber > 4)
        {
            activeSlotIndexNumber = 0;
        }
        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }
        this.transform.GetChild(activeSlotIndexNumber).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ToggleActiveSlot(int numValue)
    {
        ToggleActiveHighlight(numValue - 1);

    }

    private void ToggleActiveHighlight(int indexNum)
    {
        activeSlotIndexNumber = indexNum;

        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }
        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        Transform childTransform = transform.GetChild(activeSlotIndexNumber);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();

        if (weaponInfo == null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        GameObject weaponToSpawn = weaponInfo.weaponPrefab;
        // GameObject weaponToSpawn = transform.GetChild(activeSlotIndexNumber).GetComponentInChildren<InventorySlot>().GetWeaponInfo().weaponPrefab;
        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);

        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);

        newWeapon.transform.parent = ActiveWeapon.Instance.transform;

        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());

        //Debug.Log(transform.GetChild(activeSlotIndexNumber).GetComponent<InventorySlot>().GetWeaponInfo().weaponPrefab.name);

    }
}
