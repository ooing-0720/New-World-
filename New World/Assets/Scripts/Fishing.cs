using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    // Animator animator;

    public Slider slider;
    private int speed;
    public float minPosition;
    public float maxPosition;
    public RectTransform pass;
    public GameObject successImage;
    public GameObject failImage;

    private bool isIncrease = true;
    public static bool isHit = false;

    private IEnumerator coroutine;

    private float waitTime = 1f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // animator = GetComponent<Animator>();
        coroutine = FishingAttack();
        speed = 100;
        slider.value = 0;
        minPosition = pass.anchoredPosition.x;
        maxPosition = pass.sizeDelta.x + minPosition;
        StartCoroutine(coroutine);  
    }

    IEnumerator FishingAttack()
    {
        yield return null;
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            if (isIncrease)
            {
                slider.value += Time.deltaTime * speed;
                if(slider.value == 200)
                {
                    isIncrease = false;
                }
                yield return null;
            }
            else
            {
                slider.value -= Time.deltaTime * speed;
                if (slider.value == 0)
                {
                    isIncrease = true;
                }
                yield return null;
            }
            

        }// 낚시 성공 -> 포인트 획득
        if (Input.GetKeyDown(KeyCode.Space) && (slider.value >= minPosition && slider.value <= maxPosition))
        {
            successImage.SetActive(true);
            isHit = true;

            yield break; 
        }

        // 낚시 실패 -> 게임 다시
        if (Input.GetKeyDown(KeyCode.Space) && (slider.value <= minPosition && slider.value >= maxPosition))
        {
            failImage.SetActive(true);
            yield return null;
            yield return new WaitForSeconds(2f);
            failImage.SetActive(false);
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (coroutine != null && !coroutine.MoveNext())
        {
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                // Coroutine이 완료되었음
                Debug.Log("Coroutine이 완료되었습니다.");

                // 원래 씬을 활성화
                Scene originalScene = SceneManager.GetSceneByName("World_Sample");
                SceneManager.SetActiveScene(originalScene);

                // Fishing 씬을 닫음
                SceneManager.UnloadSceneAsync("Fishing");
                timer = 0f;
            }
                
        }
    }
        
}


