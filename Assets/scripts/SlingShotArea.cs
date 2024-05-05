using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;


public class SlingShotArea : MonoBehaviour
{

    [SerializeField] private LayerMask _slingShotAreaMask; 

    public bool isWithinSlingShotArea()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // checks if the point overlaps any collider on the _slingShotAreaMask layer
        if (Physics2D.OverlapPoint(worldPosition, _slingShotAreaMask)){
            return true; 
        }
        
        return false;
    }
}
