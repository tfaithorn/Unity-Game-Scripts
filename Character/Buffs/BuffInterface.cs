using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BuffInterface
{
    void BuffStart(Buff buff);

    void BuffTick();

    void BuffRemove();

    void BuffFinish();
}
