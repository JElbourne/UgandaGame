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
    bool m_crouchPressed;

    public float CrouchHeightPercentage = 0.5f;
    public float CrouchMoveSpeedPercentage = 0.50f;

    // Use this for initialization
    void Start () {
        m_controller = GetComponent<EntityController>();
        m_collider = GetComponent<BoxCollider2D>();

        m_baseSize = m_collider.size;
        m_baseOffset = m_collider.offset;

        m_baseTestSpriteScale = TestSprite.transform.localScale;
        m_baseTestSpritePosition = TestSprite.transform.localPosition;

        m_crouchPressed = false;
    }

    private void Update()
    {
        if(m_crouchPressed == false && m_controller.collisions.isCrouching)
        {
            // Try to stand up
            // 1st check if there is enough room to stand up
            bool hasClearance = CheckHasHeightClearance();

            // If the character has room, stand Up.
            if (hasClearance) StandUp();
        }
    }

    // Update is called once per frame
    public void OnCrouchInputDown () {
        // Set the local crouch flag
        m_crouchPressed = true;

        // If the character is not already crouch we will crouch.
        if (m_controller && !m_controller.collisions.isCrouching)
        {
            m_controller.collisions.isCrouching = true;

            if (m_collider)
            {
                m_collider.size = new Vector2(1, m_baseSize.y * CrouchHeightPercentage);
                m_collider.offset = new Vector2(0, m_baseOffset.y - (m_baseSize.y * (CrouchHeightPercentage/2)));
                m_controller.CalculateRaySpacing();

                //For Testing
                TestSprite.transform.localScale = new Vector3(m_baseTestSpriteScale.x, m_baseTestSpriteScale.y * 0.5f, m_baseTestSpriteScale.z);
                TestSprite.transform.localPosition = new Vector3(m_baseTestSpritePosition.x, -(m_baseTestSpriteScale.y * 0.25f), m_baseTestSpritePosition.z);
            }
        }
    }

    public void OnCrouchInputUp()
    {
        if (m_crouchPressed)
        {
            m_crouchPressed = false;
        }
    }

    private void StandUp()
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

    private bool CheckHasHeightClearance()
    {
        float m_rayLength = m_baseSize.y * (1 - CrouchHeightPercentage);
        bool m_hasHit = false;
        for (int i = 0; i < m_controller.verticalRayCount; i++)
        {
            Vector2 m_rayOrigin = m_controller.raycastOrigins.topLeft;
            m_rayOrigin += Vector2.right * (m_controller.verticalRaySpacing * i);
            RaycastHit2D m_hit = Physics2D.Raycast(m_rayOrigin, Vector2.up, m_rayLength, m_controller.collisionMask);

            Debug.DrawRay(
                m_rayOrigin,
                Vector2.up,
                Color.blue);

            if (m_hit)
            {
                m_hasHit = true;
                break;
            }
        }

        return (m_hasHit) ? false : true;
    }

}
