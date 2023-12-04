using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    NavMeshAgent agent;

    public bool isMovable = true;

    float DASH_SPEED = 4f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!isMovable)
        {
            return;
        }

        //TODO: 나중)클릭 등의 인풋은 따로 관리하고, 통합관리 컴포넌트에서 인풋과 이동을 연결.
        //AI의 경우 AI용 통합관리 컴포넌트에서 자체적 판단 이후 이동하도록 만들것.
    }

    public void MoveTo(Vector3 destination)
    {
        if (isMovable)
        {
            agent.SetDestination(destination);
        }
    }

    public void CancelMove()
    {
        agent.SetDestination(gameObject.transform.position);
        agent.velocity = Vector3.zero;
    }

    public void DisableMovement()
    {
        isMovable = false;
        CancelMove();
        agent.isStopped = true;
    }

    public void EnableMovement()
    {
        isMovable = true;
        agent.isStopped = false;
        agent.velocity = Vector3.zero;
    }

    public void GetKnuckBack(Vector3 direction)
    {
        DisableMovement();
        agent.Move(direction * KnuckBack.KNUCKBACK_SPEED);
        agent.velocity = Vector3.zero;
        agent.SetDestination(transform.position);
        EnableMovement();
    }

    public void Dash(Vector3 direction)
    {
        DisableMovement();
        agent.Move(direction * DASH_SPEED);
        agent.velocity = Vector3.zero;
        agent.SetDestination(transform.position);
        EnableMovement();
    }
}
