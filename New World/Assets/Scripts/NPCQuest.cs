using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCQuest : MonoBehaviour
{
    public QuestManager questManager;
    public Quest[] quests;
    Quest newQuest;

    // 캐릭터 위에 퀘스트 이미지 생성에 쓰이는 변수
    public GameObject questImagePrefab;
    private GameObject questImage;
    public float minDelay = 3f;
    public float maxDelay = 5f;

    private bool isQuestActive = false;
    private float questTimer = 0f;

    private float interactionDistance = 2f;
    private GameObject player;
    private bool canInteract = false;

    public Slider levelUpSlider;
    public TMP_Text levelText;
    public TMP_Text pointsText;

    public int levelUpPoints = 30; // 레벨업에 필요한 포인트
    private int pointsIncrement = 10; // 레벨업마다 포인트가 증가하는 양
    private int currentPoint = 0; // 현재 포인트
    private int remainingPoint = 0; // 남은 포인트
    private int currentLevel = 1; // 현재 레벨

    private void Start()
    {
        questTimer = Random.Range(1f, 3f);
        QuestManager.CompleteQuest();

        player = GameObject.FindGameObjectWithTag("Player");

        if (!PlayerPrefs.HasKey("Point"))
        {
            PlayerPrefs.SetInt("Point", 0);
        }
        
        if (PlayerPrefs.HasKey("LevelUpPoints"))
        {
            levelUpPoints = PlayerPrefs.GetInt("LevelUpPoints");
        }
        else
        {
            levelUpPoints = 30;
        }

        // 저장된 정보 가져오기
        currentPoint = PlayerPrefs.GetInt("Point");
        currentLevel = PlayerPrefs.GetInt("Level", 1);

        UpdateLevelUI();
    }

    private void Update()
    {
       
        if (canInteract && Input.GetKeyDown(KeyCode.Space))
        {
            if (isQuestActive && !QuestManager.Instance.IsActive)
            {
                GenerateQuest();
            }

            // 퀘스트 완료된 후 NPC에게 다가가면 퀘스트 종료 및 포인트 획득
            if (QuestManager.Instance.IsCompleted && QuestManager.Instance.IsActive)
            {
                CompleteQuest();
            }
        }
        
            

        if (!isQuestActive && QuestManager.Instance.InactiveAndCompleted)
        {
            questTimer -= Time.deltaTime;
            if (questTimer <= 0f)
            {
                GenerateQuestImage();
            }
        }
    }

    private void GenerateQuest()
    {
        if (isQuestActive && !QuestManager.Instance.IsActive)
        {
            newQuest = GetRandomQuest();
            QuestManager.Instance.AssignQuest(newQuest);

            Debug.Log("새로운 퀘스트 부여: " + newQuest.title);
            Debug.Log("목표: " + newQuest.objectives);
            Debug.Log("보상: " + newQuest.rewards);
        }

    }


    // 퀘스트 생성되면 퀘스트 이미지 생성
    private void GenerateQuestImage()
    {
        Vector3 questPosition = transform.position;
        questPosition.y = questPosition.y + 3;
        questImage = Instantiate(questImagePrefab, questPosition, Quaternion.identity);
        questImage.transform.SetParent(transform);

        isQuestActive = true;
    }

    // 퀘스트를 생성한 NPC에게 가서 스페이스바 누르면 퀘스트 부여
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == player )
        {
            canInteract = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject == player)
        {
            canInteract = false;
        }
    }

    private Quest GetRandomQuest()
    {
        int randomIndex = Random.Range(0, quests.Length);
        return quests[randomIndex];
    }
    
    public void CompleteQuest()
    {
        questTimer = Random.Range(minDelay, maxDelay);  // 퀘스트 완료되고 3~5초 사이에 새로운 퀘스트 생성
        QuestManager.InactiveQuest();
        Destroy(questImage);

        isQuestActive = false;
        if (newQuest.title == "낚시")
        {
            currentPoint += 30;
            FishingEvent.isActive = false;
        }
        else if (newQuest.title == "요리")
        {
            currentPoint += 50;
            CookingEvent.isActive = false;
        }
        
        PlayerPrefs.SetInt("Point", currentPoint);

        if (currentPoint >= levelUpPoints)
        {
            currentLevel++;
            currentPoint -= levelUpPoints;

            levelUpPoints += pointsIncrement;

            PlayerPrefs.SetInt("Level", currentLevel);
            PlayerPrefs.SetInt("LevelUpPoints", levelUpPoints);
            PlayerPrefs.SetInt("Point", currentPoint);

            Debug.Log("레벨 업! 현재 레벨: " + currentLevel);
            Debug.Log("남은 포인트: " + remainingPoint);
        }

        UpdateLevelText(); // 레벨 텍스트 업데이트
        UpdateLevelUI(); // 퀘스트 완료 시 UI를 업데이트합니다.
    }

    private void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + currentLevel + " (" + remainingPoint + " points remaining)";
        }
    }

    private void UpdateLevelUI()
    {
        int currentPoints = PlayerPrefs.GetInt("Point");
        int pointsToNextLevel = levelUpPoints - (currentPoints % levelUpPoints);
        remainingPoint = pointsToNextLevel;

        levelUpSlider.value = (float)(currentPoints % levelUpPoints) / levelUpPoints;
        levelText.text = "Lv: " + currentLevel.ToString();
        pointsText.text = "Points: " + currentPoints.ToString() + " / " + levelUpPoints.ToString();
    }

}
