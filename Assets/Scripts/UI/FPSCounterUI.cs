using TMPro;
using UnityEngine;

public class FPSCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    private float deltaTime;

    private void Update()
    {
        if (!GameManager.Instance.IsGamePaused())
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText.text = Mathf.Ceil(fps).ToString();
        }
    }
}