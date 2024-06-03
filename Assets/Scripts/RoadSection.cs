using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSection : MonoBehaviour
{
    [SerializeField] private GameObject shaderChangeTriggerGameObject;

    private void Start()
    {
        CurvedShaderManager.SetShaderStrenghtsOnRenderers(GetComponentsInChildren<Renderer>());
    }

    public void SetShaderChangeTriggerActive()
    {
        shaderChangeTriggerGameObject.SetActive(true);
    }
}
