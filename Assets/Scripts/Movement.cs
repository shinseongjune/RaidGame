using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    NavMeshAgent agent;

    bool isMovable = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //TODO: ����)Ŭ�� ���� ��ǲ�� ���� �����ϰ�, ���հ��� ������Ʈ���� ��ǲ�� �̵��� ����.
        //AI�� ��� AI�� ���հ��� ������Ʈ���� ��ü�� �Ǵ� ���� �̵��ϵ��� �����.
        if (Input.GetMouseButton(1) && isMovable)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("MouseInputCollider")))
            {
                MoveTo(hit.point);
            }
        }
    }

    public void MoveTo(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void CancelMove()
    {
        agent.SetDestination(gameObject.transform.position);
    }

    public void DisableMovement()
    {
        isMovable = false;
        CancelMove();
    }
    public void EnableMovement()
    {
        isMovable = true;
    }
}
