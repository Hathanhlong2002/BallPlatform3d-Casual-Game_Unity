using System.Collections;
using UnityEngine;

public class PeriodicTrap : MonoBehaviour
{
    [SerializeField] private Transform spikesTransform;
    [SerializeField] private float spikeExtendedY = 0.5f;
    [SerializeField] private float spikeRetractedY = -0.5f;
    [SerializeField] private float activeDuration = 1.2f;
    [SerializeField] private float inactiveDuration = 1.8f;
    [SerializeField] private float transitionSpeed = 6f;

    private bool isSpikeActive = false;

    private void Start()
    {
        if (spikesTransform == null)
        {
            spikesTransform = transform.Find("Spikes");
        }
        StartCoroutine(TrapCycleRoutine());
    }

    private IEnumerator TrapCycleRoutine()
    {
        while (true)
        {
            // Retract
            isSpikeActive = false;
            yield return MoveSpikes(spikeRetractedY);
            yield return new WaitForSeconds(inactiveDuration);

            // Extend
            isSpikeActive = true;
            yield return MoveSpikes(spikeExtendedY);
            yield return new WaitForSeconds(activeDuration);
        }
    }

    private IEnumerator MoveSpikes(float targetY)
    {
        if (spikesTransform == null) yield break;

        Vector3 targetPos = new Vector3(spikesTransform.localPosition.x, targetY, spikesTransform.localPosition.z);
        while (Vector3.Distance(spikesTransform.localPosition, targetPos) > 0.02f)
        {
            spikesTransform.localPosition = Vector3.MoveTowards(spikesTransform.localPosition, targetPos, transitionSpeed * Time.deltaTime);
            yield return null;
        }
        spikesTransform.localPosition = targetPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckKillPlayer(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        CheckKillPlayer(other.gameObject);
    }

    private void CheckKillPlayer(GameObject target)
    {
        if (!isSpikeActive) return;

        if (target.CompareTag("Player") || target.GetComponent<BallPlayerMovement>() != null)
        {
            BallPlayerMovement player = target.GetComponent<BallPlayerMovement>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}
