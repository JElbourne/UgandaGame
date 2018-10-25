using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class TraitAutoRun : MonoBehaviour {

    EntityController m_controller;
    int dir;

	// Use this for initialization
	void Start () {
        m_controller = GetComponent<EntityController>();
        dir = 1;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (m_controller.collisions.wallSliding) {
            dir = m_controller.collisions.right ? -1:1;
        }
        else if (m_controller.collisions.below) {
            dir = 1;
        }
        m_controller.SetDirectionalInput(new Vector2(dir, 0));
        Debug.Log("Dir: " + dir);
	}
}
