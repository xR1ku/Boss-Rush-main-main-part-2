using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] float iTime = 0.5f;
    [SerializeField] Material flashMaterial;
    [SerializeField] Renderer[] renderers;
    [SerializeField] GameObject damageEffectPrefab;

    [SerializeField] AudioClipCollection hurtSounds;
    [SerializeField] AudioClipCollection deathSounds;

    public UnityEvent<int> OnInitialize;
    public UnityEvent<Damage> OnHit;
    public UnityEvent OnDeath;
    public UnityEvent<int, int> OnHealthChanged;

    int currentHealth;
    float timeSinceHit = 0;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log(gameObject.name + " initialized with health: " + maxHealth); // Log initial health

        OnInitialize?.Invoke(maxHealth);
        OnHealthChanged?.Invoke(maxHealth, maxHealth);
    }

    private void Update()
    {
        timeSinceHit += Time.deltaTime;
    }

    public bool Hit(Damage damage)
    {
        Debug.Log(gameObject.name + " was hit with damage: " + damage.amount); // Log the hit

        if (timeSinceHit < iTime)
        {
            Debug.Log("Ignored hit due to invincibility frames."); // Log invincibility
            return false;
        }

        if (currentHealth == 0)
        {
            Debug.Log("Ignored hit because already dead."); // Log dead state
            return false;
        }

        if (flashMaterial != null)
        {
            StartHitFlash();
        }

        timeSinceHit = 0;
        currentHealth -= damage.amount;

        Debug.Log("New health for " + gameObject.name + ": " + currentHealth); // Log health update

        OnHit?.Invoke(damage);
        OnHealthChanged?.Invoke(damage.amount, currentHealth);

        if (hurtSounds != null)
        {
            SoundEffectsManager.instance.PlayRandomClip(hurtSounds.clips, true);
        }

        if (damageEffectPrefab != null)
        {
            Instantiate(damageEffectPrefab, transform.position, Quaternion.identity);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }

        return true;
    }

    void Death()
    {
        Debug.Log(gameObject.name + " has died."); // Log death
        OnDeath?.Invoke();

        if (deathSounds != null)
        {
            SoundEffectsManager.instance.PlayRandomClip(deathSounds.clips, true);
        }
    }

    public void ResetIFrames()
    {
        timeSinceHit = 0;
    }

    public void StartHitFlash()
    {
        if (timeSinceHit < iTime)
        {
            Debug.Log("Skipping flash; still in invincibility time."); // Log skipped flash
            return;
        }

        foreach (var renderer in renderers)
        {
            StartCoroutine(HandleFlashMaterialSwap(renderer));
        }
    }

    IEnumerator HandleFlashMaterialSwap(Renderer renderer)
    {
        Material[] originalMats = new Material[renderer.materials.Length];

        for (int i = 0; i < originalMats.Length; i++)
        {
            originalMats[i] = renderer.materials[i];
        }

        Material[] newMats = new Material[renderer.materials.Length];

        for (int i = 0; i < newMats.Length; i++)
        {
            newMats[i] = flashMaterial;
        }

        renderer.materials = newMats;

        yield return new WaitForSeconds(iTime * 0.9f);

        renderer.materials = originalMats;
    }

    [ContextMenu("Test Hit")]
    public void TestHit()
    {
        Damage test = new Damage
        {
            amount = 1,
            direction = Vector3.zero,
            knockbackForce = 0
        };

        Hit(test);
    }
}
