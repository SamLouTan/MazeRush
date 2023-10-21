using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// import protobuf-net

public static class SaveManager
{
    public static void SavePlayer(Player player)
    {
        // create a new player data object
        PlayerData playerData = new PlayerData(player);
        // serialize the player data object
        // save the serialized player data object to a file
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.data";
        System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Create);
        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        // load the serialized player data object from a file
        // deserialize the player data object
        // return the player data object
        string path = Application.persistentDataPath + "/player.data";
        if (System.IO.File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Open);
            PlayerData playerData = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return playerData;
        }

        Debug.LogError("Save file not found in " + path);
        return null;
    }
    
    public static void SaveMaze(MazeGenerator mazeGenerator)
    {
        // create a new room data object
        MazeData data = new MazeData(mazeGenerator);
        // serialize the room data object
        // save the serialized room data object to a file
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/room.data";
        System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    
    public static MazeData LoadMaze()
    {
        // load the serialized room data object from a file
        // deserialize the room data object
        // return the room data object
        string path = Application.persistentDataPath + "/room.data";
        if (System.IO.File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Open);
            MazeData data = formatter.Deserialize(stream) as MazeData;
            stream.Close();
            return data;
        }

        Debug.LogWarning("Save file not found in " + path+" Creating new save file");
        return null;
    }
    
}