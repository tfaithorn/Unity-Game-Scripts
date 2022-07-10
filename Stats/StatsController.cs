using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Controls the stats of characters.
/// Stats are calculated at runtime based on gear, buffs & base stats
/// </summary>
public class StatsController : MonoBehaviour
{
    public delegate void UpdateHealthEvent(float currentHealth, float MaxHealth);
    public event UpdateHealthEvent updateHealthEvent;

    public delegate void UpdateEnergyEvent(float currentEnergy, float MaxEnergy);
    public event UpdateEnergyEvent updateEnergyEvent;

    InventoryController inventoryController;

    public enum DamageType {
        PHYSICAL,
        FIRE,
        COLD,
        LIGHTNING,
        CHAOS,
        HOLY,
        UNHOLY,
        POISON,
        SHADOW,
        ARCANE
    }

    public enum StatType {
        MAX_HEALTH,
        MAX_ENERGY,
        CURR_HEALTH,
        CURR_ENERGY,
        HEALTH_PERCENTAGE,
        ENERGY_PERCENTAGE,
        ATTACK_SPEED,
        CAST_SPEED,
        BLOCK_EFFECTIVENESS,
        MOVEMENT_SPEED,
        CRIT_CHANCE,
        CRIT_MULTIPLIER,
        RESISTANCE_PHYSICAL,
        RESISTANCE_FIRE,
        RESISTANCE_COLD,
        RESISTANCE_LIGHTNING,
        RESISTANCE_CHAOS,
        RESISTANCE_HOLY,
        RESISTANCE_UNHOLY,
        RESISTANCE_POISON,
        RESISTANCE_ARCANE,
        RESISTANCE_SHADOW,
        RESISTANCE_SPELL,
        RESISTANCE_MELEE,
        RESISTANCE_PROJECTILE,
        MELEE_PHYSICAL_DAMAGE,
        MELEE_FIRE_DAMAGE,
        MELEE_COLD_DAMAGE,
        MELEE_LIGHTNING_DAMAGE,
        MELEE_CHAOS_DAMAGE,
        MELEE_HOLY_DAMAGE,
        MELEE_UNHOLY_DAMAGE,
        MELEE_POISON_DAMAGE,
        MELEE_ARCANE_DAMAGE,
        MELEE_SHADOW_DAMAGE,
        SPELL_PHYSICAL_DAMAGE,
        SPELL_FIRE_DAMAGE,
        SPELL_COLD_DAMAGE,
        SPELL_LIGHTNING_DAMAGE,
        SPELL_CHAOS_DAMAGE,
        SPELL_HOLY_DAMAGE,
        SPELL_UNHOLY_DAMAGE,
        SPELL_POISON_DAMAGE,
        SPELL_ARCANE_DAMAGE,
        SPELL_SHADOW_DAMAGE,
        HEALING_EFFECTIVENESS,
        MELEE_DAMAGE_LEECHED,
        SPELL_DAMAGE_LEECHED,
        HEALTH_REGEN_PER_SECOND,
        MIN_WEAPON_DAMAGE,
        MAX_WEAPON_DAMAGE,
        PERCENTAGE_PHYSICAL_INCREASE,
        PERCENTAGE_FIRE_INCREASE,
        PERCENTAGE_COLD_INCREASE,
        PERCENTAGE_LIGHTNING_INCREASE,
        PERCENTAGE_CHAOS_INCREASE,
        PERCENTAGE_HOLY_INCREASE,
        PERCENTAGE_UNHOLY_INCREASE,
        PERCENTAGE_POISON_INCREASE,
        PERCENTAGE_ARCANE_INCREASE,
        PERCENTAGE_SHADOW_INCREASE,
        MAX_RESISTANCE_PHYSICAL,
        MAX_RESISTANCE_FIRE,
        MAX_RESISTANCE_COLD,
        MAX_RESISTANCE_LIGHTNING,
        MAX_RESISTANCE_CHAOS,
        MAX_RESISTANCE_HOLY,
        MAX_RESISTANCE_UNHOLY,
        MAX_RESISTANCE_POISON,
        MAX_RESISTANCE_SHADOW,
        MAX_RESISTANCE_ARCANE,
    }

    private class StatsGrouping {
        public StatType resistance;
        public StatType maxResistance;
        public StatType MeleeDamage;
        public StatType spellDamage;
        public StatType percentageDamage;

        public StatsGrouping(StatType resistance, StatType maxResistance, StatType meleeDamage, StatType spellDamage, StatType percentageDamage)
        {
            this.resistance = resistance;
            this.maxResistance = maxResistance;
            this.MeleeDamage = meleeDamage;
            this.spellDamage = spellDamage;
            this.percentageDamage = percentageDamage;
        }
    }

    private static Dictionary<DamageType, StatsGrouping> statGroupings = new Dictionary<DamageType, StatsGrouping>()
    {
        {DamageType.PHYSICAL, new StatsGrouping(StatType.RESISTANCE_PHYSICAL, StatType.MAX_RESISTANCE_PHYSICAL, StatType.MELEE_PHYSICAL_DAMAGE, StatType.SPELL_PHYSICAL_DAMAGE, StatType.PERCENTAGE_PHYSICAL_INCREASE) },
        {DamageType.FIRE, new StatsGrouping(StatType.RESISTANCE_FIRE, StatType.MAX_RESISTANCE_FIRE, StatType.MELEE_FIRE_DAMAGE, StatType.SPELL_FIRE_DAMAGE, StatType.PERCENTAGE_FIRE_INCREASE) },
        {DamageType.COLD, new StatsGrouping(StatType.RESISTANCE_COLD, StatType.MAX_RESISTANCE_COLD, StatType.MELEE_COLD_DAMAGE, StatType.SPELL_COLD_DAMAGE, StatType.PERCENTAGE_COLD_INCREASE) },
        {DamageType.LIGHTNING, new StatsGrouping(StatType.RESISTANCE_LIGHTNING, StatType.MAX_RESISTANCE_LIGHTNING, StatType.MELEE_LIGHTNING_DAMAGE, StatType.SPELL_LIGHTNING_DAMAGE, StatType.PERCENTAGE_LIGHTNING_INCREASE) },
        {DamageType.HOLY, new StatsGrouping(StatType.RESISTANCE_HOLY, StatType.MAX_RESISTANCE_HOLY, StatType.MELEE_HOLY_DAMAGE, StatType.SPELL_HOLY_DAMAGE, StatType.PERCENTAGE_HOLY_INCREASE) },
        {DamageType.UNHOLY, new StatsGrouping(StatType.RESISTANCE_UNHOLY, StatType.MAX_RESISTANCE_UNHOLY, StatType.MELEE_UNHOLY_DAMAGE, StatType.SPELL_UNHOLY_DAMAGE, StatType.PERCENTAGE_UNHOLY_INCREASE) },
        {DamageType.CHAOS, new StatsGrouping(StatType.RESISTANCE_CHAOS, StatType.MAX_RESISTANCE_CHAOS, StatType.MELEE_CHAOS_DAMAGE, StatType.SPELL_CHAOS_DAMAGE, StatType.PERCENTAGE_CHAOS_INCREASE) },
        {DamageType.POISON, new StatsGrouping(StatType.RESISTANCE_POISON, StatType.MAX_RESISTANCE_POISON, StatType.MELEE_POISON_DAMAGE, StatType.SPELL_POISON_DAMAGE, StatType.PERCENTAGE_POISON_INCREASE) },
        {DamageType.ARCANE, new StatsGrouping(StatType.RESISTANCE_ARCANE, StatType.MAX_RESISTANCE_ARCANE, StatType.MELEE_ARCANE_DAMAGE, StatType.SPELL_ARCANE_DAMAGE, StatType.PERCENTAGE_ARCANE_INCREASE) }
    };

    [Serializable]
    public class BaseStat {
        public StatType statType;
        public float value;

        public BaseStat(StatType statType, float value)
        {
            this.statType = statType;
            this.value = value;
        }
    }

    [SerializeField][HideInInspector] public List<BaseStat> baseStats = new List<BaseStat>();

    //build stats from gear & buffs and store in dictionary
    private Dictionary<StatType, float> stats = new Dictionary<StatType, float>()
    {
        {StatType.MAX_HEALTH, 0f},
        {StatType.MAX_ENERGY, 0f},
        {StatType.CURR_HEALTH, 0f},
        {StatType.CURR_ENERGY, 0f},
    };

    List<StatSource> statSources = new List<StatSource>();
    List<OnHitEffect> onHitEffects = new List<OnHitEffect>();

    public void CreateStatSourceFromGear()
    {
        /*
        statSources.RemoveAll(x => x.sourceType == StatSource.SourceType.GEAR);

        //iterate through gear and add stats
        foreach (var itemDict in inventoryController.equiptItems)
        {
            Item item = itemDict.Value;

            if (item != null)
            {
                foreach (ItemProperty property in item.itemProperties)
                {
                    if (property.propertyType == ItemProperty.PropertyType.STAT)
                    {
                        statSources.Add(new StatSource(property.statType, property.value, StatSource.SourceType.GEAR, item.id));
                    }
                }
            }
        }
        */

        /*
        foreach (Item item in inventoryController.GetInventory())
        {
            if (inventoryItem.isEquipt == true)
            {
                foreach (ItemProperty property in inventoryItem.item.properties)
                {
                    if (property.propertyType == ItemProperty.propertyTypes.STAT_INCREASE)
                    {
                        statSources.Add(new StatSource(property.statType, property.value, StatSource.SourceType.GEAR, inventoryItem.item.id));
                    }
                }
            }
        }
        */
    }

    private void CreateStatSourcesFromBaseStats()
    {
        foreach (BaseStat baseStat in baseStats)
        {
            statSources.Add(new StatSource(baseStat.statType, baseStat.value, StatSource.SourceType.BASE, 0));
        }
    }

    public void CreateStatSourcesFromBuffs()
    {
        foreach (BuffInstance buffInstance in buffInstances)
        {
            var buff = buffInstance.buff;
            var statIncreases = buff.GetStatIncreases();

            if (statIncreases == null)
            {
                continue;
            }

            foreach (var item in statIncreases)
            {
                statSources.Add(new StatSource(item.Key, item.Value, StatSource.SourceType.BUFF, buff.id));
            }
        }
    }

    private void BuildStats()
    {
        foreach (StatSource statSource in statSources)
        {
            stats[statSource.statType] = GetStatValue(statSource.statType) + statSource.value;
        }
    }

    private void RemoveBuffsFromStatSources()
    {
        statSources.RemoveAll(x => x.sourceType == StatSource.SourceType.BUFF);
    }

    private readonly List<BuffInstance> buffInstances = new List<BuffInstance>();

    private void Awake()
    {
        inventoryController = GetComponent<InventoryController>();
    }

    private void Start()
    {
        InitialiseStats();
        BuildStats();
        updateHealthEvent?.Invoke(GetStatValue(StatType.CURR_HEALTH), GetStatValue(StatType.MAX_HEALTH));
        updateEnergyEvent?.Invoke(GetStatValue(StatType.CURR_ENERGY), GetStatValue(StatType.MAX_ENERGY));

        //for testing
        var characterMb = this.gameObject.GetComponent<CharacterMB>();

        BuffOptions buffOptions = new BuffOptions();
        buffOptions.duration = 5;

        ApplyBuff(BuffDictionary.buffs[1], characterMb, characterMb, buffOptions);
    }

    private void InitialiseStats()
    {
        CreateStatSourcesFromBaseStats();
        CreateStatSourcesFromBuffs();
    }

    /// <summary>
    /// Adds a buff to a character.
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="creator"></param>
    /// <param name="target"></param>
    public void ApplyBuff(Buff buff, CharacterMB creator, CharacterMB target, BuffOptions options)
    {
        BuffInstance buffInstance = new BuffInstance(buff, creator, target, options);
        buffInstances.Add(buffInstance);
    }

    /// <summary>
    /// Return a list of status types afflicting the character.
    /// If the character is afflected by more than 1 status of the same type, only 1 will be returned.
    /// </summary>
    /// <returns></returns>
    public List<Buff.StatusType> GetStatuses() 
    {
        List<Buff.StatusType> statusTypes = new List<Buff.StatusType>();

        foreach (BuffInstance buffInstance in buffInstances)
        {
            var buff = buffInstance.buff;
            if (buff.statusType != Buff.StatusType.NONE && !statusTypes.Exists(x => x == buff.statusType))
            {
                statusTypes.Add(buff.statusType);
            }
        }

        return statusTypes;
    }

    public void RemoveBuff(BuffInstance buffInstance)
    {
        buffInstances.Remove(buffInstance);

        if (buffInstance.buff.hasComponent)
        {
            Destroy((Component)buffInstance.buffInterface);
        }
    }

    private void Update()
    {
        UpdateBuffs();
    }

    /// <summary>
    /// Increments buffs and calls tick functions. 
    /// TODO: look at moving buffs that have ticks or durations to separate lists to increase performance
    /// </summary>
    private void UpdateBuffs()
    {
        for (int i = 0; i < buffInstances.Count; i++)
        {
            var buffInstance = buffInstances[i];

            buffInstance.timer.durationPassed += Time.deltaTime;

            if (buffInstance.hasTick)
            {
                float nextTick = (buffInstance.currentTick + 1) * buffInstance.tickRate;
                if ((buffInstance.timer.durationPassed / buffInstance.tickRate) > nextTick)
                {
                    buffInstance.currentTick++;

                    if (buffInstance.buff.hasComponent)
                    {
                        buffInstance.buffInterface.BuffTick();
                    }
                }
            }

            if ((buffInstance.timer.durationPassed >= buffInstance.timer.endTime) && !buffInstance.isPermanent)
            {
                if (buffInstance.buff.hasComponent)
                {
                    buffInstances[i].buffInterface.BuffFinish();
                }
                RemoveBuff(buffInstances[i]);
            }
        }
    }

    /// <summary>
    /// Used to adjusted the character's damage based on their stats
    /// </summary>
    /// <param name="damageRequest"></param>
    /// <param name="enemyStatsController"></param>
    /// <returns></returns>
    public DamageRequest DealDamage(DamageRequest damageRequest, StatsController enemyStatsController)
    {
        damageRequest.sourceStatsController = this;

        //calculate bonuses using attacker's statsController
        //add additional melee damage
        if (damageRequest.isMelee)
        {
            foreach (var item in statGroupings)
            {
                damageRequest.values[item.Key] += GetStatValue(item.Value.MeleeDamage);
            }
        }

        //add additional spell damage
        if (damageRequest.isSpell)
        {
            foreach (var item in statGroupings)
            {
                damageRequest.values[item.Key] += GetStatValue(item.Value.spellDamage);
            }
        }

        //determine if it is a crit
        int critRoll = new System.Random().Next(0, 100);
        float totalCritChance = (GetStatValue(StatType.CRIT_CHANCE) + damageRequest.addedCritChance);
        float totalCritChancePercentage = 1 + damageRequest.addedCritChancePercentage;

        if (critRoll < (totalCritChance * totalCritChancePercentage)) {
            damageRequest.isCrit = true;

            foreach (var item in damageRequest.values)
            {
                damageRequest.values[item.Key] = damageRequest.values[item.Key] * (((GetStatValue(StatType.CRIT_MULTIPLIER) + damageRequest.addedCritMultiplier) * (1 + damageRequest.addedCritMultiplierPercentage)) / 100f);
            }
        }

        return enemyStatsController.TakeDamage(damageRequest);
    }

    /// <summary>
    /// Used to apply the character's resistances to the attack
    /// </summary>
    /// <param name="damageRequest"></param>
    /// <returns></returns>
    public DamageRequest TakeDamage(DamageRequest damageRequest)
    {
        //reduce the damage taken based on the defender's stat controller
        //reduce based on resistances
        foreach (var item in statGroupings)
        {
            damageRequest.values[item.Key] = (1 - (GetStatValue(item.Value.resistance) / 100)) * damageRequest.values[item.Key];
        }

        //TODO: perform health leech here after resistances have been applied
        if (!damageRequest.sourceStatsController) {
            Debug.Log("Source Was:" + damageRequest.sourceStatsController);
            Debug.Log("Target Is:" + damageRequest.sourceStatsController);
        }

        float damageTotal = 0f;
        foreach (var item in statGroupings)
        {
            damageTotal += damageRequest.values[item.Key];
        }

        if (stats[StatType.CURR_HEALTH] - damageTotal <= 0)
        {
            stats[StatType.CURR_HEALTH] = 0;
            die();
        }
        else {
            stats[StatType.CURR_HEALTH] -= damageTotal;
        }

        updateHealthEvent?.Invoke(stats[StatType.CURR_HEALTH], stats[StatType.MAX_HEALTH]);

        return damageRequest;
    }

    public void UpdateEnergy(float value)
    {
        if (value + stats[StatType.CURR_ENERGY] > stats[StatType.MAX_ENERGY])
        {
            stats[StatType.CURR_ENERGY] = stats[StatType.MAX_ENERGY];
        }
        else if (stats[StatType.CURR_ENERGY] - value < 0)
        {
            stats[StatType.CURR_ENERGY] = 0;
        }
        else {
            stats[StatType.CURR_ENERGY] += value;
        }

        updateEnergyEvent?.Invoke(stats[StatType.CURR_ENERGY], stats[StatType.MAX_ENERGY]);
    }

    public void DecreaseEnergy(float value)
    {

        updateEnergyEvent?.Invoke(stats[StatType.CURR_ENERGY], stats[StatType.MAX_ENERGY]);
    }

    public float Heal(float value)
    {
        float effectiveness = 1 + GetStatValue(StatType.HEALING_EFFECTIVENESS);

        float healTotal = value * effectiveness;
        float overHealing = 0f;

        if ((stats[StatType.CURR_HEALTH] + healTotal) > stats[StatType.MAX_HEALTH])
        {
            overHealing = stats[StatType.MAX_HEALTH] - (stats[StatType.CURR_HEALTH] + healTotal);
            stats[StatType.CURR_HEALTH] = stats[StatType.MAX_HEALTH];
        }
        else {
            stats[StatType.CURR_HEALTH] += healTotal;
        }

        updateHealthEvent?.Invoke(stats[StatType.CURR_HEALTH], stats[StatType.MAX_HEALTH]);

        return overHealing;
    }
    private void die()
    {
        Debug.Log("You are dead");
    }

    public float GetStatValue(StatType statName)
    {
        float value;
        if (stats.TryGetValue(statName, out value))
        {
            return value;
        }

        return 0f;
    }

    public float GetBaseStatValue(StatType statType)
    {
        var value = baseStats.Find(x => x.statType == statType);

        if (value != null)
        {
            return value.value;
        }
        else
        {
            return 0f;
        }
    }

    public float GetAddedSpellDamage()
    {
        float addedSpellDamage = 0;

        foreach (var item in statGroupings)
        {
            addedSpellDamage += GetStatValue(item.Value.spellDamage);
        }
        return addedSpellDamage;
    }

    public float GetAddedWeaponDamage()
    {
        float addedWeaponDamage = 0f;

        foreach (var item in statGroupings)
        {
            addedWeaponDamage += GetStatValue(item.Value.MeleeDamage);
        }

        return addedWeaponDamage;
    }

    public List<BuffInstance> GetBuffInstances()
    {
        return buffInstances;
    }
}
