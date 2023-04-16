using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskWander : BehaviorTree.Node
{
    private Transform _transform;
    private Vector3 _targetPosition;
    
    public TaskWander(Transform transform, Vector3 targetPosition)
    {
        _transform = transform;
        _targetPosition = targetPosition;
    }
    public override NodeState Evaluate()
    {
        if (_targetPosition == Vector3.zero)
        {
            _targetPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        }

        _transform.position = Vector3.MoveTowards(_transform.position, _targetPosition, 1 * Time.deltaTime);
        _transform.rotation = Quaternion.LookRotation(_targetPosition - _transform.position);

        if (_transform.position == _targetPosition)
        {
            _targetPosition = Vector3.zero;
            return NodeState.SUCCESS;
        }

        state = NodeState.RUNNING;
        return NodeState.RUNNING;
    }
}