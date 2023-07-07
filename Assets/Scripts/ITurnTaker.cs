using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurnTaker
{
    public IQueueItem[] TakeTurn();
}
