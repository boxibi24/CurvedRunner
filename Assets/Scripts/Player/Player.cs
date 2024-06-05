using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float Z_OFFSET_VALUE = 0.01f;
    public static Player Instance {  get; private set; }
    public event EventHandler OnJump;
    public event EventHandler OnCurveShaderChange;
    [SerializeField] private float playerRunSpeed;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private List<RoadSectionSO> roadSectionSOList;
    private float currentZOffset;
    [SerializeField] private float jumpTime = 0.5f;
    private bool isJumping;
    int roadSectionSpawnCounter = 0;
    private enum PlayerPositionState
    {
        Left,
        Middle,
        Right,
    }
    private Vector3 playerStartPosition;
    private PlayerPositionState playerPositionState;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnPlayerMoveLeft += GameInput_OnPlayerMoveLeft;
        gameInput.OnPlayerMoveRight += GameInput_OnPlayerMoveRight;
        playerPositionState = PlayerPositionState.Middle;
        playerStartPosition = transform.position;
    }

    private void GameInput_OnPlayerMoveRight(object sender, System.EventArgs e)
    {
        if(playerPositionState == PlayerPositionState.Right) { return; }
        else if (playerPositionState == PlayerPositionState.Middle) { SetPlayerPositionState(PlayerPositionState.Right); }
        else { SetPlayerPositionState(PlayerPositionState.Middle); }
    }

    private void GameInput_OnPlayerMoveLeft(object sender, System.EventArgs e)
    {
        if (playerPositionState == PlayerPositionState.Left) { return; }
        else if (playerPositionState == PlayerPositionState.Middle) { SetPlayerPositionState(PlayerPositionState.Left); }
        else { SetPlayerPositionState(PlayerPositionState.Middle); }
    }

    private void SetPlayerPositionState(PlayerPositionState state)
    {
        if (!isJumping)
        {
            playerPositionState = state;
            float sideMovingDistance = 3.3f;
            Vector3 targetPosition = transform.position;
            switch (state)
            {
                case PlayerPositionState.Left:
                    targetPosition = new Vector3(playerStartPosition.x - sideMovingDistance, transform.position.y, transform.position.z);
                    break;
                case PlayerPositionState.Middle:
                    targetPosition = new Vector3(playerStartPosition.x, transform.position.y, transform.position.z);
                    break;
                case PlayerPositionState.Right:
                    targetPosition = new Vector3(playerStartPosition.x + sideMovingDistance, transform.position.y, transform.position.z);
                    break;
            }
            OnJump?.Invoke(this, EventArgs.Empty);
            StartCoroutine(jumpLerp(transform.position, targetPosition));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        float roadSectionLength = 110f;
        if (other.gameObject.CompareTag("Trigger"))
        {
            if (currentZOffset > 0) { currentZOffset = 0; }
            else { currentZOffset = Z_OFFSET_VALUE; }
            Transform roadSection = Instantiate(GetRandomRoadSectionSO().prefab, new Vector3(0, currentZOffset, roadSectionLength), Quaternion.identity);
            if (roadSectionSpawnCounter == 3)
            {
                roadSectionSpawnCounter = 0;
                roadSection.GetComponent<RoadSection>().SetShaderChangeTriggerActive();
            }
            else
            {
                roadSectionSpawnCounter++;
            }
        }
        else if (other.gameObject.CompareTag("ShaderChangeTrigger"))
        {
            OnCurveShaderChange?.Invoke(this, EventArgs.Empty);
        }
        else if (other.gameObject.CompareTag("Collectible"))
        {
            GameManager.Instance.IncrementCoinCount();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.SetGameOver();
        }
    }

    private RoadSectionSO GetRandomRoadSectionSO()
    {
        return roadSectionSOList[UnityEngine.Random.Range(0, roadSectionSOList.Count)];
    }

    private IEnumerator jumpLerp(Vector3 startPosition, Vector3 targetPosition)
    {
        float elapsedTime = 0;
        while (elapsedTime < jumpTime) 
        { 
            elapsedTime += Time.deltaTime;
            isJumping = true;
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / jumpTime);
            yield return null;
        }
        isJumping = false;
    }

    public float GetPlayerRunSpeed()
    {
        return playerRunSpeed;
    }
}
