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

        //TODO: ����)Ŭ�� ���� ��ǲ�� ���� �����ϰ�, ���հ��� ������Ʈ���� ��ǲ�� �̵��� ����.
        //AI�� ��� AI�� ���հ��� ������Ʈ���� ��ü�� �Ǵ� ���� �̵��ϵ��� �����.
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
