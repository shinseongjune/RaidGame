using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    NavMeshAgent agent;
    ControlComponent control;

    public bool isMovable = true;

    float DASH_SPEED = 4f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        control = GetComponent<ControlComponent>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, agent.destination) < agent.stoppingDistance)
        {
            CancelMove();
        }
    }

    public void MoveTo(Vector3 destination)
    {
        if (isMovable)
        {
            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(destination, out navMeshHit, 100f, NavMesh.AllAreas))
            {
                agent.isStopped = false;
                agent.SetDestination(navMeshHit.position);
            }
        }
    }

    public void CancelMove()
    {
        agent.SetDestination(transform.position);
        agent.isStopped = true;
        control.EndMovement();
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
        CancelMove();
        agent.isStopped = false;
    }

    public void GetKnockBack(Vector3 direction)
    {
        DisableMovement();
        agent.Move(direction * KnockBack.KnockBack_SPEED);
        agent.velocity = Vector3.zero;
        agent.SetDestination(transform.position);
        EnableMovement();
        control.EndMovement();
    }

    public void Dash(Vector3 direction)
    {
        if (isMovable)
        {
            DisableMovement();
            agent.Move(direction * DASH_SPEED);
            agent.SetDestination(transform.position);
            agent.velocity = Vector3.zero;
            EnableMovement();
            control.EndMovement();
        }
    }
}
