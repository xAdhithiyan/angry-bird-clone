using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piggy : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 3f;
    [SerializeField] private float _damageThreshold = 0.2f;
    [SerializeField] private GameObject _piggieDeathField;
    [SerializeField] private AudioClip _popingSound;
    private float _currentHealth;

    private AudioSource _audioSouce;

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
        //particle effect
        Instantiate(_piggieDeathField, transform.position, Quaternion.identity);

        //if a AudioSource game object is setup(like on other c# scripts) it will get destroyed here since we are destorying the entire game object
        AudioSource.PlayClipAtPoint(_popingSound, transform.position);
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
