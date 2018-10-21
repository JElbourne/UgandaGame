using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityController))]
[RequireComponent(typeof(BoxCollider2D))]
public class TraitCrouch : MonoBehaviour {

    public GameObject TestSprite;
    Vector3 m_baseTestSpriteScale;
    Vector3 m_baseTestSpritePosition;

    EntityController m_controller;
    BoxCollider2D m_collider;
    Vector2 m_baseSize;
    Vector2 m_baseOffset;


    // Use this for initialization
    void Start () {
        m_controller = GetComponent<EntityController>();
        m_collider = GetComponent<BoxCollider2D>();

        m_baseSize = m_collider.size;
        m_baseOffset = m_collider.offset;

        m_baseTestSpriteScale = TestSprite.transform.localScale;
        m_baseTestSpritePosition = TestSprite.transform.localPosition;
    }
	
	// Update is called once per frame
	public void OnCrouchInputDown () {
        if (!m_controller.collisions.isCrouching)
        {
            if (m_controller)
                m_controller.collisions.isCrouching = true;

            if (m_collider)
            {
                m_collider.size = new Vector2(1, m_baseSize.y * 0.5f);
                m_collider.offset = new Vector2(0, m_baseOffset.y - (m_baseSize.y * 0.25f));
                m_controller.CalculateRaySpacing();

                //For Testing
                TestSprite.transform.localScale = new Vector3(m_baseTestSpriteScale.x, m_baseTestSpriteScale.y * 0.5f, m_baseTestSpriteScale.z);
                TestSprite.transform.localPosition = new Vector3(m_baseTestSpritePosition.x, -(m_baseTestSpriteScale.y * 0.25f), m_baseTestSpritePosition.z);
            }
        }
    }

    public void OnCrouchInputUp()
    {
        if (m_controller.collisions.isCrouching)
        {
            m_controller.collisions.isCrouching = false;
            if (m_collider)
            {
                m_collider.size = m_baseSize;
                m_collider.offset = m_baseOffset;
                m_controller.CalculateRaySpacing();

                //For Testing
                TestSprite.transform.localScale = new Vector3(m_baseTestSpriteScale.x, m_baseTestSpriteScale.y, m_baseTestSpriteScale.z);
                TestSprite.transform.localPosition = new Vector3(m_baseTestSpritePosition.x, m_baseTestSpritePosition.y, m_baseTestSpritePosition.z);

            }
        }
    }
}
