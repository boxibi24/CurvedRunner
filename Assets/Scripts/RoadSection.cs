using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSection : MonoBehaviour
{
    private void Start()
    {
        CurvedShaderManager.ChangeShaderStrenghtsOnRenderers(GetComponentsInChildren<Renderer>());
    }
}
