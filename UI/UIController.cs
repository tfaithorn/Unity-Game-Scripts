using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public PlayerCharacterMB playerCharacterController;
    public MenuController menuController;

    //keybinding buttons
    [Header("Keybind Buttons")]

    [Header("In Game UI")]
    public RectTransform reticle;
    public RectTransform abilityPanel;

    [Header("Item Description")]
    public Text ItemDetailsName;
    public Text ItemDetailsDescription;
    public Text ItemDetailsStats;
}
