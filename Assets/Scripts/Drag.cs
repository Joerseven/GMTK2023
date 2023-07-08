using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private bool isDragging;
    private Vector3 mouseOffset;
    private Movement movement;
    private MovementSpaces spaces;

    private void OnMouseDown()
    {
        isDragging = true;
        mouseOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spaces.DeShelfMole(GetComponent<Mole>());
    }

    private void OnMouseUp()
    {
        isDragging = false;
        var pos = movement.GetPosition();
        if (spaces.ValidPlacement(pos.x, pos.y))
        {
            movement.SnapTo(pos.x, pos.y);
            return;
        };
        spaces.ShelfMole(GetComponent<Mole>());
    }
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        spaces = GetComponentInParent<MovementSpaces>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            var newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + mouseOffset;
            transform.position = new Vector3(newPos.x, newPos.y, 0);
        }
    }
}
