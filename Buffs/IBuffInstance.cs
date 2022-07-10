using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffInstance
{
    void BuffStart(BuffInstance buffCharacter);

    void BuffTick();

    void BuffRemove();

    void BuffFinish();
}
