using UnityEngine;

public class InverseScale : MonoBehaviour
{
    void Update()
    {
        if (transform.parent != null)
        {
            transform.localScale = new Vector3(1f / transform.parent.localScale.x, 1f / transform.parent.localScale.y, 1f / transform.parent.localScale.z);
        }
    }
}