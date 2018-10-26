using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGroupButton : MonoBehaviour {

    private BraceletItem m_bracelet;

    [SerializeField]
    private Sprite full, empty;

    public BraceletItem Bracelet {
        get
        {
            return m_bracelet;
        }
        set
        {
            if(value != null)
            {
                GetComponent<Image>().sprite = full;
            }
            else
            {
                GetComponent<Image>().sprite = empty;
            }
            m_bracelet = value;
        }
    }
}
