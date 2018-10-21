using UnityEngine;

[RequireComponent(typeof(TraitPhysics))]
[RequireComponent(typeof(EntityController))]
public class TraitJump : MonoBehaviour {
    
    public Vector2 wallJumpClimb = new Vector2(7.5f, 16f);
    public Vector2 wallJumpOff = new Vector2(8.5f, 7f);
    public Vector2 wallLeap = new Vector2(18f, 17f);

    public float fastFalling = 1.25f;
    public float minJumpHeight = 1;

    float maxJumpVelocity;
    float minJumpVelocity;
    int wallDirX;

    EntityController m_controller;
    TraitPhysics m_physics;

	void Start()
	{
        m_controller = GetComponent<EntityController>();
        m_physics = GetComponent<TraitPhysics>();

        maxJumpVelocity = Mathf.Abs(m_physics.gravity) * m_physics.timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(m_physics.gravity) * minJumpHeight);
	}

	void Update()
	{
        if (m_physics.velocity.y < 0 && m_controller.collisions.isJumping) {
            m_physics.velocity.y -= fastFalling;
        }
        if (m_controller.collisions.below) {
            m_controller.collisions.isJumping = false;
        }
	}

	public void OnJumpInputDown()
    {
        // Player can not jump if crouching.
        if (m_controller.collisions.isCrouching) return;

        wallDirX = (m_controller.collisions.left) ? -1 : 1;

        // Jumping when sliding on a wall
        if (m_controller.collisions.wallSliding)
        {
            if (wallDirX == m_controller.directionalInput.x)
            {
                m_physics.velocity.x = -wallDirX * wallJumpClimb.x;
                m_physics.velocity.y = wallJumpClimb.y;
            }
            else if (m_controller.directionalInput.x == 0)
            {
                m_physics.velocity.x = -wallDirX * wallJumpOff.x;
                m_physics.velocity.y = wallJumpOff.y;
            }
            else
            {
                m_physics.velocity.x = -wallDirX * wallLeap.x;
                m_physics.velocity.y = wallLeap.y;
            }
        }

        // Jumping while standing on something
        if (m_controller.collisions.below)
        {
            // Change the angle of the jump if sliding down a steep slope
            if (m_controller.collisions.slidingDownMaxSlope)
            {
                if (m_controller.directionalInput.x != -Mathf.Sign(m_controller.collisions.slopeNormal.x))
                { // not jumping against max slope
                    m_physics.velocity.y = maxJumpVelocity * m_controller.collisions.slopeNormal.y;
                    m_physics.velocity.x = maxJumpVelocity * m_controller.collisions.slopeNormal.x;
                }
            }
            else
            {
                m_physics.velocity.y = maxJumpVelocity;

            }
            m_controller.collisions.isJumping = true;
        }
    }

    // Variable height jumping, (release jump input before max jump height)
    public void OnJumpInputUp()
    {
        if (m_physics.velocity.y > minJumpVelocity)
        {
            m_physics.velocity.y = minJumpVelocity;

        }
    }
}
