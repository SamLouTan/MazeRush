using UnityEngine;

public class InstanciatePlayer : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject inGameMenuPrefab;
    [SerializeField] private GameObject startFloor;
    private GameObject _player;
    public Vector3 StartPosition { get; private set; }
    void Start()
    {
        
        // Si startFloor est trouvé
        _player = Instantiate(playerPrefab, GetStartFloorCenter(), Quaternion.identity);
        _player.GetComponent<Player>().CurrentRoom = 1;
        //Instantiate(playerPrefab, new Vector3(1, 8, 15), Quaternion.identity); 
        // pour debug 
        InGameMenuManager inGameMenuManager = GetComponent<InGameMenuManager>();
        inGameMenuManager.inGameMenu = Instantiate(inGameMenuPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        inGameMenuManager.player = _player;
        inGameMenuManager.inGameMenu.SetActive(true);
        _player.transform.parent = transform;
    }

    private Vector3 GetStartFloorCenter()
    {
        // get width and depth of the start floor
        BoxCollider startFloorMeshRenderer = startFloor.GetComponent<BoxCollider>();
        Vector3 bounds = startFloorMeshRenderer.bounds.size;
        float x =bounds.x;
        float z =bounds.z;
        // get center of the start floor
        Vector3 startFloorCenter = startFloor.transform.position;
        startFloorCenter.x -= x / 2;
        startFloorCenter.z -= z / 2;
        StartPosition = startFloorCenter;
        return startFloorCenter;
        
    }
   
    
}