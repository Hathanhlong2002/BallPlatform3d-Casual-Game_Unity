using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private AudioClip bounceClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponent<BallPlayerMovement>() != null)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Reset vertical velocity so jump height is consistent regardless of falling speed
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                if (AudioManager.instance != null)
                {
                    AudioClip clip = bounceClip != null ? bounceClip : AudioManager.instance.GameBonus;
                    AudioManager.instance.PlaySfx(clip);
                }
            }
        }
    }
}
