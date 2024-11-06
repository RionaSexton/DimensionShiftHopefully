using UnityEngine;
using System.Collections;

public class EnemyMistAttack : MonoBehaviour
{
    public float damageInterval = 2f;    // Time between each damage tick
    public int damageAmount = 1;         // Damage dealt to the player per tick
    public int enemyHealth = 3;          // Enemy's health
    public float deathAnimationDuration = 1.5f; // Duration to allow death animation

    private bool playerInMist = false;
    private float nextDamageTime;
    private GameObject player;
    private MistEnemyAnim animationController;
    public GameObject mist1;
    public GameObject mist2;

    private void Start()
    {
        // Find the parent animation controller
        animationController = GetComponentInParent<MistEnemyAnim>();
        if (animationController == null)
        {
            Debug.LogError("MistEnemyAnim component is missing on the parent object.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInMist = true;
            player = other.gameObject;
            nextDamageTime = Time.time + damageInterval;
            mist1.gameObject.SetActive(true);
            mist2.gameObject.SetActive(true);

            // Trigger attack animation on the parent controller
            animationController?.PlayAttackAnimation();
            StartCoroutine(ResetAttackAnimation());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInMist = false;
            player = null;
            mist1.gameObject.SetActive(false);
            mist2.gameObject.SetActive(false);

            // Reset attack animation on the parent controller
            animationController?.ResetAttackAnimation();
        }
    }

    private void Update()
    {
        if (playerInMist && Time.time >= nextDamageTime)
        {
            player?.GetComponent<Health>().TakeDamage(damageAmount);
            nextDamageTime = Time.time + damageInterval;
        }
    }

    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Trigger the death animation on the parent controller
        animationController?.PlayDeathAnimation();
        StartCoroutine(DelayedDeath());
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Adjust to attack animation duration
        animationController?.ResetAttackAnimation();
    }

    private IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(deathAnimationDuration);
        playerInMist = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }
}
