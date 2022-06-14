using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPanel : MonoBehaviour
{
    public UIController uIController;
    private PlayerCharacter playerCharacter;
    private AbilityController abilityController;
    private KeybindsController keybindController;

    public Image ability1Image;
    public Image ability2Image;
    public Image ability3Image;
    public Image ability4Image;
    public Image ability5Image;
    public Image ability6Image;
    public Image ability7Image;
    public Image ability8Image;
    public Image ability9Image;
    public Image ability10Image;
    public Image abilityLCImage;
    public Image abilityRCImage;

    public Text ability1Text;
    public Text ability2Text;
    public Text ability3Text;
    public Text ability4Text;
    public Text ability5Text;
    public Text ability6Text;
    public Text ability7Text;
    public Text ability8Text;
    public Text ability9Text;
    public Text ability10Text;
    public Text abilityLCText;
    public Text abilityRCText;

    public class AbilityUIInstance{
        public Image image;
        public Text text;
        public GameObject cooldown;
        public Ability ability;

        public AbilityUIInstance(Image image, Text text, GameObject cooldown, Ability ability)
        {
            this.image = image;
            this.text = text;
            this.cooldown = cooldown;
            this.ability = ability;
        }
    }

    public Dictionary<KeybindsController.KeyType, AbilityUIInstance> abilityUIinstances;

    private void Awake()
    {
        playerCharacter = uIController.playerCharacter;
        abilityController = playerCharacter.abilityController;
        keybindController = playerCharacter.keybindsController;

        abilityUIinstances = new Dictionary<KeybindsController.KeyType, AbilityUIInstance>() {
            {KeybindsController.KeyType.ABILITY_1, new AbilityUIInstance(ability1Image,ability1Text, GenerateCooldownImage(ability1Image), null)},
            {KeybindsController.KeyType.ABILITY_2, new AbilityUIInstance(ability2Image,ability2Text, GenerateCooldownImage(ability2Image), null)},
            {KeybindsController.KeyType.ABILITY_3, new AbilityUIInstance(ability3Image,ability3Text, GenerateCooldownImage(ability3Image), null)},
            {KeybindsController.KeyType.ABILITY_4, new AbilityUIInstance(ability4Image,ability4Text, GenerateCooldownImage(ability4Image), null)},
            {KeybindsController.KeyType.ABILITY_5, new AbilityUIInstance(ability5Image,ability5Text, GenerateCooldownImage(ability5Image), null)},
            {KeybindsController.KeyType.ABILITY_6, new AbilityUIInstance(ability6Image,ability6Text, GenerateCooldownImage(ability6Image), null)},
            {KeybindsController.KeyType.ABILITY_7, new AbilityUIInstance(ability7Image,ability7Text, GenerateCooldownImage(ability7Image), null)},
            {KeybindsController.KeyType.ABILITY_8, new AbilityUIInstance(ability8Image,ability8Text, GenerateCooldownImage(ability8Image), null)},
            {KeybindsController.KeyType.ABILITY_9, new AbilityUIInstance(ability9Image,ability9Text, GenerateCooldownImage(ability9Image), null)},
            {KeybindsController.KeyType.ABILITY_10, new AbilityUIInstance(ability10Image,ability10Text, GenerateCooldownImage(ability10Image), null)},
            {KeybindsController.KeyType.LEFT_CLICK, new AbilityUIInstance(abilityLCImage,abilityLCText, GenerateCooldownImage(abilityLCImage), null)},
            {KeybindsController.KeyType.RIGHT_CLICK, new AbilityUIInstance(abilityRCImage,abilityRCText, GenerateCooldownImage(abilityRCImage), null)}
        };
    }

    private void Start()
    {
        keybindController.abilityAddedKeyEvent += AddAbilityImage;

    }

    public void UpdateImagesByAbility(Ability ability)
    {
        foreach (var instance in abilityUIinstances)
        {
            if (instance.Value.ability == ability)
            {
                AddAbilityImage(instance.Key, ability);
            }
        }
    }

    private void AddAbilityImage(KeybindsController.KeyType keyType, Ability ability)
    {
        abilityUIinstances[keyType].ability = ability;

        string iconPath = "Ability Icons/";
        Image img = GetImageByKey(keyType);
        img.gameObject.SetActive(true);

        if (ability.icon != null) 
        {
            Sprite iconSprite = Resources.Load<Sprite>(iconPath+ability.icon);
            img.sprite = iconSprite;
        }

        string keybindName = keybindController.GetActionKeyName(keyType);
        if (keybindName != null) 
        {
            abilityUIinstances[keyType].text.text = keybindName;
        }
    }

    private GameObject GenerateCooldownImage(Image image)
    {
        return Resources.Load<GameObject>("Prefabs/UI Prefabs/cooldown");
    }

    private Image GetImageByKey(KeybindsController.KeyType keyType)
    {
        switch (keyType) {
            case KeybindsController.KeyType.ABILITY_1:
                return ability1Image;
            case KeybindsController.KeyType.ABILITY_2:
                return ability2Image;
            case KeybindsController.KeyType.ABILITY_3:
                return ability3Image;
            case KeybindsController.KeyType.ABILITY_4:
                return ability4Image;
            case KeybindsController.KeyType.ABILITY_5:
                return ability5Image;
            case KeybindsController.KeyType.ABILITY_6:
                return ability6Image;
            case KeybindsController.KeyType.ABILITY_7:
                return ability7Image;
            case KeybindsController.KeyType.ABILITY_8:
                return ability8Image;
            case KeybindsController.KeyType.ABILITY_9:
                return ability9Image;
            case KeybindsController.KeyType.ABILITY_10:
                return ability10Image;
            case KeybindsController.KeyType.LEFT_CLICK:
                return abilityLCImage;
            case KeybindsController.KeyType.RIGHT_CLICK:
                return abilityRCImage;
            default:
                return null;
        }
    }
}
