using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Cooking : MonoBehaviour
{
    public int targetClicks = 20; // 목표 클릭 수
    public float gameDuration = 10f; // 게임 시간 (초)
    public TMP_Text timerText; // 시간을 표시할 TMP 텍스트
    public TMP_Text clickText; // 클릭 수를 표시할 TMP 텍스트
    public TMP_Text resultText; // 결과를 표시할 TMP 텍스트
    private int currentClicks = 0; // 현재 클릭 수
    private float currentTime = 0f; // 현재 시간
    private bool gameEnded = false; // 게임 종료 여부
    public RawImage foodImage; // RawImage 컴포넌트

    private bool isLeftKeyPressed = false; // 왼쪽 방향키 입력 여부
    private bool isRightKeyPressed = false; // 오른쪽 방향키 입력 여부

    public static bool isSuccess = false;

    private Color initialColor; // 초기 이미지 색상

    private float waitTime = 1f;
    private float timer = 0f;

    void Start()
    {
        // 초기화
        currentClicks = 0;
        currentTime = gameDuration;
        gameEnded = false;

        // UI 초기화
        UpdateUI();

        // 초기 이미지 색상 저장
        initialColor = foodImage.color;
        
        foodImage.color = Color.clear;
    }

    void Update()
    {
        // 게임 성공 시 씬 이동
        if (gameEnded)
        {
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                // 원래 씬을 활성화
                Scene originalScene = SceneManager.GetSceneByName("World_Sample");
                SceneManager.SetActiveScene(originalScene);

                // Cooking 씬을 닫음
                SceneManager.UnloadSceneAsync("Cooking");
                timer = 0f;
            }
            isSuccess = true;
            return;
        }

        // 시간 갱신
        currentTime -= Time.deltaTime;
        UpdateUI();

        // 게임 종료 체크
        if (currentTime <= 0f || currentClicks >= targetClicks)
        {
            EndGame();

        }

        // 방향키 입력 체크
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isLeftKeyPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            isRightKeyPressed = true;
        }

        // 클릭 수 증가 체크
        if (isLeftKeyPressed && isRightKeyPressed)
        {
            IncrementClick();
            isLeftKeyPressed = false;
            isRightKeyPressed = false;
        }

        
    }

    void UpdateUI()
    {
        // TMP 텍스트 업데이트
        timerText.text = "Time: " + Mathf.CeilToInt(currentTime).ToString() + "s";
        clickText.text = "Clicks: " + currentClicks.ToString() + "/" + targetClicks.ToString();
    }

    void IncrementClick()
    {
        if (gameEnded)
            return;

        // 클릭 수 증가
        currentClicks++;
        UpdateUI(); 
        // 이미지 서서히 선명하게 만들기
        float fadeDuration = 0.5f;
        float fadeEndValue = (float)currentClicks / targetClicks; // 클릭 수에 따른 투명도 계산
        foodImage.DOColor(new Color(initialColor.r, initialColor.g, initialColor.b, fadeEndValue), fadeDuration);

        // 최종 클릭일 경우 게임 종료
        if (currentClicks >= targetClicks)
        {
            foodImage.DOColor(Color.white, fadeDuration).OnComplete(EndGame);
        }
    }

    void EndGame()
    {
        gameEnded = true;

        if (currentClicks >= targetClicks)
        {
            // 게임 성공
            resultText.text = "Success!";
            
        }
        else
        {
            // 게임 실패
            resultText.text = "Failure!";
        }
    }
}