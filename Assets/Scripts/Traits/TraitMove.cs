using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class TraitMove : MonoBehaviour {

    EntityController m_controller;

	void Start()
	{
        m_controller = GetComponent<EntityController>();
	}

	public void Move(Vector2 moveAmount, bool standingOnPlatform)
    {
        Move(moveAmount, Vector2.zero, standingOnPlatform);
    }

    public void Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false)
    {
        m_controller.UpdateRaycastOrigins();
        m_controller.collisions.Reset();
        m_controller.collisions.moveAmountOld = moveAmount;
        m_controller.directionalInput = input;

        if (moveAmount.y < 0)
        {
            DecendSlope(ref moveAmount);
        }

        if (moveAmount.x != 0)
        {
            m_controller.collisions.faceDir = (int)Mathf.Sign(moveAmount.x);
        }

        m_controller.HorizontalCollisions(ref moveAmount);

        if (moveAmount.y != 0)
        {
            m_controller.VerticalCollisions(ref moveAmount);
        }

        if (m_controller.collisions.ledgeIsGrabbed) {
            moveAmount.y = 0;
        }

        transform.Translate(moveAmount);

        if (standingOnPlatform)
        {
            m_controller.collisions.below = true;
        }
    }

    public void ClimbSlope(ref Vector2 moveAmount, float slopeAngle, Vector2 slopeNormal)
    {
        float moveDistance = Mathf.Abs(moveAmount.x);
        float climbmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        if (moveAmount.y <= climbmoveAmountY)
        {
            moveAmount.y = climbmoveAmountY;
            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
            m_controller.collisions.below = true;
            m_controller.collisions.climbingSlope = true;
            m_controller.collisions.slopeAngle = slopeAngle;
            m_controller.collisions.slopeNormal = slopeNormal;
        }
    }

    void DecendSlope(ref Vector2 moveAmount)
    {

        RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(m_controller.raycastOrigins.bottomLeft, Vector2.down, Mathf.Abs(moveAmount.y) + m_controller.skinWidth, m_controller.collisionMask);
        RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(m_controller.raycastOrigins.bottomRight, Vector2.down, Mathf.Abs(moveAmount.y) + m_controller.skinWidth, m_controller.collisionMask);

        if (maxSlopeHitLeft ^ maxSlopeHitRight)
        {
            SlideDownMaxSlope(maxSlopeHitLeft, ref moveAmount);
            SlideDownMaxSlope(maxSlopeHitRight, ref moveAmount);
        }

        if (!m_controller.collisions.slidingDownMaxSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            Vector2 rayOrigin = (directionX == -1) ? m_controller.raycastOrigins.bottomRight : m_controller.raycastOrigins.bottomLeft;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, m_controller.collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != 0f && slopeAngle <= m_controller.maxSlopeAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == directionX)
                    {
                        if (hit.distance - m_controller.skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x))
                        {
                            float moveDistance = Mathf.Abs(moveAmount.x);
                            float decendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                            moveAmount.y -= decendmoveAmountY;

                            m_controller.collisions.slopeAngle = slopeAngle;
                            m_controller.collisions.decendingSlope = true;
                            m_controller.collisions.below = true;
                            m_controller.collisions.slopeNormal = hit.normal;
                        }
                    }
                }
            }
        }
    }


    void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 moveAmount)
    {
        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle > m_controller.maxSlopeAngle)
            {
                moveAmount.x = hit.normal.x * (Mathf.Abs(moveAmount.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

                m_controller.collisions.slopeAngle = slopeAngle;
                m_controller.collisions.slidingDownMaxSlope = true;
                m_controller.collisions.slopeNormal = hit.normal;
            }
        }
    }
}
