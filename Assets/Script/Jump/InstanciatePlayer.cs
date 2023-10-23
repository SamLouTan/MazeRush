using UnityEngine;

public class InstanciatePlayer : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    
    void Start()
    {
        // Trouvez l'objet startFloor dans la scène
        GameObject startFloor = GameObject.Find("startFloor");

        // Si startFloor est trouvé
        if (startFloor != null)
        {
            Vector3 startFloorCenter = startFloor.transform.position;
            BoxCollider boxCollider = startFloor.GetComponent<BoxCollider>();
            
            if (boxCollider != null)
            {
                Vector3 centerPosition = startFloorCenter + boxCollider.center;
                Instantiate(playerPrefab, centerPosition, Quaternion.identity);
                //Instantiate(playerPrefab, new Vector3(1, 8, 15), Quaternion.identity); 
                // pour debug 
            }
            else
            {
                Instantiate(playerPrefab, startFloorCenter, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogError("startFloor not found");
        }
    }
}