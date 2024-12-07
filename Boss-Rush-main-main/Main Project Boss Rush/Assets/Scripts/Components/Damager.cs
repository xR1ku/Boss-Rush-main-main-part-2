using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damager : MonoBehaviour
{
    [SerializeField] int damageAmount;
    [SerializeField] float knockbackForce = 1;
    [SerializeField] GameObject hitEffectPrefab;
    [SerializeField] AudioClipCollection hitSounds;

    public UnityEvent OnContact;
    public UnityEvent OnSuccessfulHit;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Damager collided with: " + other.name); // Log the collision

        OnContact?.Invoke();

        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable != null)
        {
            Debug.Log("Found Damageable on: " + other.name); // Log when Damageable is found

            Vector3 dir = other.transform.position - transform.position;
            dir.Normalize();

            Damage damage = new Damage
            {
                amount = damageAmount,
                direction = dir,
                knockbackForce = knockbackForce
            };

            if (damageable.Hit(damage))
            {
                Debug.Log("Successful hit! Dealt " + damageAmount + " damage to: " + other.name); // Log successful hit
                OnSuccessfulHit?.Invoke();

                if (hitEffectPrefab != null)
                {
                    Instantiate(hitEffectPrefab, other.transform.position, Quaternion.identity);
                }

                if (hitSounds != null)
                {
                    SoundEffectsManager.instance.PlayRandomClip(hitSounds.clips, true);
                }
            }
            else
            {
                Debug.Log("Hit was not successful. Enemy might be invincible or already dead."); // Log failed hit
            }
        }
    }
}

public class Damage
{
    public int amount = 0;
    public Vector3 direction = Vector3.zero;
    public float knockbackForce = 1;
}
