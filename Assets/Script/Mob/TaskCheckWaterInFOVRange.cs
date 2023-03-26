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
        object t = GetData("target");
        if (t == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Water"))
                {
                    // Faire quelque chose lorsque l'eau est détectée
                }
            }
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
