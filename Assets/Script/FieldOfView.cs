using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius = 5f;
    [Range(0, 360)] public float angle = 90f;

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    public Transform target { get; private set; }

    public bool CanSeeTarget { get; private set; }

    private void Awake()
    {
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            CheckTarget();
        }
    }

    private void CheckTarget()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask); // Check for target in FOV Radius
        
        if (rangeChecks.Length == 0)
        {
            if (CanSeeTarget)
            {
                CanSeeTarget = false;
                target = null;
            }
            return;
        }

        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (Collider targetCollider in rangeChecks) // Iterate over all potential targets.
        {
            Vector3 closestPoint = targetCollider.ClosestPoint(transform.position); // Calculate the closest point on the target's surface to the player's position
            Transform potentialTarget = targetCollider.transform;
            Vector3 directionToTarget = (closestPoint - transform.position).normalized;
            Debug.Log(Vector3.Angle(transform.forward, directionToTarget) + " : " + angle/2);
            Debug.Log("Closest position : " + closestPoint);
            var DEB = GameObject.Find("DEB");
            DEB.transform.position = closestPoint;
            
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2) // Check if the target is within the FOV angle.
            {
                
                float distanceToTarget = Vector3.Distance(transform.position, closestPoint);
                //directionToTarget = (closestPoint - transform.position).normalized;

                if (distanceToTarget < closestDistance) // If the target is the closest yet and not obstructed, set as new target.
                {
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                    {
                        Debug.Log("TEST");
                        closestTarget = potentialTarget;
                        closestDistance = distanceToTarget;
                    }
                }
                
            }

            if (closestTarget != null)
            {
                if (!CanSeeTarget)
                {
                    CanSeeTarget = true;
                }
                target = closestTarget;
                Debug.Log(target.name);

            }
            else
            {
                if (CanSeeTarget)
                {
                    CanSeeTarget = false;
                    target = null;
                }
            }

        }
    }
}
