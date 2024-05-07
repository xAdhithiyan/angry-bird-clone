using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piggy : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 3f;
    [SerializeField] private float _damageThreshold = 0.2f;
    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void DamangePiggy(float damangeAmount)
    {
        _currentHealth -= damangeAmount;

        if(_currentHealth <= 0f ) {
            Die();
        }
    }

    public void Die()
    {
        GameManager.instance.RemovePiggy(this);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // damange is calculated based on velocity -> here collision has info on the other collider that this collider strikes
        float ImpactVelocity = collision.relativeVelocity.magnitude;

        if(ImpactVelocity > _damageThreshold) { 
            DamangePiggy(ImpactVelocity);
        }
    }
}
