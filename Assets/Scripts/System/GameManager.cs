using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverBackground;
    public GameObject winImage;
    public GameObject loseImage;
    public GameObject titleButton;
    public GameObject gameCanvas;

    [SerializeField]
    CameraRig cameraRig;

    [SerializeField]
    NavMeshSurface surface;

    [SerializeField]
    List<GameObject> mapPrefabs = new List<GameObject>();

    [SerializeField]
    List<GameObject> tempPlayerPrefabs = new List<GameObject>();

    [SerializeField]
    GameObject tempBossPrefab;

    [SerializeField]
    GameObject inputHandlerPrefab;

    Player player;
    InputHandler inputHandler;

    GameObject playerCharacter;

    GameObject field;

    CharacterControlComponent playerControl;
    TempBossControlComponent bossControl;

    public SliderValueSetter playerHPBar;
    public SliderValueSetter playerMPBar;
    public SliderValueSetter bossHPBar;

    public SkillIconCooldownSetter qCoolSetter;
    public SkillIconCooldownSetter wCoolSetter;
    public SkillIconCooldownSetter eCoolSetter;
    public DashIconCooldownSetter dashCoolSetter;

    bool isGameOver = false;

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
        playerControl = player.character;
        player.inputHandler = inputHandler;
        inputHandler.player = player;

        Stats playerStats = playerCharacter.GetComponent<Stats>();
        playerHPBar.targetStats = playerStats;
        playerHPBar.targetType = Stat.Type.MaxHP;
        playerMPBar.targetStats = playerStats;
        playerMPBar.targetType = Stat.Type.MaxMP;

        SkillSlots slots = playerControl.skillSlots;
        slots.AssignTempSkill();
        qCoolSetter.slot = slots.q;
        wCoolSetter.slot = slots.w;
        eCoolSetter.slot = slots.e;
        dashCoolSetter.move = playerControl.movement;

        //test boss generation
        bossControl = Instantiate(tempBossPrefab, field.transform.Find("BossStartPositions").GetChild(0).position, Quaternion.LookRotation(Vector3.back)).GetComponent<TempBossControlComponent>();
        bossControl.mapCenter = field.transform.Find("SpecialPositions").GetChild(0).transform.position;

        Stats bossStats = bossControl.GetComponent<Stats>();
        bossHPBar.targetStats = bossStats;
        bossHPBar.targetType = Stat.Type.MaxHP;

        #endregion �׽�Ʈ ����

        //TODO: ����)�޴����� ĳ���ͳ� ���� ������ ������ �޾ƿ��� �ν��Ͻ� ����� �Ҵ��ϴ� ������� �ϼ�.
        //��Ƽ�� ī�޶�� ���߿� ��������.
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }

        if (bossControl.isDead)
        {
            playerControl.isEnd = true;

            gameCanvas.SetActive(false);

            //�¸�ó��
            gameOverBackground.SetActive(true);
            winImage.SetActive(true);
            titleButton.SetActive(true);

            isGameOver = true;
        }
        else if (playerControl.isDead)
        {
            bossControl.isEnd = true;

            gameCanvas.SetActive(false);

            //�й�ó��
            gameOverBackground.SetActive(true);
            loseImage.SetActive(true);
            titleButton.SetActive(true);

            isGameOver = true;
        }
    }

    public void GoTitleScene()
    {
#if UNITY_ANDROID
        SceneManager.LoadScene("AndroidTitleScene");
#endif
#if UNITY_STANDALONE_WIN
        SceneManager.LoadScene("TitleScene");
#endif
    }
}
