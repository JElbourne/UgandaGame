using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    private static InventoryController instance;

    public static InventoryController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryController>();
            }

            return instance;
        }
    }

    private List<BraceletItem> m_groups = new List<BraceletItem>();

    [SerializeField]
    private InventoryGroupButton[] m_inventoryGroupButtons;

    //For Debugging Purpose
    [SerializeField]
    private ItemObject[] items;

    private void Awake()
    {
        BraceletItem bracelet = (BraceletItem)Instantiate(items[0]);
        bracelet.Instantiate(16);
        bracelet.Use();
    }

    private void Update()
    {
        // For Debugging
        if(Input.GetKeyDown(KeyCode.J))
        {
            BraceletItem bracelet = (BraceletItem)Instantiate(items[0]);
            bracelet.Instantiate(16);
            bracelet.Use();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            BraceletItem bracelet = (BraceletItem)Instantiate(items[0]);
            bracelet.Instantiate(16);
            AddItem(bracelet);
        }
    }

    public void UpdateStackSize(IClickable _clickable)
    {
        if (_clickable.Count == 0)
        {
            _clickable.Icon.color = new Color(0, 0, 0, 0);
        }
    }

    public bool CanAddInventoryGroup
    {
        get
        {
            return m_groups.Count < m_inventoryGroupButtons.Length;
        }
    }

    public void AddGroup(BraceletItem _group)
    {
        Debug.Log("AddGroup() is called");

        foreach (InventoryGroupButton inventoryGroupButton in m_inventoryGroupButtons)
        {
            if(inventoryGroupButton.Bracelet == null)
            {
                inventoryGroupButton.Bracelet = _group;
                m_groups.Add(_group);
                break;
            }
        }
    }

    public void AddItem(ItemObject _item)
    {
        foreach (BraceletItem _group in m_groups)
        {
            if (_group.InventoryGroup.AddItem(_item))
            {
                return;
            }
        }
    }

    public void OpenClose()
    {
        bool groupClosed = m_groups.Find(x => !x.InventoryGroup.IsOpen);

        foreach (BraceletItem _braceletItem in m_groups)
        {
            if (_braceletItem.InventoryGroup.IsOpen != groupClosed)
            {
                _braceletItem.InventoryGroup.OpenClose();
            }
        }
    }
}
