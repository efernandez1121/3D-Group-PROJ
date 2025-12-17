using UnityEngine;

public class BlockSound : MonoBehaviour
{
    public AudioSource audioSource;
    public float movementThreshold = 0.2f; // block must move THIS fast
    public float cooldown = 0.2f;          // delay between sounds

    private Rigidbody rb;
    private float nextPlayTime = 0f;
    private float sceneStartTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        sceneStartTime = Time.time; // record when the scene started
    }

    void Update()
    {
        // Ignore the first 0.5 seconds to prevent startup noise
        if (Time.time < sceneStartTime + 0.5f)
            return;

        float speed = rb.velocity.magnitude;

        // Only play sound when block is truly moving
        if (speed > movementThreshold && Time.time > nextPlayTime)
        {
            audioSource.Play();
            nextPlayTime = Time.time + cooldown;
        }
    }
}
