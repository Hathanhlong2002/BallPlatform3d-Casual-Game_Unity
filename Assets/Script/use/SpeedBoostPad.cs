using UnityEngine;

public class SpeedBoostPad : MonoBehaviour
{
    [SerializeField] private float boostImpulse = 14f;
    [SerializeField] private AudioClip boostClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponent<BallPlayerMovement>() != null)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Push forward in the pad direction, retaining horizontal velocity
                Vector3 boostDirection = transform.forward;
                boostDirection.y = 0f;
                boostDirection.Normalize();

                rb.AddForce(boostDirection * boostImpulse, ForceMode.VelocityChange);

                if (AudioManager.instance != null)
                {
                    AudioClip clip = boostClip != null ? boostClip : AudioManager.instance.GetCoin;
                    AudioManager.instance.PlaySfx(clip);
                }
            }
        }
    }
}
