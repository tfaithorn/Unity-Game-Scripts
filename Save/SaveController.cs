using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine;
using System;
using System.Linq;

public class SaveController : MonoBehaviour
{
    [Serializable]
    private class PlayerSaveData
    {
        public CharacterSaveData characterSaveData;
        public CameraSaveData cameraSaveData;
        public AbilityKeySaveData abilityKeySaveData;
        public PlayerSaveData(CharacterSaveData characterSaveData, CameraSaveData cameraSaveData, AbilityKeySaveData abilityKeySaveData)
        {
            this.characterSaveData = characterSaveData;
            this.cameraSaveData = cameraSaveData;
            this.abilityKeySaveData = abilityKeySaveData;
        }
    }

    [Serializable]
    private class CameraSaveData
    {
        public CameraSave[] cameraSaves;

        public CameraSaveData(CameraSave[] cameraSaves)
        {
            this.cameraSaves = cameraSaves;
        }
    }

    [Serializable]
    private class CameraSave
    {
        public TransformSaveData cameraAnchorTransform;
        public TransformSaveData cameraTransform;
        public float cameraRotationX;
        public float cameraRotationY;
        public float cameraDistance;

        public CameraSave(TransformSaveData cameraAnchorTransform, TransformSaveData cameraTransform, float cameraRotationX, float cameraRotationY, float cameraDistance)
        {
            this.cameraAnchorTransform = cameraAnchorTransform;
            this.cameraTransform = cameraTransform;
            this.cameraRotationX = cameraRotationX;
            this.cameraRotationY = cameraRotationY;
            this.cameraDistance = cameraDistance;
        }
    }

    [Serializable]
    private class CharacterSaveData
    {
        public string guid;
        public TransformSaveData transformSaveData;
        public BuffSaveData buffSaveData;
        public ItemSaveData itemSaveData;
        public AbilitySaveData abilitySaveData;
        public CharacterSaveData(string guid, BuffSaveData buffSaveData, TransformSaveData transformSaveData, ItemSaveData itemSaveData, AbilitySaveData abilitySaveData)
        {
            this.guid = guid;
            this.buffSaveData = buffSaveData;
            this.transformSaveData = transformSaveData;
            this.itemSaveData = itemSaveData;
            this.abilitySaveData = abilitySaveData;
        }
    }

    [Serializable]
    private class TransformSaveData
    {
        public float x;
        public float y;
        public float z;
        public float rotationX;
        public float rotationY;
        public float rotationZ;
        public float rotationW;

        public TransformSaveData(float x, float y, float z, float rotationX, float rotationY, float rotationZ, float rotationW)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.rotationX = rotationX;
            this.rotationY = rotationY;
            this.rotationZ = rotationZ;
            this.rotationW = rotationW;
        }
    }

    [Serializable]
    private class BuffSaveData
    {
        public BuffSave[] buffSaves;
    }

    [Serializable]
    private class BuffSave
    {
        public long buffId;
        public string creatorUuid;
        public float durationPassed;
        public float endTime;
        public int maxStacks;
        public int stacks;
        public bool isPermanent;
        public bool hasTick;
        public float tickRate;
    }

    [Serializable]
    private class ItemSaveData
    {
        public ItemSave[] itemSaves;
    }

    [Serializable]
    private class ItemSave
    {
        public long itemId;
        public int equiptSlotId;
        public int quantity;

        public ItemSave(long itemId, int equiptSlotId, int quantity)
        {
            this.itemId = itemId;
            this.equiptSlotId = equiptSlotId;
            this.quantity = quantity;
        }
    }

    [Serializable]
    private class AbilitySaveData 
    { 
        public AbilitySave[] abilitySaves;

        public AbilitySaveData(AbilitySave[] abilitySaves)
        {
            this.abilitySaves = abilitySaves;
        }
    }

    [Serializable]
    private class AbilitySave 
    {
        public long abilityId;
        public bool isLoaded;
        public SaveTimer cooldownTimer;
        public SaveTimer durationTimer;

        public AbilitySave(long abilityId, bool isLoaded, SaveTimer cooldownTimer, SaveTimer durationTimer)
        {
            this.abilityId = abilityId;
            this.isLoaded = isLoaded;
            this.cooldownTimer = cooldownTimer;
            this.durationTimer = durationTimer;
        }
    }

    [Serializable]
    private class AbilityKeySaveData
    {
        public AbilityKeySave[] abilityKeySaves;

        public AbilityKeySaveData(AbilityKeySave[] abilityKeySaves)
        {
            this.abilityKeySaves = abilityKeySaves;
        }
    }

    [Serializable]
    private class AbilityKeySave
    { 
        public string keyNum;
        public long abilityId;

        public AbilityKeySave(string keyNum, long abilityId)
        {
            this.keyNum = keyNum;
            this.abilityId = abilityId;
        }
    }

    [Serializable]
    private class SaveTimer 
    {
        public float durationPassed;
        public float endTime;

        public SaveTimer(float durationPassed, float endTime)
        {
            this.durationPassed = durationPassed;
            this.endTime = endTime;
        }
    }

    SceneController sceneController;
    List<Character> charactersLoadedInScene;
    public bool isNewPlayer = false;
    public Save lastSave;
    public Player player;
    public PlayerCharacterMB playerCharacterMB;

    public void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        sceneController = GetComponent<SceneController>();
        charactersLoadedInScene = new List<Character>();
    }

    public void CreateNewPlayer(string name, List<ItemInstance> inventory, List<AbilityInstance> abilities, Dictionary<KeybindsController.KeyType, Ability> abilityKeys)
    {
        var playerRepository = new PlayerRepository();
        this.player = playerRepository.AddNewPlayer(name);
        PlayerCache.AddNewPlayer(this.player);

        //The below creates a save for the player in the starting scene and then load the scene
        ItemSaveData itemSaveData = GenerateItemSaveData(inventory);
        TransformSaveData transformSaveData = new TransformSaveData(0,0,-10,0,0,0,0);
        AbilitySaveData abilitySaveData = GenerateAbilitySaveData(abilities);
        BuffSaveData buffSaveData = GenerateBuffSaveData(new List<BuffInstance>());
        CharacterSaveData characterSaveData = new CharacterSaveData(Guid.NewGuid().ToString(), null, transformSaveData,itemSaveData, abilitySaveData);
        AbilityKeySaveData abilityKeySaveData = GenerateAbilityKeySaveData(abilityKeys);
        PlayerSaveData playerSaveData = new PlayerSaveData(characterSaveData, null, abilityKeySaveData);

        string saveData = JsonUtility.ToJson(playerSaveData);
        var saveRepository = new SaveRepository();
        var save = saveRepository.NewSave(this.player.id, "Auto Save", saveData, 2, true, 0);
        StartCoroutine(LoadSaveAsync(save));
    }

    public void SetPlayerIdentity(Player player)
    {
        playerCharacterMB.Initialise(player);
    }

    /// <summary>
    /// Note: The LoadSaveAsync coroutine must be called from a persistent/static object, or it will stop 
    /// running when the scene is unloaded
    /// </summary>
    /// <param name="save"></param>
    public void LoadSave(Save save)
    {
        StartCoroutine(LoadSaveAsync(save));
    }

    public void InitialisePlayerCharacter(PlayerCharacterMB playerCharacterMB)
    {
        playerCharacterMB.Initialise(player);
        this.playerCharacterMB = playerCharacterMB;
    }

    private IEnumerator LoadSaveAsync(Save save)
    {
        //pause time
        Time.timeScale = 0;

        var saveRepository = new SaveRepository();
        var saveData = JsonUtility.FromJson<PlayerSaveData>(save.saveData);
        var transformSaveData = saveData.characterSaveData.transformSaveData;

        CameraSaveData cameraSaveData = null;

        SceneZone sceneZone = SceneZoneDatabase.GetSceneZone(save.sceneId);
        yield return StartCoroutine(sceneController.LoadSceneAsync(sceneZone));

        var playerMbGameObject = sceneController.GetPlayerPrefab();
        var placeCharacterOptions = new PlaceCharacterOptions();
        placeCharacterOptions.SetPosition(new Vector3(transformSaveData.x, transformSaveData.y, transformSaveData.z));
        placeCharacterOptions.SetRotation(new Quaternion(transformSaveData.rotationX, transformSaveData.rotationY, transformSaveData.rotationZ, transformSaveData.rotationW));
        this.playerCharacterMB = (PlayerCharacterMB)sceneController.PlaceCharacter(playerMbGameObject, placeCharacterOptions);

        if (saveData.cameraSaveData.cameraSaves.Length > 0)
        {
            cameraSaveData = saveData.cameraSaveData;
            //set camera anchor transform
            this.playerCharacterMB.camAnchor.transform.position = new Vector3(cameraSaveData.cameraSaves[0].cameraAnchorTransform.x, cameraSaveData.cameraSaves[0].cameraAnchorTransform.y, cameraSaveData.cameraSaves[0].cameraAnchorTransform.z);
            this.playerCharacterMB.camAnchor.transform.rotation = new Quaternion(cameraSaveData.cameraSaves[0].cameraAnchorTransform.rotationX, cameraSaveData.cameraSaves[0].cameraAnchorTransform.rotationY, cameraSaveData.cameraSaves[0].cameraAnchorTransform.rotationZ, cameraSaveData.cameraSaves[0].cameraAnchorTransform.rotationW);

            //Set camera transform
            this.playerCharacterMB.cam.position = new Vector3(cameraSaveData.cameraSaves[0].cameraTransform.x, cameraSaveData.cameraSaves[0].cameraTransform.y, cameraSaveData.cameraSaves[0].cameraTransform.z);
            this.playerCharacterMB.cam.rotation = new Quaternion(cameraSaveData.cameraSaves[0].cameraTransform.rotationX, cameraSaveData.cameraSaves[0].cameraTransform.rotationY, cameraSaveData.cameraSaves[0].cameraTransform.rotationZ, cameraSaveData.cameraSaves[0].cameraTransform.rotationW);

            //set cameraScript variables
            this.playerCharacterMB.cameraScript.InitializeCamera(cameraSaveData.cameraSaves[0].cameraRotationX, cameraSaveData.cameraSaves[0].cameraRotationY, cameraSaveData.cameraSaves[0].cameraDistance);
        }

        //Load initialise the player character
        this.player = save.player;
        this.playerCharacterMB.Initialise(this.player);

        LoadCharacterItems(this.playerCharacterMB, saveData.characterSaveData.itemSaveData);
        LoadCharacterAbilities(this.playerCharacterMB, saveData.characterSaveData.abilitySaveData);
        LoadPlayerAbilityKeys(this.playerCharacterMB, saveData.abilityKeySaveData.abilityKeySaves);

        //Load characters in the scene that have data
        LoadNpcCharactersForScene(save);

        lastSave = save;

        //resume time using the ingame menu (will also toggle menu off)
        MenuController menuController = GameObject.FindObjectOfType<MenuController>();

        if (menuController)
        {
            menuController.EnableGameplay();
        }
    }

    private void LoadPlayerAbilityKeys(PlayerCharacterMB playerCharacterMB, AbilityKeySave[] abilityKeySaves)
    {
        foreach (var abilityKeySave in abilityKeySaves)
        {
            switch (abilityKeySave.keyNum)
            {
                case "1":
                    playerCharacterMB.keybindsController.AddAbilityKey(KeybindsController.KeyType.ABILITY_1, AbilityCache.GetAbility(abilityKeySave.abilityId));
                    break;
                case "2":
                    playerCharacterMB.keybindsController.AddAbilityKey(KeybindsController.KeyType.ABILITY_2, AbilityCache.GetAbility(abilityKeySave.abilityId));
                    break;
                case "3":
                    playerCharacterMB.keybindsController.AddAbilityKey(KeybindsController.KeyType.ABILITY_3, AbilityCache.GetAbility(abilityKeySave.abilityId));
                    break;
                case "4":
                    playerCharacterMB.keybindsController.AddAbilityKey(KeybindsController.KeyType.ABILITY_4, AbilityCache.GetAbility(abilityKeySave.abilityId));
                    break;
                case "5":
                    playerCharacterMB.keybindsController.AddAbilityKey(KeybindsController.KeyType.ABILITY_5, AbilityCache.GetAbility(abilityKeySave.abilityId));
                    break;
                case "6":
                    playerCharacterMB.keybindsController.AddAbilityKey(KeybindsController.KeyType.ABILITY_6, AbilityCache.GetAbility(abilityKeySave.abilityId));
                    break;
                case "7":
                    playerCharacterMB.keybindsController.AddAbilityKey(KeybindsController.KeyType.ABILITY_7, AbilityCache.GetAbility(abilityKeySave.abilityId));
                    break;
                case "8":
                    playerCharacterMB.keybindsController.AddAbilityKey(KeybindsController.KeyType.ABILITY_8, AbilityCache.GetAbility(abilityKeySave.abilityId));
                    break;
                case "9":
                    playerCharacterMB.keybindsController.AddAbilityKey(KeybindsController.KeyType.ABILITY_9, AbilityCache.GetAbility(abilityKeySave.abilityId));
                    break;
                case "10":
                    playerCharacterMB.keybindsController.AddAbilityKey(KeybindsController.KeyType.ABILITY_10, AbilityCache.GetAbility(abilityKeySave.abilityId));
                    break;
            }
        }
    }

    private void LoadCharacterAbilities(CharacterMB characterMB, AbilitySaveData abilitySaveData)
    {
        AbilityController abilityController = characterMB.abilityController;

        foreach (AbilitySave abilitySave in abilitySaveData.abilitySaves)
        {
            abilityController.AddAbility(AbilityCache.GetAbility(abilitySave.abilityId));

            if (abilitySave.isLoaded)
            {
                abilityController.LoadAbility(AbilityCache.GetAbility(abilitySave.abilityId));
            }
        }
    }

    private void LoadCharacterItems(CharacterMB characterMB, ItemSaveData itemSaveData)
    {
        Inventory inventory = characterMB.inventory;

        if (inventory != null)
        {
            foreach (ItemSave itemSave in itemSaveData.itemSaves)
            {
                var itemInstance = new ItemInstance(ItemCache.GetItem(itemSave.itemId), itemSave.equiptSlotId, itemSave.quantity);
                inventory.AddToInventoryWithoutCheck(itemInstance);
            }
        }
    }

    /// <summary>
    /// Utility to find the SaveController gameobject
    /// </summary>
    /// <returns></returns>
    public static SaveController FindSaveController()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag(Constants.persistentScriptsTagName);

        if (go.Length != 0)
        {
            return go[0].GetComponent<SaveController>();
        }
        else
        {
            return null;
        }
    }

    public void OverrideSave(Save save, long sceneId)
    {
        SaveRepository saveRepository = new SaveRepository();
        save.saveData = JsonUtility.ToJson(GenerateCharacterSaveData(this.playerCharacterMB));
        save.sceneId = sceneId;

        //TODO: instead of overriding, hide previous save and create a new one, because other saves may rely on this data
        //saveRepository.OverrideSave(save);

        //update runtime data with new save details
        Save updatedSave = saveRepository.GetSaveById(save.id);
    }

    private void LoadNpcCharactersForScene(Save save)
    {        
        var saveCharacterRepository = new SaveCharacterRepository();
        var saveCharacters = saveCharacterRepository.LoadNpcCharactersForSave(save);

        List<NpcCharacterMB> npcCharactersInScene = GameObject.FindObjectsOfType<NpcCharacterMB>().ToList();

        foreach (SaveCharacter saveCharacter in saveCharacters)
        {
            var characterInScene = npcCharactersInScene.Find(x => x.guid == saveCharacter.guid);
            
            //Destroy character with the same guid in scene
            if (characterInScene != null)
            {
                bool replaceCharacter = characterInScene.ShouldCharacterBeReplaced();

                if (replaceCharacter)
                {
                    GameObject.Destroy(characterInScene);

                    //Instantiate the character in the scene
                    var npcCharacter = Resources.Load<NpcCharacterMB>(Constants.characterModelPath + "/" + saveCharacter.character.prefabPath);
                    var saveData = JsonUtility.FromJson<CharacterSaveData>(saveCharacter.saveData);

                    PlaceCharacterOptions placeCharacterOptions = new PlaceCharacterOptions();
                    placeCharacterOptions.SetPosition(new Vector3(saveData.transformSaveData.x, saveData.transformSaveData.y, saveData.transformSaveData.z));
                    placeCharacterOptions.SetRotation(new Quaternion(saveData.transformSaveData.rotationX, saveData.transformSaveData.rotationY, saveData.transformSaveData.rotationZ, saveData.transformSaveData.rotationW));
                    sceneController.PlaceCharacter(npcCharacter, placeCharacterOptions);
                }
            }
        }
    }

    private void SaveNpcCharactersForScene(long saveId, long sceneId)
    {
        var npcCharacters = FindObjectsOfType<NpcCharacterMB>();
        var saveCharacterRepository = new SaveCharacterRepository();

        foreach (var npcCharacter in npcCharacters)
        {
            var characterSaveData = GenerateCharacterSaveData(npcCharacter);
            string jsonSaveData = JsonUtility.ToJson(characterSaveData);
            saveCharacterRepository.SaveCharacter(jsonSaveData, npcCharacter.id, saveId, sceneId, npcCharacter.guid);
        }
    }

    private PlayerSaveData GeneratePlayerSaveData(PlayerCharacterMB playerCharacterMB)
    {
        var characterSaveData = GenerateCharacterSaveData(playerCharacterMB);
        var cameraSaveData = GenerateCameraSaveData(playerCharacterMB);
        var abilityKeySaveData = GenerateAbilityKeySaveData(playerCharacterMB.keybindsController.abilityKeys);
        return new PlayerSaveData(characterSaveData, cameraSaveData, abilityKeySaveData);
    }
    public Save SaveGame(string saveName, long sceneId, Camera cam, bool isSystem)
    {
        var saveRepository = new SaveRepository();
        var saveData = GeneratePlayerSaveData(this.playerCharacterMB);
        string jsonSaveData = JsonUtility.ToJson(saveData);

        long parentId = 0;
        if (lastSave != null)
        {
            parentId = lastSave.id;
        }

        var save = saveRepository.NewSave(this.player.id, saveName, jsonSaveData, sceneId, isSystem, parentId);
        SaveCache.AddSave(save);

        if (cam != null)
        {
            string path = Application.streamingAssetsPath + "/" + Constants.saveScreenshotPath + "/" + save.name + ".PNG";
            SaveScreenshot(path, cam);
        }

         lastSave = save;
        SaveNpcCharactersForScene(save.id, sceneId);
        return save;
    }

    /*
    public void LoadMostRecentDataForScene()
    {
        var saveRepository = new SaveRepository();
    }
    */
    public void SaveScreenshot(string path, Camera cam)
    {
        int resHeight = Screen.height;
        int resWidth = Screen.width;

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, bytes);
    }

    private CameraSaveData GenerateCameraSaveData(PlayerCharacterMB playerCharacterMB)
    {
        var cameraAnchorTransformData = GenerateTransformSaveData(playerCharacterMB.camAnchor.transform);
        var cameraTransformData = GenerateTransformSaveData(playerCharacterMB.cameraScript.cam.transform);

        var cameraRotationX = playerCharacterMB.cameraScript.rotationX;
        var cameraRotationY = playerCharacterMB.cameraScript.rotationY;
        var cameraDistance = playerCharacterMB.cameraScript.camZDistance;

        CameraSaveData cameraSaveData = new CameraSaveData(new List<CameraSave>() { new CameraSave(cameraAnchorTransformData, cameraTransformData, cameraRotationX, cameraRotationY, cameraDistance) }.ToArray());

        return cameraSaveData;
    }

    private CharacterSaveData GenerateCharacterSaveData(CharacterMB characterMB)
    {
        TransformSaveData transformSaveData = GenerateTransformSaveData(characterMB.characterModelTransform);
        BuffSaveData buffSaveData = GenerateBuffSaveData(characterMB.statsController.GetBuffInstances());

        ItemSaveData itemSaveData = null;
        if (characterMB.inventory != null)
        {
            itemSaveData = GenerateItemSaveData(characterMB.inventory.GetItems());
        }

        AbilitySaveData abilitySaveData = null;
        if (characterMB.abilityController != null)
        {
            abilitySaveData = GenerateAbilitySaveData(characterMB.abilityController.abilitiesList);
        }

        return new CharacterSaveData(characterMB.guid, buffSaveData, transformSaveData, itemSaveData, abilitySaveData);
    }

    private BuffSaveData GenerateBuffSaveData(List<BuffInstance> buffInstances)
    {

        List<BuffSave> buffSaves = new List<BuffSave>();
        foreach(BuffInstance buffInstance in buffInstances)
        {
            var buffSave = new BuffSave();
            buffSave.buffId = buffInstance.buff.id;
            buffSave.creatorUuid = buffInstance.creator.guid;
            buffSave.durationPassed = buffInstance.timer.durationPassed;
            buffSave.endTime = buffInstance.timer.endTime;
            buffSave.maxStacks = buffInstance.maxStacks;
            buffSave.stacks = buffInstance.stacks;
            buffSave.isPermanent = buffInstance.isPermanent;
            buffSave.hasTick = buffInstance.hasTick;
            buffSave.tickRate = buffInstance.tickRate;
            buffSaves.Add(buffSave);
        }

        BuffSaveData buffSaveData = new BuffSaveData();
        buffSaveData.buffSaves = buffSaves.ToArray();
        return buffSaveData;
    }

    private AbilitySaveData GenerateAbilitySaveData(List<AbilityInstance> abilityInstances)
    {
        var abilitySaves = new List<AbilitySave>();

        foreach (AbilityInstance abilityInstance in abilityInstances)
        {
            SaveTimer cooldownTimer = null;
            if (abilityInstance.duration != null)
            {
                cooldownTimer = new SaveTimer(abilityInstance.duration.durationPassed, abilityInstance.cooldown.endTime);
            }

            SaveTimer durationTimer = null;
            if (durationTimer != null)
            {
                durationTimer = new SaveTimer(abilityInstance.duration.durationPassed, abilityInstance.duration.endTime);
            }
            abilitySaves.Add(new AbilitySave(abilityInstance.ability.id, abilityInstance.isLoaded, cooldownTimer, durationTimer));
        }

        return new AbilitySaveData(abilitySaves.ToArray());
    }
    private TransformSaveData GenerateTransformSaveData(Transform transform)
    {
        var rotation = transform.rotation;
        TransformSaveData transformSaveData = new TransformSaveData(transform.position.x, transform.position.y, transform.position.z, rotation.x, rotation.y, rotation.z, rotation.w);

        return transformSaveData;
    }

    private ItemSaveData GenerateItemSaveData(List<ItemInstance> items)
    {
        ItemSaveData itemSaveData = new ItemSaveData();
        var itemSaveList = new List<ItemSave>();

        foreach (ItemInstance itemInstance in items)
        {
            itemSaveList.Add(new ItemSave(itemInstance.item.id, itemInstance.equiptSlotId, itemInstance.quantity));
        }

        itemSaveData.itemSaves = itemSaveList.ToArray();

        return itemSaveData;
    }

    private AbilityKeySaveData GenerateAbilityKeySaveData(Dictionary<KeybindsController.KeyType, Ability> abilityKeys)
    {
        List<AbilityKeySave> abilitySaves = new List<AbilityKeySave>();

        foreach (var item in abilityKeys)
        {
            if (item.Value == null)
            {
                continue;
            }

            string key;
            switch (item.Key) 
            {
                case KeybindsController.KeyType.ABILITY_1:
                    key = "1";
                    break;
                case KeybindsController.KeyType.ABILITY_2:
                    key = "2";
                    break;
                case KeybindsController.KeyType.ABILITY_3:
                    key = "3";
                    break;
                case KeybindsController.KeyType.ABILITY_4:
                    key = "4";
                    break;
                case KeybindsController.KeyType.ABILITY_5:
                    key = "5";
                    break;
                case KeybindsController.KeyType.ABILITY_6:
                    key = "6";
                    break;
                case KeybindsController.KeyType.ABILITY_7:
                    key = "7";
                    break;
                case KeybindsController.KeyType.ABILITY_8:
                    key = "8";
                    break;
                case KeybindsController.KeyType.ABILITY_9:
                    key = "9";
                    break;
                case KeybindsController.KeyType.ABILITY_10:
                    key = "10";
                    break;
                default:
                    key = "0";
                    break;
            }
            
            abilitySaves.Add(new AbilityKeySave(key, item.Value.id));
        }

        return new AbilityKeySaveData(abilitySaves.ToArray());
    }
}

