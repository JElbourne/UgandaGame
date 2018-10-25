using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemObject : ScriptableObject {

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int stackSize;

    private InventorySlot slot;

    public int StackSize
    {
        get
        {
            return stackSize;
        }

    }

    public Sprite Icon
    {
        get
        {
            return icon;
        }

    }

    protected InventorySlot Slot
    {
        get
        {
            return slot;
        }

        set
        {
            slot = value;
        }
    }
}
