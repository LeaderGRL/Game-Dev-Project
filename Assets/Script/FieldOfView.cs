using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range (0, 360)]
    public float angle;
    public LayerMask targetMask;
    public LayerMask obstackeMask;
    public Transform target;

    public bool canSeeTarget;

    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        
        while (true)
        {
            yield return wait;
            CheckTarget();
        }
    }

    private void CheckTarget()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstackeMask))
                    canSeeTarget = true;
                else
                {
                    canSeeTarget = false;
                    target = null;
                }
            }
            else
            {
                canSeeTarget = false;
                target = null;
            }
        }
        else if (canSeeTarget)
        {
            canSeeTarget = false;
            target = null;
        }
    }
}
