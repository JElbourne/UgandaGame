using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class TraitLedgeGrab : MonoBehaviour {

    EntityController m_controller;
    Vector3 endPosition;
    Vector3 startPosition;
    Vector3[] climbWaypoints;
    Bounds bounds;
    bool climbing;

    int fromWaypointIndex;
    float percentageBetweenWaypoints;
    float speed = 5f;

    void Start()
    {
        m_controller = GetComponent<EntityController>();
        bounds = m_controller.collider.bounds;
        climbWaypoints = new Vector3[3];
    }

	private void Update()
	{
        if (m_controller.collisions.ledgeIsGrabbed && climbing) {
            fromWaypointIndex %= climbWaypoints.Length;
            int toWaypointIndex = (fromWaypointIndex + 1) % climbWaypoints.Length;
            float distanceBetweenWaypoints = Vector3.Distance(climbWaypoints[fromWaypointIndex], climbWaypoints[toWaypointIndex]);
            percentageBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
            percentageBetweenWaypoints = Mathf.Clamp01(percentageBetweenWaypoints);
            transform.position = Vector3.Lerp(climbWaypoints[fromWaypointIndex], climbWaypoints[toWaypointIndex], percentageBetweenWaypoints);
            if (percentageBetweenWaypoints >= 1f)
            {
                fromWaypointIndex++;
                percentageBetweenWaypoints = 0f;

                if (fromWaypointIndex == climbWaypoints.Length - 1) {
                    fromWaypointIndex = 0;
                    climbing = false;
                    m_controller.collisions.ledgeIsGrabbed = false;
                }
            }

        }
	}

	public void ClimbLedge() {
        percentageBetweenWaypoints = 0f;
        fromWaypointIndex = 0;
        float climbHorizontal = bounds.max.x - bounds.min.x;
        float climbVertical = bounds.max.y - bounds.min.y;
        float direction = m_controller.collisions.left ? -1 : 1;
        Vector3 pos = transform.position;
        climbWaypoints[0] = new Vector3(pos.x, pos.y, pos.z);
        climbWaypoints[1] = new Vector3(pos.x, pos.y + climbVertical, pos.z);
        climbWaypoints[2] = new Vector3(pos.x + climbHorizontal * direction, pos.y + climbVertical, pos.z);
        // Do this last so update does not start before setting waypoints.
        climbing = true;
    }
}
