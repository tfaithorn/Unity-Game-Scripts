using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ErrorPanel : MonoBehaviour
{
    public TextMeshProUGUI text;
    public InventoryController inventoryController;
    private float fadeTime = 3f;
    private float holdTime = 1f;
    Coroutine coroutine;


    public enum MessageType { 
        INVENTORY_FULL
    }

    public void SetMessage(MessageType messageType)
    {
        switch (messageType) {
            case MessageType.INVENTORY_FULL:
                //text.text = LanguageController.GetPhrase("errorMessage.inventoryIsFull");
                break;
        }

        //text.color = new Color(text.color.r, text.color.g, text.color.b,1);
        text.alpha = 1;

        coroutine = StartCoroutine(FadePanel());
    }

    public void PanelVisible()
    { 
        
    }


    public void InventoryFull()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        SetMessage(MessageType.INVENTORY_FULL);
    }


    
    IEnumerator FadePanel()
    {

        yield return new WaitForSecondsRealtime(holdTime);


        Timer timer = new Timer(fadeTime);

        while (timer.durationPassed < fadeTime)
        {

            float relativePercentage = (1 - ((timer.durationPassed - holdTime) / timer.endTime));
            text.alpha = relativePercentage;

            timer.durationPassed += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

    }

}
