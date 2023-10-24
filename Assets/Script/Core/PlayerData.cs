// Description: PlayerData class for storing player data.
// 

using UnityEngine;

[System.Serializable]
public class PlayerData
{
    
    public int CurrentRoom { get; private set; }// Current level is the level that the player is currently playing
    public int CurrentGold { get; private set; }// Current gold is the gold that the player currently has in the game actually not implemented yet
   
    //Player Stats
    public int RemainingBullets { get; private set; }// 0 = no bullets, 1 = 1 bullet, 2 = 2 bullets, etc.
    public int Checkpoint { get; private set; } // 0 = no checkpoint, 1 = checkpoint 1, 2 = checkpoint 2, etc.
    public float[] CheckpointPosition { get; private set; } = new float[3];// store checkpoint position as a float array
    public int Difficulty { get; private set; } // 0 = easy, 1 = normal, 2 = hard, etc.
    public int[,] UnlockedLevels { get; private set; } // 0 = locked, 1 = unlocked
    
    //Transform
    public float[] Position { get; private set; } = new float[3];// store position as a float array
    public float[] Rotation { get; private set; } = new float[3];// store rotation as a float array
    public float[] Scale { get; private set; } = new float[3];// store scale as a float array

    public float Timer {get; private set;}
  
    public PlayerData(Player player)
    {
        Vector3 playerPosition = player.PlayerTransform.position;
        Vector3 playerRotation = player.PlayerTransform.rotation.eulerAngles;
        Vector3 playerScale = player.PlayerTransform.localScale;
        CurrentRoom = player.CurrentRoom;
        CurrentGold = player.CurrentGold;
        RemainingBullets = player.RemainingBullets;
        Checkpoint = player.Checkpoint;
        CheckpointPosition[0]= player.CheckpointPosition.x;
        CheckpointPosition[1]= player.CheckpointPosition.y;
        CheckpointPosition[2]= player.CheckpointPosition.z;
        Difficulty = player.Difficulty;
        UnlockedLevels = player.UnlockedLevels;
        Position[0] = playerPosition.x;
        Position[1] = playerPosition.y;
        Position[2] = playerPosition.z;
        Rotation[0] = playerRotation.x;
        Rotation[1] = playerRotation.y;
        Rotation[2] = playerRotation.z;
        Scale[0] = playerScale.x;
        Scale[1] = playerScale.y;
        Scale[2] = playerScale.z;
        Timer = player.Timer;
    }
    public PlayerData(int currentRoom, float timer)
    {
         PlayerData _this = SaveManager.LoadPlayer();
        Timer = timer;
        CurrentRoom = currentRoom;
        CurrentGold = _this.CurrentGold;
        RemainingBullets = _this.RemainingBullets;
        Checkpoint = _this.Checkpoint;
        CheckpointPosition[0]= _this.CheckpointPosition[0];
        CheckpointPosition[1]= _this.CheckpointPosition[1];
        CheckpointPosition[2]= _this.CheckpointPosition[2];
        Difficulty = _this.Difficulty;
        UnlockedLevels = _this.UnlockedLevels;
        Position[0] = _this.Position[0];
        Position[1] =  _this.Position[1];
        Position[2] =  _this.Position[2];
        Rotation[0] = _this.Rotation[0];
        Rotation[1] = _this.Rotation[1];
        Rotation[2] = _this.Rotation[2];
        Scale[0] =_this.Scale[0];
        Scale[1] =_this.Scale[1];
        Scale[2] =_this.Scale[2];
     
        
        
    }
}