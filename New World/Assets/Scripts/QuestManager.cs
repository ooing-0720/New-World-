using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Quest nowQuest;

    // 현재 진행중인 퀘스트가 있는지
    private static bool isActive = false;
    private static bool isCompleted = false;

    // 최대 1개의 퀘스트만 생성할 수 있도록 싱글톤 유지
    private static QuestManager instance;
    public static QuestManager Instance
    {
        get
        {
            if (instance == null)
            {
                // QuestManager 오브젝트가 없는 경우, 새로운 GameObject를 생성하고 QuestManager 스크립트를 추가합니다.
                GameObject questManagerObj = new GameObject("QuestManager");
                instance = questManagerObj.AddComponent<QuestManager>();
            }

            return instance;
        }
    }

  

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool IsActive
    {
        get
        {
            return isActive;
        }
    }

    public bool IsCompleted
    {
        get
        {
            return isCompleted;
        }
    }

    public void AssignQuest(Quest quest)
    {
        nowQuest = quest;
        isActive = true;
        isCompleted = false;
    }

    public Quest GetNowQuest()
    {
        return nowQuest;
    }

    public static void InactiveQuest()
    {
        isActive = false;
    }

    public static void CompleteQuest()
    {
        isCompleted = true;
    }

    // 현재 진행중인 퀘스트 없음
    public bool InactiveAndCompleted
    {
        get
        {
            return !isActive && isCompleted;
        }
       
    }
}
