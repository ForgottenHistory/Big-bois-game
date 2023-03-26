using UnityEngine;
using FishNet.Object;

public class MovementDebug : NetworkBehaviour
{
    public float speed = 1f;
    public float amplitude = 1f;

    private float timeOffset;

    void Start()
    {
        // Store the initial time offset
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        // Calculate the horizontal offset based on time and amplitude
        float horizontalOffset = Mathf.Sin(Time.time * speed + timeOffset) * amplitude;

        // Update the object's position
        transform.position = new Vector3(horizontalOffset, transform.position.y, transform.position.z);
    }
}
