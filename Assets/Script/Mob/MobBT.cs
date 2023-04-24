using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class MobBT : BehaviorTree.Tree
{
    public Unit unitPathfinding;
    protected override BehaviorTree.Node SetupTree()
    {
        //BehaviorTree.Node root = new TaskWander(transform, Vector2.zero);

        BehaviorTree.Node root = new Selector(new List<BehaviorTree.Node>
        {
            new Sequence(new List<BehaviorTree.Node>
            {
                new TaskCheckWaterInFOVRange(transform),
                new TaskGoToTarget(transform, unitPathfinding),
            }),
            new TaskWander(transform, unitPathfinding)
        });
        
        return root;
    }
}
