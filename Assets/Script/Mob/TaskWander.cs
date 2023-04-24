using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskWander : BehaviorTree.Node
{
    private Transform _transform;
    private Vector3 _targetPosition = Vector3.zero;
    private Unit _unitPathfinding;
    
    public TaskWander(Transform transform, Unit unitPathfinding)
    {
        _transform = transform;
        _unitPathfinding = unitPathfinding;
    }
    public override NodeState Evaluate()
    {
        if (_targetPosition == Vector3.zero)
        {
            Debug.Log("TEST");
            _targetPosition = new Vector3(Random.Range(-10, 10), 1.0f, Random.Range(-10, 10));
        }

        _unitPathfinding.target = _targetPosition;
        _unitPathfinding.StartCoroutine(_unitPathfinding.UpdatePath());
        //_transform.position = Vector3.MoveTowards(_transform.position, _targetPosition, 1 * Time.deltaTime);
        //_transform.rotation = Quaternion.LookRotation(_targetPosition - _transform.position);
        //Vector3.ClampMagnitude(_targetPosition, 10.0f);
        //Debug.Log(Mathf.Abs(Vector3.Distance(_targetPosition, _transform.position)));
        if (Mathf.Abs(Vector3.Distance(_targetPosition, _transform.position)) < 1f)
        {
            _unitPathfinding.StopAllCoroutines();
            _targetPosition = Vector3.zero;
            return NodeState.SUCCESS;
        }

        state = NodeState.RUNNING;
        return NodeState.RUNNING;
    }
}