using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginTurnsButton : MonoBehaviour
{
    // Start is called before the first frame update
    void OnMouseDown()
    {
        GetComponentInParent<GameLoop>().StartTurns();
    }
}
