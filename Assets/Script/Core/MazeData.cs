[System.Serializable]
public class MazeData
{
    public int MazeWidth { get; private set; }
    public int MazeDepth { get; private set; }
    public int MazeSeed { get; private set; }
    
    // save wether a coin is on the maze cell or not
    public bool[,] CoinNotOnCell { get; set; } 
    
    public MazeData(MazeGenerator mazeGenerator)
    {
        CoinNotOnCell = mazeGenerator.CoinNotOnCell;
        MazeWidth = mazeGenerator.MazeWidth;
        MazeDepth = mazeGenerator.MazeDepth;
        MazeSeed = mazeGenerator.MazeSeed;
    }
 
    
}