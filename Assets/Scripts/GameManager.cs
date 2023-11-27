using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    CameraRig cameraRig;

    [SerializeField]
    NavMeshSurface surface;

    [SerializeField]
    List<GameObject> mapPrefabs = new List<GameObject>();

    [SerializeField]
    List<GameObject> tempPlayerPrefabs = new List<GameObject>();

    GameObject player1;

    GameObject field;

    void Start()
    {
        #region �׽�Ʈ ����
        //test map generation
        field = Instantiate(mapPrefabs[0]);

        surface.BuildNavMesh();

        //test player generation
        player1 = Instantiate(tempPlayerPrefabs[0], field.transform.Find("PlayerStartPositions").GetChild(0).position, Quaternion.identity);
        cameraRig.transform.position = player1.transform.position;
        cameraRig.target = player1.transform;
        #endregion �׽�Ʈ ����

        //TODO: �޴����� ĳ���ͳ� ���� ������ ������ �޾ƿ��� �ν��Ͻ� ����� �Ҵ��ϴ� ������� �ϼ�.
        //��Ƽ�� ī�޶�� ���߿� ��������.
    }
}
