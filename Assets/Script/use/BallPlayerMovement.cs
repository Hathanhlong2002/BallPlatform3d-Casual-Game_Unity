using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallPlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float respawnDelay = 1f;

    private Rigidbody rb;
    private Vector2 inputVector;
    private Vector3 respawnPoint;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        respawnPoint = transform.position + Vector3.up * 0.5f;
    }

    private void Update()
    {
        if (isDead)
        {
            inputVector = Vector2.zero;
            return;
        }

        if (Input.GetKey(KeyCode.K))
        {
            inputVector = Vector2.zero;
        }
        else
        {
            inputVector.x = Input.GetAxis("Horizontal");
            inputVector.y = Input.GetAxis("Vertical");
        }
    }

    private void FixedUpdate()
    {
        if (isDead || rb == null || rb.isKinematic) return;

        rb.linearVelocity = new Vector3(inputVector.x * movementSpeed, rb.linearVelocity.y, inputVector.y * movementSpeed);
    }

    public void Die()
    {
        if (isDead) return;
        StartCoroutine(HandleDeathSequence());
    }

    private IEnumerator HandleDeathSequence()
    {
        isDead = true;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySfx(AudioManager.instance.clipDie);
        }

        bool isGameOver = false;
        if (ItemCollector.instance != null)
        {
            isGameOver = ItemCollector.instance.DeductHeart();
        }

        yield return new WaitForSeconds(respawnDelay);

        if (isGameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            transform.position = respawnPoint;
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            isDead = false;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Coin"))
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySfx(AudioManager.instance.GetCoin);
            }
            Destroy(other.gameObject);
            
            if (ItemCollector.instance != null)
            {
                ItemCollector.instance.AddCoins(10);
            }
        }
        else if (other.CompareTag("Hearth"))
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySfx(AudioManager.instance.GameBonus);
            }
            Destroy(other.gameObject);
            
            if (ItemCollector.instance != null)
            {
                ItemCollector.instance.AddHeart(1);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BackGround"))
        {
            Die();
        }
        else if (collision.gameObject.CompareTag("SaveGame"))
        {
            respawnPoint = collision.gameObject.transform.position + Vector3.up * 1f;
        }
    }
} 
