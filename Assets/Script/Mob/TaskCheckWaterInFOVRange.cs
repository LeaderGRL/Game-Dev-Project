using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskCheckWaterInFOVRange : BehaviorTree.Node
{
    private Transform transform;

    public TaskCheckWaterInFOVRange(Transform transform)
    {
        this.transform = transform;
    }

    public override BehaviorTree.NodeState Evaluate()
    {
        FieldOfView FOV = transform.GetComponent<FieldOfView>();
        FOV.SetTargetMask("Water");
        
        object t = GetData("target");
        if (t != null)
        {
            Debug.Log("Already have a target");
            state = NodeState.SUCCESS;
            return state;
        }

        if (FOV.CanSeeTarget)
        {
            Debug.Log("Find water");
            parent.parent.SetData("target", FOV.targetClosestPoint);
            state = NodeState.SUCCESS;
            return state;
        }

        if (state != NodeState.RUNNING)
        {
            Debug.Log("Pass to running state");
            state = NodeState.RUNNING;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
