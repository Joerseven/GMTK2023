using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQueueItem
{
    public bool IsFinished();
    public void Update(float dt);
}
