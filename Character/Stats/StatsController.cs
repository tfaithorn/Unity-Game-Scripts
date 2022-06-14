using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class StatsController : MonoBehaviour
{
    public delegate void UpdateHealthEvent(float currentHealth, float MaxHealth);
    public event UpdateHealthEvent updateHealthEvent;

    public delegate void UpdateEnergyEvent(float currentEnergy, float MaxEnergy);
    public event UpdateEnergyEvent updateEnergyEvent;

    InventoryController inventoryController;
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
        FIRE_RESISTANCE,
        COLD_RESISTANCE,
        PHYSICAL_RESISTANCE,
        LIGHTNING_RESISTANCE,
        CHAOS_RESISTANCE,
        HOLY_RESISTANCE,
        UNHOLY_RESISTANCE,
        POISON_RESISTANCE,
        ARCANE_RESISTANCE,
        SHADOW_RESISTANCE,
        SPELL_RESISTANCE,
        MELEE_RESISTANCE,
        PROJECTILE_RESISTANCE,
        PHYSICAL_MELEE_DAMAGE,
        FIRE_MELEE_DAMAGE,
        COLD_MELEE_DAMAGE,
        LIGHTNING_MELEE_DAMAGE,
        CHAOS_MELEE_DAMAGE,
        HOLY_MELEE_DAMAGE,
        UNHOLY_MELEE_DAMAGE,
        POISON_MELEE_DAMAGE,
        ARCANE_MELEE_DAMAGE,
        SHADOW_MELEE_DAMAGE,
        PHYSICAL_SPELL_DAMAGE,
        FIRE_SPELL_DAMAGE,
        COLD_SPELL_DAMAGE,
        LIGHTNING_SPELL_DAMAGE,
        CHAOS_SPELL_DAMAGE,
        HOLY_SPELL_DAMAGE,
        UNHOLY_SPELL_DAMAGE,
        POISON_SPELL_DAMAGE,
        ARCANE_SPELL_DAMAGE,
        SHADOW_SPELL_DAMAGE,
        HEALING_EFFECTIVENESS,
        MELEE_DAMAGE_LEECHED,
        SPELL_DAMAGE_LEECHED,
        HEALTH_REGEN_PER_SECOND,
        MIN_WEAPON_DAMAGE,
        MAX_WEAPON_DAMAGE,
        PHYSICAL_PERCENTAGE_INCREASE,
        FIRE_PERCENTAGE_INCREASE,
        COLD_PERCENTAGE_INCREASE,
        LIGHTNING_PERCENTAGE_INCREASE,
        CHAOS_PERCENTAGE_INCREASE,
        HOLY_PERCENTAGE_INCREASE,
        UNHOLY_PERCENTAGE_INCREASE,
        POISON_PERCENTAGE_INCREASE,
        ARCANE_PERCENTAGE_INCREASE,
        SHADOW_PERCENTAGE_INCREASE
    }

    public class Stat {
        StatType statType;
        string name;
        float value;

        public Stat(StatType statType, string name, float value)
        { 
            
        }
    }
    public enum DamageType
    {
        PHYSICAL,
        FIRE,
        COLD,
        LIGHTNING,
        CHAOS,
        HOLY,
        UNHOLY,
        POISON,
        ARCANE,
        SHADOW
    }

    //build stats from gear & buffs and store in dictionary
    private Dictionary<StatType, float> stats = new Dictionary<StatType, float>()
    {
        {StatType.MAX_HEALTH, 0f},
        {StatType.MAX_ENERGY, 0f},
        {StatType.CURR_HEALTH, 0f},
        {StatType.CURR_ENERGY, 0f},
        {StatType.HEALTH_PERCENTAGE, 0f},
        {StatType.ENERGY_PERCENTAGE, 0f},
        {StatType.CRIT_CHANCE, 0f},
        {StatType.CRIT_MULTIPLIER, 0f},
        {StatType.PHYSICAL_RESISTANCE, 0f},
        {StatType.FIRE_RESISTANCE, 0f},
        {StatType.COLD_RESISTANCE, 0f},
        {StatType.LIGHTNING_RESISTANCE, 0f},
        {StatType.CHAOS_RESISTANCE, 0f},
        {StatType.HOLY_RESISTANCE, 0f},
        {StatType.UNHOLY_RESISTANCE, 0f},
        {StatType.POISON_RESISTANCE, 0f},
        {StatType.ARCANE_RESISTANCE, 0f},
        {StatType.SHADOW_RESISTANCE, 0f},
        {StatType.SPELL_RESISTANCE, 0f},
        {StatType.MELEE_RESISTANCE, 0f},
        {StatType.PROJECTILE_RESISTANCE, 0f},
        {StatType.PHYSICAL_MELEE_DAMAGE, 0f},
        {StatType.FIRE_MELEE_DAMAGE, 0f},
        {StatType.COLD_MELEE_DAMAGE, 0f},
        {StatType.LIGHTNING_MELEE_DAMAGE, 0f},
        {StatType.CHAOS_MELEE_DAMAGE, 0f},
        {StatType.HOLY_MELEE_DAMAGE, 0f},
        {StatType.UNHOLY_MELEE_DAMAGE, 0f},
        {StatType.POISON_MELEE_DAMAGE, 0f},
        {StatType.ARCANE_MELEE_DAMAGE, 0f},
        {StatType.SHADOW_MELEE_DAMAGE, 0f},
        {StatType.PHYSICAL_SPELL_DAMAGE, 0f},
        {StatType.FIRE_SPELL_DAMAGE, 0f},
        {StatType.COLD_SPELL_DAMAGE, 0f},
        {StatType.LIGHTNING_SPELL_DAMAGE, 0f},
        {StatType.CHAOS_SPELL_DAMAGE, 0f},
        {StatType.HOLY_SPELL_DAMAGE, 0f},
        {StatType.UNHOLY_SPELL_DAMAGE, 0f},
        {StatType.POISON_SPELL_DAMAGE, 0f},
        {StatType.ARCANE_SPELL_DAMAGE, 0f},
        {StatType.SHADOW_SPELL_DAMAGE, 0f},
        {StatType.HEALING_EFFECTIVENESS, 0f},
        {StatType.MELEE_DAMAGE_LEECHED, 0f},
        {StatType.SPELL_DAMAGE_LEECHED, 0f},
        {StatType.HEALTH_REGEN_PER_SECOND, 0f},
        {StatType.MIN_WEAPON_DAMAGE, 0f},
        {StatType.MAX_WEAPON_DAMAGE, 0f},
        {StatType.PHYSICAL_PERCENTAGE_INCREASE, 0f},
        {StatType.FIRE_PERCENTAGE_INCREASE, 0f},
        {StatType.COLD_PERCENTAGE_INCREASE, 0f},
        {StatType.LIGHTNING_PERCENTAGE_INCREASE, 0f},
        {StatType.CHAOS_PERCENTAGE_INCREASE, 0f},
        {StatType.HOLY_PERCENTAGE_INCREASE, 0f},
        {StatType.UNHOLY_PERCENTAGE_INCREASE, 0f},
        {StatType.POISON_PERCENTAGE_INCREASE, 0f},
        {StatType.ARCANE_PERCENTAGE_INCREASE, 0f},
        {StatType.SHADOW_PERCENTAGE_INCREASE, 0f},
        {StatType.MOVEMENT_SPEED, 0f},
        {StatType.CAST_SPEED, 0f},
        {StatType.ATTACK_SPEED, 0f},
    };

    List<StatSource> statSources = new List<StatSource>();
    List<OnHitEffect> onHitEffects = new List<OnHitEffect>();

    public void CalculateStats()
    {
        foreach (var item in stats)
        {
            stats[item.Key] = 0f;

            foreach (StatSource source in statSources)
            {
                if (source.statType == item.Key)
                {
                    stats[item.Key] += source.value;
                }
            }
        }
    }

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

    public void CreateStatSourceFromBuffs()
    {
        statSources.RemoveAll(x => x.sourceType == StatSource.SourceType.BUFF);
        foreach (Buff buff in buffs)
        {
            foreach (var item in buff.statIncreases)
            {
                statSources.Add(new StatSource(item.Key, item.Value, StatSource.SourceType.BUFF, buff.id));
                // do something with entry.Value or entry.Key
            }
        }
    }

    private List<Buff> buffs = new List<Buff>();

    private void Awake()
    {
         inventoryController = GetComponent<InventoryController>();

        stats[StatType.CURR_HEALTH] = 800;
        stats[StatType.MAX_HEALTH] = 1000;

        stats[StatType.CURR_ENERGY] = 900;
        stats[StatType.MAX_ENERGY] = 1000;
    }

    private void Start()
    {

        updateHealthEvent?.Invoke(stats[StatType.CURR_HEALTH], stats[StatType.MAX_HEALTH]);
        updateEnergyEvent?.Invoke(stats[StatType.CURR_ENERGY], stats[StatType.MAX_ENERGY]);

        Buff buff = new Buff();
        buff.className = "Regeneration";
        buff.hasComponent = true;
        buff.isPermanent = false;
        buff.name = "buff1";
        buff.durationPassed = 0;
        buff.duration = 10;
        buff.source = this.gameObject;
        ApplyBuff(buff);

        Buff buff2 = new Buff();
        buff2.hasComponent = true;
        buff2.className = "Regeneration";
        buff2.name = "buff2";
        buff2.isPermanent = false;
        buff2.durationPassed = 0;
        buff2.duration = 20;
        buff2.source = this.gameObject;
        //buff2.statusType = Buff.StatusType.FEAR;
        ApplyBuff(buff2);
    }

    public void ApplyBuff(Buff buff)
    {
        if (buff.hasComponent)
        {
            buff.buffInterface = (BuffInterface)gameObject.AddComponent(Type.GetType(buff.className));
            buff.buffInterface.BuffStart(buff);
        }
        buffs.Add(buff);
        
    }

    public List<Buff.StatusType> GetStatuses() 
    {
        List<Buff.StatusType> statusTypes = new List<Buff.StatusType>();

        foreach (Buff.StatusType foo in Enum.GetValues(typeof(Buff.StatusType))) {
            if (buffs.Exists(x => x.statusType == foo)) {
                statusTypes.Add(foo);
            }
        }

        return statusTypes;
    }

    public void RemoveBuff(Buff buff)
    {
        buffs.Remove(buff);
        Destroy((Component)buff.buffInterface);
    }

    public Buff GetBuffByName(string name)
    {
        return buffs.Find(x => x.name == name);
    }

    public List<Buff> GetBuffsBySource(GameObject gb)
    {
        return (from buff in buffs 
               where buff.source == gb 
               select buff).ToList();
    }

    private void Update()
    {
        //buffs
        for(int i= 0; i<buffs.Count;i++)
        {
            if (!buffs[i].isPermanent)
            {
                buffs[i].durationPassed += Time.deltaTime;
                
                if ((buffs[i].durationPassed / buffs[i].tickRate) > buffs[i].currentTick)
                {
                    buffs[i].currentTick++;
                    if (buffs[i].hasComponent)
                    {
                        buffs[i].buffInterface.BuffTick();
                    }
                }

                if (buffs[i].durationPassed >= buffs[i].duration)
                {
                    if (buffs[i].hasComponent)
                    {
                        buffs[i].buffInterface.BuffFinish();
                    }
                    RemoveBuff(buffs[i]);
                }
            }
        }
    }
    /// <summary>
    /// Used to adjusted damage using the character's stats
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
            damageRequest.values[DamageType.PHYSICAL] += stats[StatType.PHYSICAL_MELEE_DAMAGE];
            damageRequest.values[DamageType.FIRE] += stats[StatType.FIRE_MELEE_DAMAGE];
            damageRequest.values[DamageType.COLD] += stats[StatType.COLD_MELEE_DAMAGE];
            damageRequest.values[DamageType.LIGHTNING] += stats[StatType.LIGHTNING_MELEE_DAMAGE];
            damageRequest.values[DamageType.CHAOS] += stats[StatType.CHAOS_MELEE_DAMAGE];
            damageRequest.values[DamageType.HOLY] += stats[StatType.HOLY_MELEE_DAMAGE];
            damageRequest.values[DamageType.UNHOLY] += stats[StatType.UNHOLY_MELEE_DAMAGE];
            damageRequest.values[DamageType.POISON] += stats[StatType.POISON_MELEE_DAMAGE];
            damageRequest.values[DamageType.ARCANE] += stats[StatType.ARCANE_MELEE_DAMAGE];
            damageRequest.values[DamageType.SHADOW] += stats[StatType.SHADOW_MELEE_DAMAGE];
        }

        //add additional spell damage
        if (damageRequest.isSpell)
        {
            damageRequest.values[DamageType.PHYSICAL] += stats[StatType.PHYSICAL_SPELL_DAMAGE];
            damageRequest.values[DamageType.FIRE] += stats[StatType.FIRE_SPELL_DAMAGE];
            damageRequest.values[DamageType.COLD] += stats[StatType.COLD_SPELL_DAMAGE];
            damageRequest.values[DamageType.LIGHTNING] += stats[StatType.LIGHTNING_SPELL_DAMAGE];
            damageRequest.values[DamageType.CHAOS] += stats[StatType.CHAOS_SPELL_DAMAGE];
            damageRequest.values[DamageType.HOLY] += stats[StatType.HOLY_SPELL_DAMAGE];
            damageRequest.values[DamageType.UNHOLY] += stats[StatType.UNHOLY_SPELL_DAMAGE];
            damageRequest.values[DamageType.POISON] += stats[StatType.POISON_SPELL_DAMAGE];
            damageRequest.values[DamageType.ARCANE] += stats[StatType.ARCANE_SPELL_DAMAGE];
            damageRequest.values[DamageType.SHADOW] += stats[StatType.SHADOW_SPELL_DAMAGE];
        }

        //determine if it is a crit
        int critRoll = new System.Random().Next(0, 100);
        float totalCritChance = (stats[StatType.CRIT_CHANCE] + damageRequest.addedCritChance);
        float totalCritChancePercentage = 1 + damageRequest.addedCritChancePercentage;

        if (critRoll < (totalCritChance * totalCritChancePercentage)) {
            damageRequest.isCrit = true;

            foreach (var item in damageRequest.values)
            {
                damageRequest.values[item.Key] = damageRequest.values[item.Key] * (((stats[StatType.CRIT_MULTIPLIER] + damageRequest.addedCritMultiplier) * (1 + damageRequest.addedCritMultiplierPercentage)) / 100f);
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
        //reduce damage based on defender's stat controller
        float damageTotal = 0f;
        //reduce based on resistances
        damageRequest.values[DamageType.ARCANE] = (1 - (stats[StatType.ARCANE_RESISTANCE] / 100)) * damageRequest.values[DamageType.ARCANE];
        damageRequest.values[DamageType.PHYSICAL] = (1 - (stats[StatType.PHYSICAL_RESISTANCE] / 100)) * damageRequest.values[DamageType.PHYSICAL];
        damageRequest.values[DamageType.FIRE] = (1 - (stats[StatType.FIRE_RESISTANCE] / 100)) * damageRequest.values[DamageType.FIRE];
        damageRequest.values[DamageType.COLD] = (1 - (stats[StatType.COLD_RESISTANCE] / 100)) * damageRequest.values[DamageType.COLD];
        damageRequest.values[DamageType.LIGHTNING] = (1 - (stats[StatType.LIGHTNING_RESISTANCE] / 100)) * damageRequest.values[DamageType.LIGHTNING];
        damageRequest.values[DamageType.CHAOS] = (1 - (stats[StatType.CHAOS_RESISTANCE] / 100)) * damageRequest.values[DamageType.CHAOS];
        damageRequest.values[DamageType.HOLY] = (1 - (stats[StatType.HOLY_RESISTANCE] / 100)) * damageRequest.values[DamageType.HOLY];
        damageRequest.values[DamageType.UNHOLY] = (1 - (stats[StatType.UNHOLY_RESISTANCE] / 100)) * damageRequest.values[DamageType.UNHOLY];
        damageRequest.values[DamageType.POISON] = (1 - (stats[StatType.POISON_RESISTANCE] / 100)) * damageRequest.values[DamageType.POISON];
        damageRequest.values[DamageType.SHADOW] = (1 - (stats[StatType.SHADOW_RESISTANCE] / 100)) * damageRequest.values[DamageType.SHADOW];

        //TODO: perform health leech here after resistances have been applied
        if (!damageRequest.sourceStatsController) {
            Debug.Log("Source Was:" + damageRequest.sourceStatsController);
            Debug.Log("Target Is:" + damageRequest.sourceStatsController);
        }

        damageTotal = damageRequest.values[DamageType.ARCANE]
            + damageRequest.values[DamageType.PHYSICAL]
            + damageRequest.values[DamageType.FIRE]
            + damageRequest.values[DamageType.COLD]
            + damageRequest.values[DamageType.LIGHTNING]
            + damageRequest.values[DamageType.CHAOS]
            + damageRequest.values[DamageType.HOLY]
            + damageRequest.values[DamageType.UNHOLY]
            + damageRequest.values[DamageType.POISON]
            + damageRequest.values[DamageType.SHADOW];

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

    public void UpdateENERGY(float value)
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
        float effectiveness = 1 + stats[StatType.HEALING_EFFECTIVENESS];

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
        return stats[statName];
    }

    public float GetAddedSpellDamage()
    {
        float addedSpellDamage = 0;
        addedSpellDamage += stats[StatType.PHYSICAL_SPELL_DAMAGE];
        addedSpellDamage += stats[StatType.FIRE_SPELL_DAMAGE];
        addedSpellDamage += stats[StatType.COLD_SPELL_DAMAGE];
        addedSpellDamage += stats[StatType.LIGHTNING_SPELL_DAMAGE];
        addedSpellDamage += stats[StatType.CHAOS_SPELL_DAMAGE];
        addedSpellDamage += stats[StatType.HOLY_SPELL_DAMAGE];
        addedSpellDamage += stats[StatType.UNHOLY_SPELL_DAMAGE];
        addedSpellDamage += stats[StatType.POISON_SPELL_DAMAGE];
        addedSpellDamage += stats[StatType.ARCANE_SPELL_DAMAGE];
        addedSpellDamage += stats[StatType.SHADOW_SPELL_DAMAGE];

        return addedSpellDamage;
    }

    public float GetAddedWeaponDamage()
    {
        float addedWeaponDamage = 0f;

        addedWeaponDamage += stats[StatType.PHYSICAL_MELEE_DAMAGE];
        addedWeaponDamage += stats[StatType.FIRE_MELEE_DAMAGE];
        addedWeaponDamage += stats[StatType.COLD_MELEE_DAMAGE];
        addedWeaponDamage += stats[StatType.LIGHTNING_MELEE_DAMAGE];
        addedWeaponDamage += stats[StatType.CHAOS_MELEE_DAMAGE];
        addedWeaponDamage += stats[StatType.HOLY_MELEE_DAMAGE];
        addedWeaponDamage += stats[StatType.UNHOLY_MELEE_DAMAGE];
        addedWeaponDamage += stats[StatType.POISON_MELEE_DAMAGE];
        addedWeaponDamage += stats[StatType.ARCANE_MELEE_DAMAGE];
        addedWeaponDamage += stats[StatType.SHADOW_MELEE_DAMAGE];

        return addedWeaponDamage;
    }
}
