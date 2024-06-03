using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnList;
    [SerializeField] private bool randomizeSpawnTime;
    [SerializeField] private float spawnTime;
    [SerializeField, ShowIf("randomizeSpawnTime")] private float spawnTimeMin;
    private float spawnTimer = 0f;
    private float spawnInterval = 0f;
    private float updateShaderTimer = 1f;
    private bool shouldSpawn = false;
    [SerializeField] private bool isReverseDirection;


    private void Start()
    {
        if (spawnTimeMin > spawnTime)
        {
            Debug.LogError("Cannot set SpawnTimeMin greater than SpawnTime");
        }
        if (randomizeSpawnTime)
        {
            setRandomSpawnInterval();
        }
        else
        {
            spawnInterval = spawnTime;
        }
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;

    }

    private void GameManager_OnStateChange(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            shouldSpawn = false;
        }
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            shouldSpawn = false;
        }
        if (GameManager.Instance.IsGamePlaying())
        {
            shouldSpawn = true;
        }
    }

    private void setRandomSpawnInterval()
    {
        spawnInterval = UnityEngine.Random.Range(spawnTimeMin, spawnTime);
    }

    private void Update()
    {
        if (shouldSpawn) 
        { 
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                spawnTimer = 0;
                spawnRandomObjectInList();
                if (randomizeSpawnTime)
                {
                    setRandomSpawnInterval();
                }
            }
        }
        updateShaderTimer -= Time.deltaTime;
        if (updateShaderTimer < 0)
        {
            updateShaderTimer = 1;
            CurvedShaderManager.SetShaderStrenghtsOnRenderers(GetComponentsInChildren<Renderer>());
        }
    }

    private void spawnRandomObjectInList()
    {
        if (isReverseDirection)
        {
            Instantiate(spawnList[UnityEngine.Random.Range(0, spawnList.Count)], transform.position, Quaternion.Euler(new Vector3(0,180,0))) ;
        }
        else
        {
            Instantiate(spawnList[UnityEngine.Random.Range(0, spawnList.Count)], transform);
        }
    }
}
