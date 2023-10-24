[System.Serializable]
public class MazeData
{
    public int MazeWidth { get; private set; }
    public int MazeDepth { get; private set; }
    public int MazeSeed { get; private set; }
    
    // save wether a coin is on the maze cell or not
    public bool[,] CoinNotOnCell { get; set; } 
    
    public int Difficulty { get; private set; } // 0 = easy, 1 = normal, 2 = hard, etc.
    public MazeData(MazeGenerator mazeGenerator)
    {
        CoinNotOnCell = mazeGenerator.CoinNotOnCell;
        MazeWidth = mazeGenerator.MazeWidth;
        MazeDepth = mazeGenerator.MazeDepth;
        MazeSeed = mazeGenerator.MazeSeed;
        Difficulty = mazeGenerator.Difficulty;
  
        
    }
 
    
}