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

    private List<BraceletItem> m_bracelets = new List<BraceletItem>();

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
    }

    public bool CanAddInventoryGroup
    {
        get
        {
            return m_bracelets.Count < m_inventoryGroupButtons.Length;
        }
    }

    public void AddGroup(BraceletItem _bracelet)
    {
        foreach (InventoryGroupButton inventoryGroupButton in m_inventoryGroupButtons)
        {
            if(inventoryGroupButton.Bracelet == null)
            {
                inventoryGroupButton.Bracelet = _bracelet;
                m_bracelets.Add(_bracelet);
                break;
            }
        }
    }
}
