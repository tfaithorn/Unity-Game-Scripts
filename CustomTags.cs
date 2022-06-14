using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTags : MonoBehaviour
{
    public enum TagTypes { 
        SLOPE = 1
    }

    public List<TagTypes> tags;

    public bool HasTag(TagTypes tag) {
        if (tags.Contains(tag)) {
            return true;
        }
        return false;
    }

}
