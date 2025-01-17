using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 10f;
    public float boostSpeed = 50f;
    public float boostDuration = 2f;
    public float jumpForce = 10f;
    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;
    public static int lives = 5;
    private static bool isFirstLoad = true;
    public bool hasStaff = false;
    public static event Action OnStaffCollected;
    public bool isTakingDamage = false;
    public bool keyCollected = false; // Make this public so it can be accessed

    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip fireballSound;
    public AudioClip gameOverSound;
    public AudioClip levelTheme;
    public AudioClip bossFightMusic;
    public AudioClip keySound; // Add key sound

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool onIce = false;
    private float normalFriction = 0.4f;
    private float iceControlFactor = 1f; // Default control factor
    private float iceFriction = 0.4f; // Default friction
    private bool isBoosting = false;
    private AudioSource soundEffects;
    private AudioSource backgroundMusic;
    private float damageDelay = 1f;

    private float fireballCooldown = 1.5f;
    private float lastFireballTime = -1.5f;

    private GoblinSpawner goblinSpawner;
    private Door door; // Add reference to door

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null && rb.sharedMaterial == null)
        {
            rb.sharedMaterial = new PhysicsMaterial2D()
            {
                friction = normalFriction,
            };
        }
        rb.gravityScale = 0.5f;

        backgroundMusic = gameObject.AddComponent<AudioSource>();
        backgroundMusic.clip = levelTheme;
        backgroundMusic.loop = true;
        backgroundMusic.volume = 1f;
        backgroundMusic.Play();

        soundEffects = gameObject.AddComponent<AudioSource>();

        PlayerController.OnStaffCollected += SwitchToBossFightMusic;

        if (isFirstLoad)
        {
            lives = 5;
            isFirstLoad = false;
        }

        goblinSpawner = UnityEngine.Object.FindFirstObjectByType<GoblinSpawner>();
        door = UnityEngine.Object.FindFirstObjectByType<Door>(); // Initialize door reference
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        float currentSpeed = onIce ? walkSpeed * iceControlFactor : walkSpeed;
        if (isBoosting)
        {
            currentSpeed = boostSpeed;
        }

        rb.velocity = new Vector2(moveHorizontal * currentSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isBoosting && moveHorizontal != 0)
        {
            StartCoroutine(Boost());
        }

        if (hasStaff && Input.GetKeyDown(KeyCode.Return) && Time.time >= lastFireballTime + fireballCooldown)
        {
            ShootFireball();
            lastFireballTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            hasStaff = true;
            OnStaffCollected?.Invoke();
        }
    }

    void HandleStaffCollected()
    {
        hasStaff = true;

        if (bossFightMusic != null)
        {
            SwitchToBossFightMusic();
        }
    }

    void SwitchToBossFightMusic()
    {
        if (bossFightMusic != null)
        {
            backgroundMusic.Stop();
            backgroundMusic.clip = bossFightMusic;
            backgroundMusic.Play();
        }
    }

    IEnumerator Boost()
    {
        isBoosting = true;
        soundEffects.PlayOneShot(dashSound);
        yield return new WaitForSeconds(boostDuration);
        isBoosting = false;
    }

    void ShootFireball()
    {
        Vector3 spawnPosition = fireballSpawnPoint.position;
        spawnPosition.y = Mathf.Max(spawnPosition.y, transform.position.y + 0.5f);

        GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D fireballRb = fireball.GetComponent<Rigidbody2D>();
        if (fireballRb != null)
        {
            // Ensure fireball always moves downwards
            Vector2 fireballDirection = Vector2.down.normalized; // Normalize the direction vector
            fireballRb.velocity = fireballDirection * 20f;

            soundEffects.PlayOneShot(fireballSound);
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
        soundEffects.PlayOneShot(jumpSound);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.GetComponent<IcePhysics>() != null)
        {
            isGrounded = true;
            if (collision.gameObject.GetComponent<IcePhysics>() != null)
            {
                onIce = true;
                ApplyIcePhysics(collision.gameObject.GetComponent<IcePhysics>());
            }
            else
            {
                onIce = false;
                ApplyNormalPhysics();
            }
        }

        if (collision.gameObject.CompareTag("Staff"))
        {
            CollectStaff();
            Destroy(collision.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.GetComponent<IcePhysics>() != null)
        {
            isGrounded = true;
            if (collision.gameObject.GetComponent<IcePhysics>() != null)
            {
                onIce = true;
                ApplyIcePhysics(collision.gameObject.GetComponent<IcePhysics>());
            }
            else
            {
                onIce = false;
                ApplyNormalPhysics();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.GetComponent<IcePhysics>() != null)
        {
            isGrounded = false;
            if (collision.gameObject.GetComponent<IcePhysics>() != null)
            {
                onIce = false;
                ApplyNormalPhysics();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lava") && !isTakingDamage)
        {
            StartCoroutine(TakeDamageFromLava());
        }
    }

    IEnumerator TakeDamageFromLava()
    {
        isTakingDamage = true;
        Die();

        yield return new WaitForSeconds(damageDelay);

        isTakingDamage = false;
    }

    void ApplyIcePhysics(IcePhysics ice)
    {
        iceControlFactor = ice.iceControlFactor;
        iceFriction = ice.iceFriction; // Set the friction based on IcePhysics component

        var material = rb.sharedMaterial;
        material.friction = iceFriction;
        rb.sharedMaterial = material;
    }

    void ApplyNormalPhysics()
    {
        iceControlFactor = 1f;

        var material = rb.sharedMaterial;
        material.friction = normalFriction;
        rb.sharedMaterial = material;
    }

    public void CollectStaff()
    {
        hasStaff = true;

        if (goblinSpawner != null)
        {
            goblinSpawner.SpawnGoblins();
        }

        OnStaffCollected?.Invoke();
    }

    public void CollectKey()
    {
        keyCollected = true;
        soundEffects.PlayOneShot(keySound); // Play the key collection sound

        // Notify the door about key collection
        if (door != null)
        {
            door.KeyCollected();
        }
    }

    public void Die()
    {
        if (isTakingDamage)
        {
            return;
        }
        isTakingDamage = true;
        lives--;

        if (lives <= 0)
        {
            PlayGameOverSoundEffect();
            isFirstLoad = true;
            SceneManager.LoadScene(3);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void PlayGameOverSoundEffect()
    {
        if (gameOverSound != null)
        {
            AudioSource.PlayClipAtPoint(gameOverSound, transform.position);
        }
    }

    void StopLevelTheme()
    {
        backgroundMusic.Stop();
    }

    private void OnDestroy()
    {
        PlayerController.OnStaffCollected -= SwitchToBossFightMusic;
    }

    public void TakeDamage()
    {
        if (!isTakingDamage)
        {
            StartCoroutine(TakeDamageCoroutine());
        }
    }

    private IEnumerator TakeDamageCoroutine()
    {
        isTakingDamage = true;
        lives--;
        if (lives <= 0)
        {
            isFirstLoad = true;
            SceneManager.LoadScene(3);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        yield return new WaitForSeconds(damageDelay);
        isTakingDamage = false;
    }
}