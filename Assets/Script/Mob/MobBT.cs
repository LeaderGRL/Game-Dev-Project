using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class MobBT : BehaviorTree.Tree
{
    protected override BehaviorTree.Node SetupTree()
    {
        //BehaviorTree.Node root = new TaskWander(transform, Vector2.zero);

        BehaviorTree.Node root = new Selector(new List<BehaviorTree.Node>
        {
            new Sequence(new List<BehaviorTree.Node>
            {
                new TaskCheckWaterInFOVRange(transform),
                new TaskGoToTarget(transform),
            }),
            new TaskWander(transform, Vector2.zero)
        });
        
        return root;
    }
}
