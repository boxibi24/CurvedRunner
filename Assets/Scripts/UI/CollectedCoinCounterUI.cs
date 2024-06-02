using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectedCoinCounterUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI collectedCoinCountText;
    private void Start()
    {
        GameManager.Instance.OnUpdateCollectedCoinCount += GameManager_OnUpdateCollectedCoinCount;
    }

    private void GameManager_OnUpdateCollectedCoinCount(object sender, System.EventArgs e)
    {
        collectedCoinCountText.SetText(GameManager.Instance.GetCollectedCoin().ToString());
    }
}
