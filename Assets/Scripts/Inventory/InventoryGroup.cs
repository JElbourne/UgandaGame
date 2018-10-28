using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGroup : MonoBehaviour {

    [SerializeField]
    private GameObject slotPrefab;

    private CanvasGroup m_canvasGroup;

    private List<InventorySlot> m_slots = new List<InventorySlot>();

    public bool IsOpen
    {
        get
        {
            return m_canvasGroup.alpha > 0;
        }
    }

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            InventorySlot _slot = Instantiate(slotPrefab, transform).GetComponent<InventorySlot>();
            m_slots.Add(_slot);
        }
    }

    public bool AddItem(ItemObject _item)
    {
        foreach(InventorySlot _slot in m_slots)
        {
            if(_slot.IsEmpty)
            {
                _slot.AddItem(_item);
                return true;
            }
        }
        return false;
    }

    public void OpenClose()
    {
        m_canvasGroup.alpha = m_canvasGroup.alpha > 0 ? 0 : 1;

        m_canvasGroup.blocksRaycasts = m_canvasGroup.blocksRaycasts == true ? false : true;
    }
}
