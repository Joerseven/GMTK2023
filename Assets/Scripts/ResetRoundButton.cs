using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRoundButton : MonoBehaviour
{
    public Sprite onDown;
    public Sprite onUp;
    private void OnMouseDown()
    {
        GetComponentInParent<GameLoop>().BeginLevel();
        GetComponent<SpriteRenderer>().sprite = onDown;
    }

    private void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().sprite = onUp;
    }
}
