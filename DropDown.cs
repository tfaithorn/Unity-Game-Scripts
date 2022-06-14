using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDown : MonoBehaviour
{
    [HideInInspector]
    public int listIdx;
    [HideInInspector]
    public List<string> MyList = new List<string>() {"test 1","test 2"};
}
