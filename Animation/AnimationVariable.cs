using System.Collections;
using System.Collections.Generic;

public class AnimationVariable
{
    public bool hasBool;
    public bool hasFloat;
    public bool hasInt;

    public string name;
    public bool boolValue;
    public float floatValue;
    public int intValue;

    public AnimationVariable(string name, float value)
    {
        this.name = name;
        this.hasFloat = true;
        this.floatValue = value;
    }

    public AnimationVariable(string name, int value)
    {
        this.name = name;
        this.hasInt = true;
        this.intValue = value;
    }

    public AnimationVariable(string name, bool value)
    {
        this.name = name;
        this.hasBool = true;
        this.boolValue = value;
    }
}