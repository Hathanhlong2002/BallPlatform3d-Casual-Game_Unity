using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollow : MonoBehaviour
{
    [SerializeField] public GameObject[] waypoints;
    [SerializeField] public float speed = 1f;

    private int currentWaypointIndex = 0;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    private void FixedUpdate()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        GameObject targetWaypoint = waypoints[currentWaypointIndex];
        if (targetWaypoint == null) return;

        Vector3 currentPos = rb != null ? rb.position : transform.position;
        Vector3 targetPos = targetWaypoint.transform.position;

        if (Vector3.Distance(currentPos, targetPos) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            targetWaypoint = waypoints[currentWaypointIndex];
            if (targetWaypoint == null) return;
            targetPos = targetWaypoint.transform.position;
        }

        Vector3 nextPos = Vector3.MoveTowards(currentPos, targetPos, speed * Time.fixedDeltaTime);
        if (rb != null)
        {
            rb.MovePosition(nextPos);
        }
        else
        {
            transform.position = nextPos;
        }
    }
}
