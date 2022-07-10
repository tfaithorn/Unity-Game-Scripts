using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(StatsController))]
public class StatsControllerEditor : Editor
{
    bool resistancesExpanded = false;
    bool meleeDamageExpanded = false;
    bool spellDamageExpanded = false;
    bool percentageDamageExpanded = false;

    [SerializeField] float resPhysical;
    [SerializeField] float resFire;
    [SerializeField] float resCold;
    [SerializeField] float resLightning;
    [SerializeField] float resHoly;
    [SerializeField] float resUnholy;
    [SerializeField] float resPoison;
    [SerializeField] float resArcane;
    [SerializeField] float resChaos;
    [SerializeField] float resShadow;

    [SerializeField] private float currentHealth = 0;
    [SerializeField] private float maxHealth = 0;
    [SerializeField] private float currentEnergy = 0;
    [SerializeField] private float maxEnergy = 0;

    float meleePhysical;
    float meleeFire;
    float meleeCold;
    float meleeLightning;
    float meleeHoly;
    float meleeUnholy;
    float meleePoison;
    float meleeArcane;
    float meleeChaos;
    float meleeShadow;

    float spellPhysical;
    float spellFire;
    float spellCold;
    float spellLightning;
    float spellHoly;
    float spellUnholy;
    float spellPoison;
    float spellArcane;
    float spellChaos;
    float spellShadow;

    float damagePercentagePhysical;
    float damagePercentageFire;
    float damagePercentageCold;
    float damagePercentageLightning;
    float damagePercentageHoly;
    float damagePercentageUnholy;
    float damagePercentagePoison;
    float damagePercentageArcane;
    float damagePercentageChaos;
    float damagePercentageShadow;

    SerializedProperty physicalRes;

    private void OnEnable()
    {
        physicalRes = serializedObject.FindProperty("baseStats");
        Debug.Log(physicalRes);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StatsController statsController = (StatsController)base.target;
        

        //Debug.Log(statsController.GetStatValue(StatsController.StatType.CURR_ENERGY));
        GUIContent resHeaderLabel = new GUIContent("Resistances");
        GUIContent meleeHeaderLabel = new GUIContent("Melee Damage");
        GUIContent spellHeaderLabel = new GUIContent("Spell Damage");
        GUIContent damagePercentageHeaderLabel = new GUIContent("Damage Percentages");

        GUIContent physicalLabel = new GUIContent("Physical");
        GUIContent fireLabel = new GUIContent("Fire");
        GUIContent coldLabel = new GUIContent("Cold");
        GUIContent lightningLabel = new GUIContent("Lightning");
        GUIContent holyLabel = new GUIContent("Holy");
        GUIContent unholyLabel = new GUIContent("Unholy");
        GUIContent poisonLabel = new GUIContent("Poison");
        GUIContent chaosLabel = new GUIContent("Chaos");
        GUIContent arcaneLabel = new GUIContent("Arcane");
        GUIContent shadowLabel = new GUIContent("Shadow");


        currentHealth = EditorGUILayout.FloatField("Current Health", statsController.GetBaseStatValue(StatsController.StatType.CURR_HEALTH));
        maxHealth = EditorGUILayout.FloatField("Max Health", statsController.GetBaseStatValue(StatsController.StatType.MAX_HEALTH));
        currentEnergy = EditorGUILayout.FloatField("Current Energy", statsController.GetBaseStatValue(StatsController.StatType.CURR_ENERGY));
        maxEnergy = EditorGUILayout.FloatField("Max Energy", statsController.GetBaseStatValue(StatsController.StatType.MAX_ENERGY));

        resistancesExpanded = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), resistancesExpanded, resHeaderLabel, true);

        if (resistancesExpanded)
        {
            resPhysical = EditorGUILayout.FloatField(physicalLabel, statsController.GetBaseStatValue(StatsController.StatType.RESISTANCE_PHYSICAL));
            resFire = EditorGUILayout.FloatField(fireLabel, statsController.GetBaseStatValue(StatsController.StatType.RESISTANCE_FIRE));
            resCold = EditorGUILayout.FloatField(coldLabel, statsController.GetBaseStatValue(StatsController.StatType.RESISTANCE_COLD));
            resLightning = EditorGUILayout.FloatField(lightningLabel, statsController.GetBaseStatValue(StatsController.StatType.RESISTANCE_LIGHTNING));
            resHoly = EditorGUILayout.FloatField(holyLabel, statsController.GetBaseStatValue(StatsController.StatType.RESISTANCE_HOLY));
            resUnholy = EditorGUILayout.FloatField(unholyLabel, statsController.GetBaseStatValue(StatsController.StatType.RESISTANCE_UNHOLY));
            resPoison = EditorGUILayout.FloatField(poisonLabel, statsController.GetBaseStatValue(StatsController.StatType.RESISTANCE_POISON));
            resArcane = EditorGUILayout.FloatField(arcaneLabel, statsController.GetBaseStatValue(StatsController.StatType.RESISTANCE_ARCANE));
            resChaos = EditorGUILayout.FloatField(chaosLabel, statsController.GetBaseStatValue(StatsController.StatType.RESISTANCE_CHAOS));
            resShadow = EditorGUILayout.FloatField(shadowLabel, statsController.GetBaseStatValue(StatsController.StatType.RESISTANCE_SHADOW));
        }

        meleeDamageExpanded = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), meleeDamageExpanded, meleeHeaderLabel, true);

        if (meleeDamageExpanded)
        {
            meleePhysical = EditorGUILayout.FloatField(physicalLabel, statsController.GetStatValue(StatsController.StatType.MELEE_PHYSICAL_DAMAGE));
            meleeFire = EditorGUILayout.FloatField(fireLabel, statsController.GetStatValue(StatsController.StatType.MELEE_FIRE_DAMAGE));
            meleeCold = EditorGUILayout.FloatField(coldLabel, statsController.GetStatValue(StatsController.StatType.MELEE_COLD_DAMAGE));
            meleeLightning = EditorGUILayout.FloatField(lightningLabel, statsController.GetStatValue(StatsController.StatType.MELEE_LIGHTNING_DAMAGE));
            meleeHoly = EditorGUILayout.FloatField(holyLabel, statsController.GetStatValue(StatsController.StatType.MELEE_HOLY_DAMAGE));
            meleeUnholy = EditorGUILayout.FloatField(unholyLabel, statsController.GetStatValue(StatsController.StatType.MELEE_UNHOLY_DAMAGE));
            meleePoison = EditorGUILayout.FloatField(poisonLabel, statsController.GetStatValue(StatsController.StatType.MELEE_POISON_DAMAGE));
            meleeArcane = EditorGUILayout.FloatField(arcaneLabel, statsController.GetStatValue(StatsController.StatType.MELEE_ARCANE_DAMAGE));
            meleeChaos = EditorGUILayout.FloatField(chaosLabel, statsController.GetStatValue(StatsController.StatType.MELEE_CHAOS_DAMAGE));
            meleeShadow = EditorGUILayout.FloatField(shadowLabel, statsController.GetStatValue(StatsController.StatType.MELEE_SHADOW_DAMAGE));
        }

        spellDamageExpanded = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), spellDamageExpanded, spellHeaderLabel, true);

        if (spellDamageExpanded)
        {
            spellPhysical = EditorGUILayout.FloatField(physicalLabel, statsController.GetStatValue(StatsController.StatType.SPELL_PHYSICAL_DAMAGE));
            spellFire = EditorGUILayout.FloatField(fireLabel, statsController.GetStatValue(StatsController.StatType.SPELL_FIRE_DAMAGE));
            spellCold = EditorGUILayout.FloatField(coldLabel, statsController.GetStatValue(StatsController.StatType.SPELL_COLD_DAMAGE));
            spellLightning = EditorGUILayout.FloatField(lightningLabel, statsController.GetStatValue(StatsController.StatType.SPELL_LIGHTNING_DAMAGE));
            spellHoly = EditorGUILayout.FloatField(holyLabel, statsController.GetStatValue(StatsController.StatType.SPELL_HOLY_DAMAGE));
            spellUnholy = EditorGUILayout.FloatField(unholyLabel, statsController.GetStatValue(StatsController.StatType.SPELL_UNHOLY_DAMAGE));
            spellPoison = EditorGUILayout.FloatField(poisonLabel, statsController.GetStatValue(StatsController.StatType.SPELL_POISON_DAMAGE));
            spellArcane = EditorGUILayout.FloatField(arcaneLabel, statsController.GetStatValue(StatsController.StatType.SPELL_ARCANE_DAMAGE));
            spellChaos = EditorGUILayout.FloatField(chaosLabel, statsController.GetStatValue(StatsController.StatType.SPELL_CHAOS_DAMAGE));
            spellShadow = EditorGUILayout.FloatField(shadowLabel, statsController.GetStatValue(StatsController.StatType.SPELL_SHADOW_DAMAGE));
        }

        percentageDamageExpanded = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), percentageDamageExpanded, damagePercentageHeaderLabel, true);

        if (percentageDamageExpanded)
        {
            damagePercentagePhysical = EditorGUILayout.FloatField(physicalLabel, 0);
            damagePercentageFire = EditorGUILayout.FloatField(fireLabel, 0);
            damagePercentageCold = EditorGUILayout.FloatField(coldLabel, 0);
            damagePercentageLightning = EditorGUILayout.FloatField(lightningLabel, 0);
            damagePercentageHoly = EditorGUILayout.FloatField(holyLabel, 0);
            damagePercentageUnholy = EditorGUILayout.FloatField(unholyLabel, 0);
            damagePercentagePoison = EditorGUILayout.FloatField(poisonLabel, 0);
            damagePercentageArcane = EditorGUILayout.FloatField(arcaneLabel, 0);
            damagePercentageChaos = EditorGUILayout.FloatField(chaosLabel, 0);
            damagePercentageShadow = EditorGUILayout.FloatField(shadowLabel, 0);
        }

        if (GUI.changed)
        {
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.CURR_HEALTH, currentHealth), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.MAX_HEALTH, maxHealth), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.CURR_ENERGY, currentEnergy), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.MAX_ENERGY, maxEnergy), statsController);
            
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.RESISTANCE_PHYSICAL, resPhysical), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.RESISTANCE_FIRE, resFire), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.RESISTANCE_COLD, resCold), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.RESISTANCE_LIGHTNING, resLightning), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.RESISTANCE_HOLY, resHoly), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.RESISTANCE_UNHOLY, resUnholy), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.RESISTANCE_POISON, resPoison), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.RESISTANCE_ARCANE, resArcane), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.RESISTANCE_CHAOS, resChaos), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.RESISTANCE_SHADOW, resShadow), statsController);

            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.MELEE_PHYSICAL_DAMAGE, meleePhysical), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.MELEE_FIRE_DAMAGE, meleeFire), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.MELEE_COLD_DAMAGE, meleeCold), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.MELEE_LIGHTNING_DAMAGE, meleeLightning), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.MELEE_HOLY_DAMAGE, meleeHoly), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.MELEE_UNHOLY_DAMAGE, meleeUnholy), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.MELEE_POISON_DAMAGE, meleePoison), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.MELEE_ARCANE_DAMAGE, meleeArcane), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.MELEE_CHAOS_DAMAGE, meleeChaos), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.MELEE_SHADOW_DAMAGE, meleeShadow), statsController);

            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.SPELL_PHYSICAL_DAMAGE, spellPhysical), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.SPELL_FIRE_DAMAGE, spellFire), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.SPELL_COLD_DAMAGE, spellCold), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.SPELL_LIGHTNING_DAMAGE, spellLightning), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.SPELL_HOLY_DAMAGE, spellHoly), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.SPELL_UNHOLY_DAMAGE, spellUnholy), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.SPELL_POISON_DAMAGE, spellPoison), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.SPELL_ARCANE_DAMAGE, spellArcane), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.SPELL_CHAOS_DAMAGE, spellChaos), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.SPELL_SHADOW_DAMAGE, spellShadow), statsController);

            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.PERCENTAGE_PHYSICAL_INCREASE, damagePercentagePhysical), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.PERCENTAGE_FIRE_INCREASE, damagePercentageFire), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.PERCENTAGE_COLD_INCREASE, damagePercentageCold), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.PERCENTAGE_LIGHTNING_INCREASE, damagePercentageLightning), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.PERCENTAGE_HOLY_INCREASE, damagePercentageHoly), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.PERCENTAGE_UNHOLY_INCREASE, damagePercentageUnholy), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.PERCENTAGE_POISON_INCREASE, damagePercentagePoison), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.PERCENTAGE_ARCANE_INCREASE, damagePercentageArcane), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.PERCENTAGE_CHAOS_INCREASE, damagePercentageChaos), statsController);
            AddOrUpdateBaseStat(new StatsController.BaseStat(StatsController.StatType.PERCENTAGE_SHADOW_INCREASE, damagePercentageShadow), statsController);

            EditorUtility.SetDirty(base.target);
        }
    }

    private void AddOrUpdateBaseStat(StatsController.BaseStat baseStat, StatsController statsController)
    {
        var index = statsController.baseStats.FindIndex(x => x.statType == baseStat.statType);

        if (index != -1)
        {
            statsController.baseStats.RemoveAt(index);
        }

        if (baseStat.value != 0f)
        {
            statsController.baseStats.Add(baseStat);
        }
    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}
