using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class StatRowDamage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public StatsDamageRowToolTipController statsDamageRowToolTipController;
    public TextMeshProUGUI name;
    public TextMeshProUGUI value;
    public StatsController statsController;
    private RectTransform myRect;

    private void Start()
    {
        name.text = LanguageController.GetPhrase("meleeDamage.label");
        myRect = this.GetComponent<RectTransform>();

        string totalMeleeDamage = statsController.GetAddedWeaponDamage().ToString();
        value.text = totalMeleeDamage;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        statsDamageRowToolTipController.SetToolTip(statsController);
        float xPos = this.myRect.position.x - (this.myRect.rect.width / 2) - statsDamageRowToolTipController.panel.rect.width + 10;
        float yPos = this.myRect.position.y - (this.myRect.rect.height / 2) - (statsDamageRowToolTipController.panel.rect.height / 2);

        statsDamageRowToolTipController.panel.anchoredPosition = new Vector2(xPos, yPos);
        statsDamageRowToolTipController.Activate();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        statsDamageRowToolTipController.Deactivate();
    }
}
