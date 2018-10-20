using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public GameObject target;

    EntityController m_controller;
    TraitJump m_jumpTrait;
    TraitLedgeGrab m_ledgeTrait;

	// Use this for initialization
	void Start () {
        m_jumpTrait = target.GetComponent<TraitJump>();
        m_ledgeTrait = target.GetComponent<TraitLedgeGrab>();
        m_controller = target.GetComponent<EntityController>();
	}
	
	// Update is called once per frame
	void Update () {
        // Directional moving
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        m_controller.SetDirectionalInput(directionalInput);

        // Jumping
        if (Input.GetKeyDown (KeyCode.Space)) {
            if (m_ledgeTrait && m_controller.collisions.ledgeIsGrabbed)
            {
                m_ledgeTrait.ClimbLedge();
            } else if (m_jumpTrait) {
                m_jumpTrait.OnJumpInputDown();
            }
        }

        // Early Ending Jump
        if (Input.GetKeyUp(KeyCode.Space)) {
            if (m_jumpTrait) m_jumpTrait.OnJumpInputUp();
        }
	}
}
