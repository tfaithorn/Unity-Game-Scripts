using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSlider : MonoBehaviour
{
    public UIController uIController;

    private Reticle reticle;

    //note: use event in target reticle
    public Slider enemyHealthSlider;
    StatsController enemyStatsController;
    public Text healthText;

    private bool isSubscribed = false;

    private void Awake()
    {
        reticle = uIController.reticle.GetComponent<Reticle>();
    }

    public void Start()
    {
        reticle.reticleTargetChange += SetEnemyStatsController;
    }

    public void SetEnemyStatsController(CharacterMB character)
    {
        //clear existing statsController event

        if (isSubscribed)
        {
            enemyStatsController.updateHealthEvent -= UpdateEnemyHealthSlider;
        }

        if (character.statsController != null)
        {
            //bind event to change slider value
            enemyStatsController = character.statsController;
            enemyStatsController.updateHealthEvent += UpdateEnemyHealthSlider;

            float enemyHealth = enemyStatsController.GetStatValue(StatsController.StatType.CURR_HEALTH);
            float enemyMaxHealth = enemyStatsController.GetStatValue(StatsController.StatType.MAX_HEALTH);

            UpdateEnemyHealthSlider(enemyHealth, enemyMaxHealth);

            isSubscribed = true;
        }
    }

    private void UpdateEnemyHealthSlider(float currentHealth, float maxHealth)
    {
        enemyHealthSlider.maxValue = maxHealth;
        enemyHealthSlider.value = currentHealth;
        //enemyHealthSlider.text = currentHealth + " / " + maxHealth;
    }

}
