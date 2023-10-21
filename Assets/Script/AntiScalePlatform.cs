using UnityEngine;

public class FollowPlatform : MonoBehaviour
{
    public Transform platform;

    void Update()
    {
        // Suivre la position et la rotation de la plateforme
        transform.position = platform.position;
        transform.rotation = platform.rotation;
    }
}