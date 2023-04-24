using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGoToTarget : BehaviorTree.Node
{
    private Transform _transform;
    private Unit _unitPathFinding;

    public TaskGoToTarget(Transform transform, Unit unitPathFinding)
    {
        _transform = transform;
        _unitPathFinding = unitPathFinding;
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
        
        if (Vector3.Distance(_transform.position, target.GetComponent<Collider>().ClosestPoint(_transform.position)) > 0.01f)
        {
            Debug.Log("Go to target !");
            _unitPathFinding.StopAllCoroutines();
            _unitPathFinding.target = target.GetComponent<Collider>().ClosestPoint(_transform.position);
            _unitPathFinding.StartCoroutine(_unitPathFinding.UpdatePath());
            //_transform.position = Vector3.MoveTowards(_transform.position, target.GetComponent<Collider>().ClosestPoint(_transform.position), 1 * Time.deltaTime);
            //_transform.LookAt(target.GetComponent<Collider>().ClosestPoint(_transform.position));
        }
        else
        {
            BehaviorTree.Node child = new BehaviorTree.Sequence(new List<BehaviorTree.Node>{
                new TaskWander(_transform, _unitPathFinding),
            });
        }

        state = BehaviorTree.NodeState.RUNNING;
        return state;
    }
}
