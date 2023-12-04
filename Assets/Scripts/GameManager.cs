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

    [SerializeField]
    GameObject inputHandlerPrefab;

    Player player;
    InputHandler inputHandler;

    GameObject playerCharacter;

    GameObject field;

    void Start()
    {
        #region �׽�Ʈ ����
        //test map generation
        field = Instantiate(mapPrefabs[0]);

        surface.BuildNavMesh();

        //test player generation
        player = new Player();
        inputHandler = Instantiate(inputHandlerPrefab).GetComponent<InputHandler>();

        playerCharacter = Instantiate(tempPlayerPrefabs[0], field.transform.Find("PlayerStartPositions").GetChild(0).position, Quaternion.identity);
        cameraRig.transform.position = playerCharacter.transform.position;
        cameraRig.target = playerCharacter.transform;
        
        player.character = playerCharacter.GetComponent<CharacterControlComponent>();
        player.inputHandler = inputHandler;
        inputHandler.player = player;
        #endregion �׽�Ʈ ����

        //TODO: ����)�޴����� ĳ���ͳ� ���� ������ ������ �޾ƿ��� �ν��Ͻ� ����� �Ҵ��ϴ� ������� �ϼ�.
        //��Ƽ�� ī�޶�� ���߿� ��������.
    }
}
