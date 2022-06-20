using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceSlidersPanel : MonoBehaviour
{
    public UIController uIController;
    StatsController statsController;

    public Slider healthSlider;
    public Slider energySlider;

    public Text healthText;
    public Text energyText;

    private void Awake()
    {
        statsController = uIController.playerCharacterController.statsController;
    }

    private void Start()
    {
        statsController.updateHealthEvent += UpdateHealthSlider;
        statsController.updateEnergyEvent += UpdateManaSlider;
    }

    private void UpdateHealthSlider(float currentHealth, float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        healthText.text = currentHealth + " / " + maxHealth;
    }

    private void UpdateManaSlider(float currentEnergy, float maxEnergy)
    {
        energySlider.maxValue = maxEnergy;
        energySlider.value = currentEnergy;
        energyText.text = currentEnergy + " / " + maxEnergy;
    }

}
