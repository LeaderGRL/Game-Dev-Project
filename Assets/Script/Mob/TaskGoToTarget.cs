using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGoToTarget : BehaviorTree.Node
{
    private Transform _transform;

    public TaskGoToTarget(Transform transform)
    {
        _transform = transform;
    }

    public override BehaviorTree.NodeState Evaluate()
    {
        Transform target = GetData("target") as Transform;
        if (target == null)
        {
            Debug.Log("Fail to go to target");
            state = BehaviorTree.NodeState.FAILURE;
            return state;
        }
        
        if (Vector3.Distance(_transform.position, target.position) > 0.01f)
        {
            Debug.Log("Go to target !");
            _transform.position = Vector3.MoveTowards(_transform.position, target.position, 1 * Time.deltaTime);
            _transform.LookAt(target.position);
        }

        state = BehaviorTree.NodeState.RUNNING;
        return state;
    }
}
