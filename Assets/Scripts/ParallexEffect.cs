using UnityEngine;

public class SimpleParallax : MonoBehaviour
{
    public Transform player;  
    public float parallaxSpeed = 0.5f; 

    private Vector3 previousPlayerPosition;  

    void Start()
    {
        // Store the initial player position
        previousPlayerPosition = player.position;
    }

    void Update()
    {
 
        float movement = player.position.x - previousPlayerPosition.x;

        transform.position = new Vector3(transform.position.x + movement * parallaxSpeed, transform.position.y, transform.position.z);

        previousPlayerPosition = player.position;
    }
}
