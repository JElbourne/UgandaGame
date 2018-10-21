using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class TraitPhysics : MonoBehaviour {

    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;

    [HideInInspector]
    public Vector2 velocity;
    [HideInInspector]
    public float gravity;
    public float maxJumpHeight = 4;
    public float timeToJumpApex = .4f;
    float velocityXSmoothing;

    EntityController m_controller;

	private void Awake()
	{
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
	}

	void Start() {
        m_controller = GetComponent<EntityController>();
    }

	void Update()
	{
        if (m_controller.collisions.above || m_controller.collisions.below)
        {
            if (m_controller.collisions.slidingDownMaxSlope)
            {
                velocity.y += m_controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }
	}

	public void CalculateVelocity()
    {
        float targetVelocityX = m_controller.directionalInput.x * m_controller.CalculateMoveSpeed();
        velocity.x = Mathf.SmoothDamp(
            velocity.x,
            targetVelocityX,
            ref velocityXSmoothing,
            (m_controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }
}
