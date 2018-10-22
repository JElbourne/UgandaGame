using UnityEngine;

[RequireComponent(typeof(TraitPhysics))]
public class EntityController : RaycastController {
    
    public float moveSpeed = 6;
    public float maxSlopeAngle = 60;
    public CollisionInfo collisions;

    [HideInInspector]
    public Vector2 directionalInput;

    TraitPhysics m_physics;
    TraitMove m_moveTrait;
    TraitCrouch m_crouchTrait;
    TraitWallSlide m_wallSlide;
    TraitLedgeGrab m_ledgeGrab;

    public override void Awake()
    {
        base.Awake();
        m_physics = GetComponent<TraitPhysics>();
        m_moveTrait = GetComponent<TraitMove>();
        m_crouchTrait = GetComponent<TraitCrouch>();
        m_wallSlide = GetComponent<TraitWallSlide>();
    }

    public override void Start()
    {
        base.Start();
        collisions.faceDir = 1;
    }

    void Update()
    {
        // Determine Velocity to start
        m_physics.CalculateVelocity();

        if (m_wallSlide) m_wallSlide.HandleWallSliding();
        // Move last in the order
        if (m_moveTrait) m_moveTrait.Move(m_physics.velocity * Time.deltaTime, directionalInput);
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public float CalculateMoveSpeed()
    {
        if (m_crouchTrait && collisions.isCrouching)
            return moveSpeed * m_crouchTrait.CrouchSpeedPercentage;
        return moveSpeed;
    }

    public void HorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        if (Mathf.Abs(moveAmount.x) < skinWidth) {
            rayLength = 2 * skinWidth;
        }

        int hitCount = 0;
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(
                rayOrigin,
                Vector2.right * directionX,
                Color.red);
            
            if (hit)
            {
                if (hit.distance == 0) {
                    continue;
                }

                hitCount++;

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if(i == 0 && slopeAngle <= maxSlopeAngle) {
                    if (collisions.decendingSlope) {
                        collisions.decendingSlope = false;
                        moveAmount = collisions.moveAmountOld;
                    }

                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collisions.slopeAngleOld) {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }
                    m_moveTrait.ClimbSlope(ref moveAmount, slopeAngle, hit.normal);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle) {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope) {
                        moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad * Mathf.Abs(moveAmount.x));
                    }

                    if (hitCount > Mathf.FloorToInt(horizontalRayCount * 0.5f))
                    {
                        collisions.left = directionX == -1;
                        collisions.right = directionX == 1;
                    }
                }

                if (i == horizontalRayCount-1 && collisions.ledgeIsGrabbed) {
                    //collisions.ledgeIsGrabbed = false;
                }

            } else if (hitCount == horizontalRayCount - 1 &&
                        m_physics.velocity.y < 0) {
                collisions.ledgeIsGrabbed = true;
            }

        }

        if (!collisions.left && !collisions.right && collisions.ledgeIsGrabbed) {
            //collisions.ledgeIsGrabbed = false;
        }
    }

    public void VerticalCollisions(ref Vector2 moveAmount) {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(
                rayOrigin,
                Vector2.up * directionY,
                Color.red);

            if (hit) {
                if (hit.collider.tag == "Through") {
                    if (directionY == 1 || hit.distance == 0) {
                        continue;
                    }
                    if (collisions.fallingThroughPlatform) {
                        continue;
                    }
                    if (directionalInput.y == -1) {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", .5f);
                        continue;
                    }
                }

                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (collisions.climbingSlope) {
                    moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad * Mathf.Sign(moveAmount.x));

                }
                collisions.below = directionY == -1f;
                collisions.above = directionY == 1f;
            }
        }

        if (collisions.climbingSlope) {
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit) {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle) {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                    collisions.slopeNormal = hit.normal;
                }
            }
        }
    }

    void ResetFallingThroughPlatform() {
        collisions.fallingThroughPlatform = false;
    }

    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;

        public bool isJumping;
        public bool ledgeIsGrabbed;
        public bool wallSliding;
        public bool climbingSlope;
        public bool decendingSlope;
        public bool slidingDownMaxSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector2 slopeNormal;
        public Vector2 moveAmountOld;
        public int faceDir;
        public bool fallingThroughPlatform;
        public bool isCrouching;

        public void Reset() {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            decendingSlope = false;
            slidingDownMaxSlope = false;
            slopeNormal = Vector2.zero;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }

    }

}
