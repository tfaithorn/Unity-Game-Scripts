using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MenuPanel
{
    public Canvas canvas;
    public RectTransform inventoryListPanel;
    public RectTransform statsContentPanel;
    public PlayerCharacterController playerCharacterController;
    public ItemMouseOverTooltipController itemMouseOverTooltipController;
    public StatMouseOverTooltipController statMouseOverTooltipController;
    private InventoryController inventoryController;
    private StatsController statsController;

    public Slider healthSlider;
    public Slider energySlider;

    private void Awake()
    {
        statsController = playerCharacterController.statsController;
    }

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        SetSliders();
        ClearItemList();
        GetInventoryList(0);
    }

    /// <summary>
    /// Removes item prefabs from list. It should really be using pooling instead of deleting and instantiating them, 
    /// but we will cross that bridge when it becomes an issue (i.e. using PoolingHelper).
    /// </summary>
    private void ClearItemList()
    {

        foreach (Transform child in inventoryListPanel)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void SetInventoryList(List<CharacterItem> characterItems)
    {
        foreach (CharacterItem characterItem in characterItems)
        {
            ItemUIPrefab itemUIPrefab = Resources.Load<ItemUIPrefab>("Prefabs/UI Prefabs/Item Panel");
            itemUIPrefab.SetItem(characterItem.item, itemMouseOverTooltipController, canvas);
            Instantiate(itemUIPrefab, inventoryListPanel);
        }
    }

    public void GetInventoryList(int itemType)
    {
        ClearItemList();
        SetInventoryList(playerCharacterController.inventoryController.GetByType((ItemCategory.CategoryType)itemType));
    }

    private void SetSliders()
    {
        float maxHealth = statsController.GetStatValue(StatsController.StatType.MAX_HEALTH);
        float currHealth = statsController.GetStatValue(StatsController.StatType.CURR_HEALTH);

        float maxEnergy = statsController.GetStatValue(StatsController.StatType.MAX_ENERGY);
        float currEnergy = statsController.GetStatValue(StatsController.StatType.CURR_ENERGY);

        float healthRatio = currHealth / maxHealth;
        float energyRatio = currEnergy / maxEnergy;

        healthSlider.value = healthRatio;

        energySlider.value = energyRatio;

    }
}
