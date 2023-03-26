using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class MobBT : BehaviorTree.Tree
{
    protected override BehaviorTree.Node SetupTree()
    {
        BehaviorTree.Node root = new TaskWander(transform, Vector2.zero);

        return root;
    }
}
