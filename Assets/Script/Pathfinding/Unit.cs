using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    public float speed = 20;
    public float turnDist = 5;
    //Vector3[] path;
    //int targetIndex;

    Path path;

    void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = new Path(waypoints, transform.position, turnDist);
            //targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        //Vector3 currentWaypoint = path[0];

        while (true)
        {
            //if (transform.position == currentWaypoint)
            //{
            //    targetIndex++;
            //    if (targetIndex >= path.Length)
            //    {
            //        targetIndex = 0;
            //        path = new Vector3[0];
            //        yield break;
            //    }

            //    currentWaypoint = path[targetIndex];
            //}

            //transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
            
            //for (int i = targetIndex; i < path.Length; i++)
            //{
            //    Gizmos.color = Color.black;
            //    Gizmos.DrawCube(path[i], Vector3.one);

            //    if (i == targetIndex)
            //    {
            //        Gizmos.DrawLine(transform.position, path[i]);
            //    }
            //    else
            //    {
            //        Gizmos.DrawLine(path[i - 1], path[i]);
            //    }
            //}
        }
    }
}
