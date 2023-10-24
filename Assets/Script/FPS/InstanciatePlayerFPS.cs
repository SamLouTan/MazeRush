using UnityEngine;

public class InstanciatePlayerFPS : MonoBehaviour
{
    [SerializeField] private GameObject PlayerFPS;
    void Start()
    { 
        Instantiate(PlayerFPS, new Vector3(8, 1, -12), Quaternion.identity);
    }
}