using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginTurnsButton : MonoBehaviour
{
    public Sprite onDown;
    public Sprite onUp;
    void OnMouseDown()
    {
        GetComponentInParent<GameLoop>().StartTurns();
        GetComponent<SpriteRenderer>().sprite = onDown;
    }

    void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().sprite = onUp;
    }
}
