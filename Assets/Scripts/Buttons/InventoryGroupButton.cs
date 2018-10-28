using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryGroupButton : MonoBehaviour, IPointerClickHandler {

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_bracelet != null)
        {
            m_bracelet.InventoryGroup.OpenClose();
        }
    }
}
