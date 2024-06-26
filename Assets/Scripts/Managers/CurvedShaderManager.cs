using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurvedShaderManager : MonoBehaviour
{
    public static CurvedShaderManager Instance {  get; private set; }
    private static string shaderSidewaysStrengthsPropertyName = "_Sideways_Strength";
    private static string shaderBackwardsStrengthsPropertyName = "_Backwards_Strength";
    [SerializeField] private float minStrengthsValue;
    [SerializeField] private float maxStrengthsValue;
    [SerializeField] private float valuesLerpTime;
    public static float sidewaysStrenghtValue = 0.008f;
    public static float backwardsStrenghtValue = -0.002f;
    private bool isChangingShaderValues;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There are multiple instances of CuvedShaderManager");
        }
        Instance = this;
    }

    private void Start()
    {
        Player.Instance.OnCurveShaderChange += Player_OnCurveShaderChange;
    }

    private void Player_OnCurveShaderChange(object sender, System.EventArgs e)
    {
        StartCoroutine(StrengthsValuesLerp(sidewaysStrenghtValue, GetRandomizeStrengthsValue(), backwardsStrenghtValue, GetRandomizeStrengthsValue()));
    }

    private IEnumerator StrengthsValuesLerp(float startValueSideways, float targetValueSideways, float startValueBackwards, float targetValueBackwards)
    {
        float elapsedTime = 0;
        while (elapsedTime < valuesLerpTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpInterval = elapsedTime / valuesLerpTime;
            CurvedShaderManager.sidewaysStrenghtValue = Mathf.SmoothStep(startValueSideways, targetValueSideways, lerpInterval);
            CurvedShaderManager.backwardsStrenghtValue = Mathf.SmoothStep(startValueBackwards, targetValueBackwards, lerpInterval);
            SetShaderStrenghtsOnRenderers(FindObjectsOfType<Renderer>());
            yield return null;
        }
    }
    public static void SetShaderStrenghtsOnRenderers(Renderer[] renderers)
    {
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;

            foreach (Material mat in materials)
            {
                if (mat.HasProperty(shaderSidewaysStrengthsPropertyName))
                {
                    mat.SetFloat(shaderSidewaysStrengthsPropertyName, sidewaysStrenghtValue);
                }
                if (mat.HasProperty(shaderBackwardsStrengthsPropertyName))
                {
                    mat.SetFloat(shaderBackwardsStrengthsPropertyName, backwardsStrenghtValue);
                }
            }
        }
    }
    public static void SetShaderStrenghtsOnRenderers(Renderer renderer)
    {
        Material[] materials = renderer.materials;

        foreach (Material mat in materials)
        {
            if (mat.HasProperty(shaderSidewaysStrengthsPropertyName))
            {
                mat.SetFloat(shaderSidewaysStrengthsPropertyName, sidewaysStrenghtValue);
            }
            if (mat.HasProperty(shaderBackwardsStrengthsPropertyName))
            {
                mat.SetFloat(shaderBackwardsStrengthsPropertyName, backwardsStrenghtValue);
            }
        }
    }

    private float GetRandomizeStrengthsValue()
    {
        return UnityEngine.Random.Range(minStrengthsValue, maxStrengthsValue); 
    }
}
