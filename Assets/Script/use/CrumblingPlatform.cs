using System.Collections;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 0.75f;
    [SerializeField] private float respawnDelay = 2.5f;

    private Vector3 initialPos;
    private Collider[] colliders;
    private Renderer[] renderers;
    private bool isCrumbling = false;

    private void Awake()
    {
        initialPos = transform.position;
        colliders = GetComponentsInChildren<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isCrumbling) return;

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.GetComponent<BallPlayerMovement>() != null)
        {
            StartCoroutine(CrumbleRoutine());
        }
    }

    private IEnumerator CrumbleRoutine()
    {
        isCrumbling = true;
        float elapsed = 0f;

        // Visual shake warning
        while (elapsed < fallDelay)
        {
            float shakeX = Random.Range(-0.08f, 0.08f);
            float shakeZ = Random.Range(-0.08f, 0.08f);
            transform.position = initialPos + new Vector3(shakeX, 0f, shakeZ);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Hide platform and disable colliders
        SetPlatformActive(false);

        yield return new WaitForSeconds(respawnDelay);

        // Respawn and restore
        transform.position = initialPos;
        SetPlatformActive(true);
        isCrumbling = false;
    }

    private void SetPlatformActive(bool active)
    {
        if (colliders != null)
        {
            foreach (var col in colliders)
            {
                if (col != null) col.enabled = active;
            }
        }

        if (renderers != null)
        {
            foreach (var rend in renderers)
            {
                if (rend != null) rend.enabled = active;
            }
        }
    }
}
