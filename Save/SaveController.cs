using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
public class SaveController : MonoBehaviour
{
    [Serializable]
    private class PlayerSaveData
    {
        public CharacterSaveData characterSaveData;
        public CameraSaveData cameraSaveData;

        public PlayerSaveData(CharacterSaveData characterSaveData, CameraSaveData cameraSaveData)
        {
            this.characterSaveData = characterSaveData;
            this.cameraSaveData = cameraSaveData;
        }
    }

    [Serializable]
    private class CameraSaveData
    {
        public TransformSaveData cameraAnchorTransform;
        public TransformSaveData cameraTransform;
        public float cameraRotationX;
        public float cameraRotationY;
        public float cameraDistance;

        public CameraSaveData(TransformSaveData cameraAnchorTransform, TransformSaveData cameraTransform, float cameraRotationX, float cameraRotationY, float cameraDistance)
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
        public long id;
        public string uuid;
        public TransformSaveData transformSaveData;
        public BuffSaveData buffSaveData;
        public ItemSaveData itemSaveData;
        public CharacterSaveData(long id, string uuid, BuffSaveData buffSaveData, TransformSaveData transformSaveData, ItemSaveData itemSaveData)
        {
            this.id = id;
            this.uuid = uuid;
            this.buffSaveData = buffSaveData;
            this.transformSaveData = transformSaveData;
            this.itemSaveData = itemSaveData;
        }
    }

    [Serializable]
    private struct TransformSaveData
    {
        public float x;
        public float y;
        public float z;

        public float rotationX;
        public float rotationY;
        public float rotationZ;

        public float scaleX;
        public float scaleY;
        public float scaleZ;
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
        public long id;
        public int equiptSlotId;
        public int quantity;

        public ItemSave(long id, int equiptSlotId, int quantity)
        {
            this.id = id;
            this.equiptSlotId = equiptSlotId;
            this.quantity = quantity;
        }
    }

    SceneController sceneController;
    Dictionary<KeybindsController.KeyType, Ability> abilityKeys;
    Dictionary<KeybindsController.KeyType, InputAction> keybinds;
    List<Character> charactersLoadedInScene;
    public long currentSaveId;
    public PlayerCharacterMB thisPlayerCharacterMB;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        sceneController = GetComponent<SceneController>();
        charactersLoadedInScene = new List<Character>();
    }

    public void LoadPlayerCharacter(PlayerCharacterMB playerCharacterController)
    {
        LoadCharacterAbilities(playerCharacterController);
        LoadCharacterItems(playerCharacterController);
    }

    /// <summary>
    /// Note: The LoadSaveAsync coroutine must be called from a persistent/static object
    /// or it will stop running when the scene unloads
    /// </summary>
    /// <param name="save"></param>
    public void LoadSave(Save save)
    {
        StartCoroutine(LoadSaveAsync(save));
    }

    private IEnumerator LoadSaveAsync(Save save)
    {
        var saveRepository = new SaveRepository();
        var saveData = JsonUtility.FromJson<PlayerSaveData>(save.saveData);
        var transformSaveData = saveData.characterSaveData.transformSaveData;
        var cameraSaveData = saveData.cameraSaveData;
        Vector3 playerPosition = new Vector3(transformSaveData.x, transformSaveData.y, transformSaveData.z);

        SceneZone sceneZone = SceneZoneDatabase.GetSceneZone(save.sceneId);
        yield return StartCoroutine(sceneController.LoadScene(sceneZone));

        var playerMbGameObject = sceneController.GetPlayerPrefab();
        var placeCharacterOptions = new PlaceCharacterOptions();
        placeCharacterOptions.position = new Vector3(transformSaveData.x, transformSaveData.y, transformSaveData.z);
        placeCharacterOptions.rotation = new Vector3(transformSaveData.rotationX, transformSaveData.rotationY, transformSaveData.rotationZ);
        placeCharacterOptions.scale = new Vector3(transformSaveData.scaleX, transformSaveData.scaleY, transformSaveData.scaleZ);

        thisPlayerCharacterMB = (PlayerCharacterMB)sceneController.PlaceCharacter(playerMbGameObject, placeCharacterOptions);
        
        //set camera anchor transform
        thisPlayerCharacterMB.camAnchor.localPosition = new Vector3(cameraSaveData.cameraAnchorTransform.x, cameraSaveData.cameraAnchorTransform.y, cameraSaveData.cameraAnchorTransform.z);
        thisPlayerCharacterMB.camAnchor.rotation = Quaternion.Euler(cameraSaveData.cameraAnchorTransform.rotationX, cameraSaveData.cameraAnchorTransform.rotationY, cameraSaveData.cameraAnchorTransform.rotationZ);
        //thisPlayerCharacterMB.camAnchor.localScale = new Vector3(cameraSaveData.cameraAnchorTransform.scaleX, cameraSaveData.cameraAnchorTransform.scaleY, cameraSaveData.cameraAnchorTransform.scaleZ);
        
        //Set camera transform
        thisPlayerCharacterMB.cam.position = new Vector3(cameraSaveData.cameraTransform.x, cameraSaveData.cameraTransform.y, cameraSaveData.cameraTransform.z);
        thisPlayerCharacterMB.cam.rotation = Quaternion.Euler(cameraSaveData.cameraTransform.rotationX, cameraSaveData.cameraTransform.rotationY, cameraSaveData.cameraTransform.rotationZ);
        thisPlayerCharacterMB.cam.localScale = new Vector3(cameraSaveData.cameraTransform.scaleX, cameraSaveData.cameraTransform.scaleY, cameraSaveData.cameraTransform.scaleZ);

        //set cameraScript variables
        thisPlayerCharacterMB.cameraScript.InitializeCamera(cameraSaveData.cameraRotationX, cameraSaveData.cameraRotationY, cameraSaveData.cameraDistance);

    }

    private void LoadCharacterAbilities(PlayerCharacterMB playerCharacterController)
    {
        AbilityRepository abilityRepository = new AbilityRepository();
        var abilities = abilityRepository.GetByCriteria(new List<SqlClient.Expr>() { new SqlClient.Cond("id", new long[] {5,7}, SqlClient.OP_IN) });

        Ability longShot = abilities[0];
        Ability explodingShot = abilities[1];
        Debug.Log(longShot.icon);
        playerCharacterController.AddAbility(longShot, KeybindsController.KeyType.RIGHT_CLICK);
        playerCharacterController.AddAbility(explodingShot, KeybindsController.KeyType.ABILITY_2);

        abilityKeys = playerCharacterController.keybindsController.abilityKeys;
        keybinds = playerCharacterController.keybindsController.keybinds;

        playerCharacterController.abilityKeys = abilityKeys;
        playerCharacterController.keybinds = keybinds;
    }

    private void LoadCharacterItems(CharacterMB characterMB)
    {
        Inventory characterInventory = characterMB.GetComponent<Inventory>();

        if (characterInventory != null)
        {
            var criteria = new List<SqlClient.Expr>()
            {
                new SqlClient.Cond("characterId", characterMB.id, SqlClient.OP_EQUAL)
            };

            ItemCharacterRepository itemCharacterRepository = new ItemCharacterRepository();
            //characterInventory.inventory = itemCharacterRepository.GetByCriteria(criteria);

            var inventory = itemCharacterRepository.GetByCriteria(criteria);

            foreach (ItemCharacter itemCharacter in inventory)
            {
                characterInventory.AddToInventoryWithoutCheck(itemCharacter);
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
        save.saveData = JsonUtility.ToJson(GenerateCharacterSaveData(thisPlayerCharacterMB));
        save.sceneId = sceneId;
        saveRepository.OverrideSave(save);

        //update runtime data with new save details
        Save updatedSave = saveRepository.GetSaveById(save.id);
        //SaveManager.UpdateSave(updatedSave);
    }

    private void LoadDataForScene(SceneZone sceneZone)
    {        
        var saveCharacterRepository = new SaveCharacterRepository();
        var saveCharacters = saveCharacterRepository.GetNpcsForScene(sceneZone.GetSceneId());

        foreach (SaveCharacter saveCharacter in saveCharacters)
        {
            var npcCharacter = Resources.Load<NpcCharacterMB>(Constants.characterModelPath + "/" + saveCharacter.character.prefabPath);
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
            saveCharacterRepository.SaveCharacter(jsonSaveData, npcCharacter.id, saveId, sceneId);
        }
    }

    private void SaveScreenshot(long saveId, Camera cam)
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
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = saveId + ".PNG";

        string path = Application.streamingAssetsPath + "/" + Constants.saveScreenshotPath + "/" + filename;

        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
    }

    public Save SaveGame(string saveName, long sceneId, Camera cam)
    {
        var saveRepository = new SaveRepository();
        var characterSaveData = GenerateCharacterSaveData(thisPlayerCharacterMB);
        var cameraSaveData = GenerateCameraSaveData();

        var saveData = new PlayerSaveData(characterSaveData, cameraSaveData);

        string jsonSaveData = JsonUtility.ToJson(saveData);

        Debug.Log(jsonSaveData);
        var save = saveRepository.NewSave(thisPlayerCharacterMB.id, saveName, jsonSaveData, sceneId);
        //SaveManager.AddSave(save);

        SaveNpcCharactersForScene(save.id, sceneId);
        return save;
    }

    private CameraSaveData GenerateCameraSaveData()
    {
        var cameraAnchorTransformData = GenerateTransformSaveData(thisPlayerCharacterMB.camAnchor);
        var cameraTransformData = GenerateTransformSaveData(thisPlayerCharacterMB.cameraScript.cam.transform);
        Debug.Log(thisPlayerCharacterMB.cameraScript.cam);
        Debug.Log(cameraAnchorTransformData);
        Debug.Log(cameraTransformData);

        var cameraRotationX = thisPlayerCharacterMB.cameraScript.rotationX;
        var cameraRotationY = thisPlayerCharacterMB.cameraScript.rotationY;
        var cameraDistance = thisPlayerCharacterMB.cameraScript.camZDistance;

        CameraSaveData cameraSaveData = new CameraSaveData(cameraAnchorTransformData, cameraTransformData, cameraRotationX, cameraRotationY, cameraDistance);

        return cameraSaveData;
    }

    private CharacterSaveData GenerateCharacterSaveData(CharacterMB characterMB)
    {
        BuffSaveData buffSaveData = GenerateBuffSaveData(characterMB);
        TransformSaveData transformSaveData = GenerateTransformSaveData(characterMB.characterModelTransform);
        ItemSaveData itemSaveData = GenerateItemSaveData(characterMB);
        return new CharacterSaveData(characterMB.id, characterMB.guid, buffSaveData, transformSaveData, itemSaveData);
    }

    private BuffSaveData GenerateBuffSaveData(CharacterMB characterMB)
    {
        var buffInstances = characterMB.statsController.GetBuffInstances();

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

    private TransformSaveData GenerateTransformSaveData(Transform transform)
    {
        TransformSaveData transformSaveData = new TransformSaveData();
        transformSaveData.x = transform.position.x;
        transformSaveData.y = transform.position.y;
        transformSaveData.z = transform.position.z;
        transformSaveData.rotationX = transform.rotation.x;
        transformSaveData.rotationY = transform.rotation.y;
        transformSaveData.rotationZ = transform.rotation.z;
        transformSaveData.scaleX = transform.localScale.x;
        transformSaveData.scaleY = transform.localScale.y;
        transformSaveData.scaleZ = transform.localScale.z;

        return transformSaveData;
    }

    private ItemSaveData GenerateItemSaveData(CharacterMB characterMB)
    {
        if (characterMB.inventory == null)
        {
            return null;
        }

        var inventory = characterMB.inventory.GetInventory();

        ItemSaveData itemSaveData = new ItemSaveData();
        var itemSaveList = new List<ItemSave>();

        foreach (ItemCharacter itemCharacter in inventory)
        {
            itemSaveList.Add(new ItemSave(itemCharacter.item.id, itemCharacter.equiptSlotId, itemCharacter.quantity));
        }

        itemSaveData.itemSaves = itemSaveList.ToArray();

        return itemSaveData;
    }

}

