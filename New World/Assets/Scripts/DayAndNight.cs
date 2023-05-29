using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    public Light directionalLight;
    public float dayDuration = 30.0f; // 낮의 기간 (초 단위)

    private float timer; // 경과한 시간

    void Update()
    {
        // 경과한 시간 업데이트
        timer += Time.deltaTime;

        // 낮과 밤 전환
        float t = Mathf.PingPong(timer, dayDuration) / dayDuration;

        // Directional Light의 Intensity 조정 (0.0 - 1.0 사이의 값 사용)
        directionalLight.intensity = Mathf.Lerp(0.0f, 1.0f, t);

        // Directional Light의 Rotation 조정
        float rotationAngle = Mathf.Lerp(-90.0f, 270.0f, t); // -90도부터 270도까지 회전
        directionalLight.transform.rotation = Quaternion.Euler(rotationAngle, 0.0f, 0.0f);

        if (timer >= 30.0f)
        {
            timer = 0.0f;
        }
    }
}