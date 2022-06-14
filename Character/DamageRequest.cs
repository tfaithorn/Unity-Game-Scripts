using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRequest
{
    public StatsController sourceStatsController;
    public bool isMelee;
    public bool isSpell;
    public bool isCrit;
    public float addedCritChance;
    public float addedCritMultiplier;
    public float addedCritChancePercentage;
    public float addedCritMultiplierPercentage;

    public Dictionary<StatsController.DamageType, float> values;

    public DamageRequest()
    {
        this.isMelee = false;
        this.isSpell = false;
        this.isCrit = false;
        this.addedCritChance = 0;
        this.addedCritMultiplier = 0;
        this.addedCritChancePercentage = 0;
        this.addedCritMultiplierPercentage = 0;

        values = new Dictionary<StatsController.DamageType, float>()
        {
           {StatsController.DamageType.PHYSICAL, 0f},
           {StatsController.DamageType.FIRE, 0f},
           {StatsController.DamageType.COLD, 0f},
           {StatsController.DamageType.LIGHTNING, 0f},
           {StatsController.DamageType.CHAOS, 0f},
           {StatsController.DamageType.HOLY, 0f},
           {StatsController.DamageType.UNHOLY, 0f},
           {StatsController.DamageType.POISON, 0f},
           {StatsController.DamageType.ARCANE, 0f},
           {StatsController.DamageType.SHADOW, 0f}
        };

        sourceStatsController = null;

    }

    public void AddDamage(StatsController.DamageType type, float value)
    {
        this.values[type] += value;
    }

}
