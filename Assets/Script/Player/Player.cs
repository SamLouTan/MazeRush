using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public int CurrentRoom { get;  set; }// Current level is the level that the player is currently playing
    public int CurrentGold { get; private set; }// Current gold is the gold that the player currently has in the game actually not implemented yet
   
    public Transform PlayerTransform { get; private set; }
    
    public int RemainingBullets { get; private set; }// 0 = no bullets, 1 = 1 bullet, 2 = 2 bullets, etc.
    public int Checkpoint { get; private set; } // 0 = no checkpoint, 1 = checkpoint 1, 2 = checkpoint 2, etc.
    public Vector3 CheckpointPosition { get; private set; }// store checkpoint position as a float array
    public int Difficulty { get; private set; } // 0 = easy, 1 = normal, 2 = hard, etc.
    public int[] UnlockedLevels { get; private set; } // 0 = locked, 1 = unlocked
    
    [SerializeField]
    private PlayerController playerController;
    
    private PlayerData playerData { get; set; }
    
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        PlayerTransform = playerController.transform;
        playerData = SaveManager.LoadPlayer();
        if(playerData != null)
        {
            CurrentRoom = playerData.CurrentRoom;
            CurrentGold = playerData.CurrentGold;
            RemainingBullets = playerData.RemainingBullets;
            Checkpoint = playerData.Checkpoint;
            CheckpointPosition = new Vector3(playerData.CheckpointPosition[0], playerData.CheckpointPosition[1], playerData.CheckpointPosition[2]);
            Difficulty = playerData.Difficulty;
            UnlockedLevels = playerData.UnlockedLevels;
            PlayerTransform.position = new Vector3(playerData.Position[0], playerData.Position[1], playerData.Position[2]);
            PlayerTransform.rotation = Quaternion.Euler(new Vector3(playerData.Rotation[0], playerData.Rotation[1], playerData.Rotation[2]));
            PlayerTransform.localScale = new Vector3(playerData.Scale[0], playerData.Scale[1], playerData.Scale[2]);
        }
        else
        {
            CurrentGold = 0;
            RemainingBullets = 0;
            Checkpoint = 0;
            CheckpointPosition = new Vector3(0, 0, 0);
            Difficulty = 0;
            UnlockedLevels = new int[3];
            PlayerTransform.position = PlayerTransform.position;
            PlayerTransform.rotation =PlayerTransform.rotation;
            PlayerTransform.localScale = PlayerTransform.localScale;
        }
    }   

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            CurrentGold++;
            GameObject cell = other.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject;
            MazeGenerator parent = cell.GetComponentInParent<MazeGenerator>();
            Vector2 cellPosition = parent.GetCellPosition(cell);
            parent.CoinNotOnCell[(int)cellPosition.x, (int)cellPosition.y] = true; 
            Destroy(other.gameObject);
            Debug.Log(CurrentGold);
        }
    }
}
