using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Braclet", menuName ="Items/Braclet", order = 1)]
public class BraceletItem : ItemObject, IUseable {

    private int slots;

    [SerializeField]
    private GameObject inventoryGroupPrefab;

    public InventoryGroup InventoryGroup { get; set; }

    public int Slots
    {
        get
        {
            return slots;
        }
    }

    public void Instantiate(int _slots)
    {
        slots = _slots;
    }

    public void Use()
    {
        if (InventoryController.Instance.CanAddInventoryGroup)
        {
            Remove();
            InventoryGroup = Instantiate(inventoryGroupPrefab, InventoryController.Instance.transform).GetComponent<InventoryGroup>();
            InventoryGroup.AddSlots(slots);

            InventoryController.Instance.AddGroup(this);
        }
    }
}
