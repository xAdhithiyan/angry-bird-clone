using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryBird : MonoBehaviour
{
    private Rigidbody2D _rb;
    private CircleCollider2D _circleColider;

    private void Awake()
    {
        //another method to SerializeField 
        _rb = GetComponent<Rigidbody2D>();
        // turns off gravity
        _rb.isKinematic = true;

        _circleColider = GetComponent<CircleCollider2D>();
        _circleColider.enabled = false;
    }


    public void launchBird(Vector2 direction, float force)
    {
        _rb.isKinematic = false;
        _circleColider.enabled = true;

        // applying force
        _rb.AddForce(direction * force, ForceMode2D.Impulse); 
    }
}
