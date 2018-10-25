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

    //For Debugging Purpose
    [SerializeField]
    private ItemObject[] items;

    private void Awake()
    {
        BraceletItem bracelet = (BraceletItem)Instantiate(items[0]);
        bracelet.Instantiate(16);
        bracelet.Use();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
