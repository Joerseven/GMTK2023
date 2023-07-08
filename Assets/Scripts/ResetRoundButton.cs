using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRoundButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        GetComponentInParent<GameLoop>().BeginLevel();
    }
}
