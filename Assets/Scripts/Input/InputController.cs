using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public GameObject target;

    EntityController m_controller;
    TraitJump m_jumpTrait;
    TraitLedgeGrab m_ledgeTrait;
    TraitCrouch m_crouchTrait;

	// Use this for initialization
	void Start () {
        m_jumpTrait = target.GetComponent<TraitJump>();
        m_crouchTrait = target.GetComponent<TraitCrouch>();
        m_ledgeTrait = target.GetComponent<TraitLedgeGrab>();
        m_controller = target.GetComponent<EntityController>();
	}

    // Update is called once per frame
    void Update() {

        CheckInputForUI();

        // Directional moving
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        m_controller.SetDirectionalInput(directionalInput);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space)) {
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

        // Crouching
        if (Input.GetButtonDown("Crouch"))
        {
            if (m_crouchTrait) m_crouchTrait.OnCrouchInputDown();
        }
        if (Input.GetButtonUp("Crouch"))
        {
            if (m_crouchTrait) m_crouchTrait.OnCrouchInputUp();
        }
    }

    private void CheckInputForUI()
    {
        // Open all Inventory Groups
        if (Input.GetKeyUp(KeyCode.Q))
        {
            InventoryController.Instance.OpenClose();
        }

    }
}
