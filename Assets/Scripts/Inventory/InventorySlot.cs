using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IClickable {

    private Stack<ItemObject> m_items = new Stack<ItemObject>();

    [SerializeField]
    private Image m_icon;

    public bool IsEmpty
    {
        get
        {
            return m_items.Count == 0;
        }
    }

    public ItemObject Item
    {
        get
        {
            if (!IsEmpty)
            {
                return m_items.Peek();
            }

            return null;
        }
    }

    public Image Icon
    {
        get
        {
            return m_icon;
        }

        set
        {
            m_icon = value;
        }
    }

    public int Count
    {
        get
        {
            return m_items.Count;
        }
    }

    public bool AddItem(ItemObject _item)
    {
        m_items.Push(_item);
        m_icon.sprite = _item.Icon;
        m_icon.color = Color.white;
        _item.Slot = this;
        return true;
    }

    public void RemoveItem(ItemObject _item)
    {
        if(!IsEmpty)
        {
            m_items.Pop();
            InventoryController.Instance.UpdateStackSize(this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (Item is IUseable)
        {
            (Item as IUseable).Use();
        }
    }
}
