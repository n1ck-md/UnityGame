using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;               
    public Vector3 offset = new Vector3(0, 0, -10f); 
    public float smoothTime = 0.2f;       

    private Vector3 velocity = Vector3.zero;  

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPosition = player.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
