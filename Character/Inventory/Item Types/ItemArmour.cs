using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemArmour : Item
{
    public enum ArmourType { 
        CHEST,
        HEAD,
        FEET,
        HANDS,
        RINGS,
        NECK,
        LEGS
    }

    public ArmourType armourType;
}
