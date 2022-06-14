using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProperty
{
    public enum PropertyType
    {
        STAT = 1,
        SCRIPT = 2
    }

    public PropertyType propertyType;
    public string phraseIdentifier;
    public float value;
    public StatsController.StatType statType;

    //TODO: add interface for script with add & remove methods

    public ItemProperty(PropertyType propertyType, string phraseIdentifier, float value) 
    {
        this.propertyType = propertyType;
        this.phraseIdentifier = phraseIdentifier;
        this.value = value;
    }
}
