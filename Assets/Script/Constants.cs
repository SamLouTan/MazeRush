public static class CONSTANTS
{
    public static float MOUSE_SENSITIVITY { get; set; } = 21f;
    public static float PLAYER_SPEED { get; set; } = 4f;
    public static float PLAYER_RUN_SPEED { get; set; } = 6f;
    public static float PLAYER_WALK_SPEED { get; set; } = 2f;
    public static float PLAYER_JUMP_FORCE { get; set; } = 5f;
    public static float PLAYER_GRAVITY { get; set; } = -9.81f;
    public static float PLAYER_GROUND_DISTANCE { get; set; } = 0.4f;
    public static float PLAYER_BOTTOM_LIMIT { get; set; } = -40f;
    public static float PLAYER_UPPER_LIMIT { get; set; } = 70f;
    
    public static int[,] SCENE_TO_LOAD { get; set; } = new int[3,3] {{1,2,3},{4,2,3},{5,2,3}};//2 shoots, 3 maze scene 
}