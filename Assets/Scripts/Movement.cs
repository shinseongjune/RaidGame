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
        //TODO: 나중)클릭 등의 인풋은 따로 관리하고, 통합관리 컴포넌트에서 인풋과 이동을 연결.
        //AI의 경우 AI용 통합관리 컴포넌트에서 자체적 판단 이후 이동하도록 만들것.
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
