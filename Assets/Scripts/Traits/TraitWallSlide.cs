using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityController))]
[RequireComponent(typeof(TraitPhysics))]
public class TraitWallSlide : MonoBehaviour {

    public float wallStickTime = .25f;
    public float wallSlideSpeedMax = 3;

    EntityController m_controller;
    TraitPhysics m_physics;

    int wallDirX;
    float timeToWallUnstick;

    void Awake()
    {
        m_controller = GetComponent<EntityController>();
        m_physics = GetComponent<TraitPhysics>();
    }

    public void HandleWallSliding()
    {
        wallDirX = (m_controller.collisions.left) ? -1 : 1;

        m_controller.collisions.wallSliding = false;

        if ((m_controller.collisions.left || m_controller.collisions.right) && !m_controller.collisions.below && m_physics.velocity.y < 0)
        {
            m_controller.collisions.wallSliding = true;

            if (m_physics.velocity.y < -wallSlideSpeedMax)
            {
                m_physics.velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                m_physics.velocity.x = 0;

                if (m_controller.directionalInput.x != wallDirX && m_controller.directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }

    }
}
