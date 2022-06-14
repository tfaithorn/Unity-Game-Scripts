using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseMessage
{
    public bool result;
    public string message;

    public ResponseMessage(bool result, string message)
    {
        this.result = result;
        this.message = message;
    }    
}
