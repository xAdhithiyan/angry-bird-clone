using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// input system is a package downloaded from the package manager in unity engine (a newer input system compared to the legacy input)
using UnityEngine.InputSystem; 

public class NewBehaviourScript : MonoBehaviour
{
    // serializefield is to expose the unity vairable in the engine so it can be assigned to the a particular gameObject
    [SerializeField] private LineRenderer _leftLineRenderer;
    [SerializeField] private LineRenderer _rightLineRenderer;

    // to get the static value of one end of the slingshot 
    [SerializeField]private Transform _leftStartPosition;
    [SerializeField] private Transform _rightStartPosition;  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(Mouse.current.leftButton.isPressed)
        {
            DrawSlingShot();
        }
    }
    
    private void DrawSlingShot()
    {
        // Mouse.current.position.ReadValue() gives the mouse position in pixels values. but we want the world position 
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        setLines(touchPosition);
    }

    private void setLines(Vector2 touchPosition)
    {
        _leftLineRenderer.SetPosition(0, touchPosition);
        _leftLineRenderer.SetPosition(1, _leftStartPosition.position);

        _rightLineRenderer.SetPosition(0, touchPosition);
        _rightLineRenderer.SetPosition(1, _rightStartPosition.position);

    }
}
