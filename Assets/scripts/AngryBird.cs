using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class AngryBird : MonoBehaviour
{
    private Rigidbody2D _rb;
    private CircleCollider2D _circleColider;

    private bool _hasBeenLaunched;

    [SerializeField] private AudioClip _hittingSound;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        //another method to SerializeField 
        _rb = GetComponent<Rigidbody2D>();
        // turns off gravity
        _rb.isKinematic = true;

        _circleColider = GetComponent<CircleCollider2D>();
        _circleColider.enabled = false;
    }

    // runs 50 times per second by default
    private void FixedUpdate()
    {
        if (_hasBeenLaunched)
        {
            // the tranform refers the the tranform of the game object this script is attached to (to make the angry bird face the velocity direction while in air)
            transform.right = _rb.velocity;
        }
    }

    public void LaunchBird(Vector2 direction, float force)
    {
       
        _rb.isKinematic = false;
        _circleColider.enabled = true;
        _hasBeenLaunched = true;

        // applying force
        _rb.AddForce(direction * force, ForceMode2D.Impulse); 
    }
    
    // when the collider touchs an another collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _hasBeenLaunched = false;
        SoundManager.instance.PlayClip(_hittingSound, _audioSource);
        Destroy(this);
    }
}
