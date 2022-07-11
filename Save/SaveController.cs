using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
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
        public float rotationW;
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
    public Save currentSave;
    public Player player;
    public PlayerCharacterMB playerCharacterMB;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        sceneController = GetComponent<SceneController>();
        charactersLoadedInScene = new List<Character>();
    }

    public void CreateNewPlayer(string name)
    {
        var playerRepository = new PlayerRepository();
        this.player = playerRepository.AddNewPlayer(name);
    }

    public void SetPlayerIdentity(Player player)
    {
        playerCharacterMB.Initialise(player);
    }

    public void LoadPlayerCharacter(PlayerCharacterMB playerCharacterController)
    {
        LoadCharacterAbilities(playerCharacterController);
        LoadCharacterItems(playerCharacterController);
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

    private IEnumerator LoadSaveAsync(Save save)
    {
        //pause time
        Time.timeScale = 0;

        var saveRepository = new SaveRepository();
        var saveData = JsonUtility.FromJson<PlayerSaveData>(save.saveData);
        var transformSaveData = saveData.characterSaveData.transformSaveData;
        var cameraSaveData = saveData.cameraSaveData;
        Vector3 playerPosition = new Vector3(transformSaveData.x, transformSaveData.y, transformSaveData.z);

        SceneZone sceneZone = SceneZoneDatabase.GetSceneZone(save.sceneId);
        yield return StartCoroutine(sceneController.LoadSceneAsync(sceneZone));

        var playerMbGameObject = sceneController.GetPlayerPrefab();
        var placeCharacterOptions = new PlaceCharacterOptions();
        placeCharacterOptions.SetPosition(new Vector3(transformSaveData.x, transformSaveData.y, transformSaveData.z));
        placeCharacterOptions.SetRotation(new Quaternion(transformSaveData.rotationX, transformSaveData.rotationY, transformSaveData.rotationZ, transformSaveData.rotationW));
        this.playerCharacterMB = (PlayerCharacterMB)sceneController.PlaceCharacter(playerMbGameObject, placeCharacterOptions);
        
        //set camera anchor transform
        this.playerCharacterMB.camAnchor.transform.position = new Vector3(cameraSaveData.cameraAnchorTransform.x, cameraSaveData.cameraAnchorTransform.y, cameraSaveData.cameraAnchorTransform.z);
        this.playerCharacterMB.camAnchor.transform.rotation = new Quaternion(cameraSaveData.cameraAnchorTransform.rotationX, cameraSaveData.cameraAnchorTransform.rotationY, cameraSaveData.cameraAnchorTransform.rotationZ, cameraSaveData.cameraAnchorTransform.rotationW);

        //Set camera transform
        this.playerCharacterMB.cam.position = new Vector3(cameraSaveData.cameraTransform.x, cameraSaveData.cameraTransform.y, cameraSaveData.cameraTransform.z);
        this.playerCharacterMB.cam.rotation = new Quaternion(cameraSaveData.cameraTransform.rotationX, cameraSaveData.cameraTransform.rotationY, cameraSaveData.cameraTransform.rotationZ, cameraSaveData.cameraTransform.rotationW);

        //set cameraScript variables
        this.playerCharacterMB.cameraScript.InitializeCamera(cameraSaveData.cameraRotationX, cameraSaveData.cameraRotationY, cameraSaveData.cameraDistance);

        //Load initialise the player character
        this.player = save.player;
        this.playerCharacterMB.Initialise(this.player);

        //Load characters in the scene that have data
        LoadNpcCharactersForScene(SceneZoneDatabase.GetSceneZone(save.sceneId));

        //resume time
        MenuController menuController = GameObject.FindObjectOfType<MenuController>();

        if (menuController)
        {
            menuController.EnableGameplay();
        }
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
        save.saveData = JsonUtility.ToJson(GenerateCharacterSaveData(this.playerCharacterMB));
        save.sceneId = sceneId;
        saveRepository.OverrideSave(save);

        //update runtime data with new save details
        Save updatedSave = saveRepository.GetSaveById(save.id);
    }

    private void LoadNpcCharactersForScene(SceneZone sceneZone)
    {        
        var saveCharacterRepository = new SaveCharacterRepository();
        var saveCharacters = saveCharacterRepository.LoadNpcCharactersForScene(sceneZone.GetSceneId(), this.playerCharacterMB.id);

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
                    Debug.Log("Character did reload!" + saveCharacter.guid);
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
    /*
    /// <summary>
    /// Note: Move this to camera script
    /// </summary>
    /// <param name="saveId"></param>
    /// <param name="cam"></param>
    /// 
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
    }*/

    public Save SaveGame(string saveName, long sceneId, Camera cam)
    {
        var saveRepository = new SaveRepository();
        var characterSaveData = GenerateCharacterSaveData(this.playerCharacterMB);
        var cameraSaveData = GenerateCameraSaveData();

        var saveData = new PlayerSaveData(characterSaveData, cameraSaveData);

        string jsonSaveData = JsonUtility.ToJson(saveData);

        var save = saveRepository.NewSave(this.player.id, saveName, jsonSaveData, sceneId);
        currentSave = save;

        SaveNpcCharactersForScene(save.id, sceneId);
        return save;
    }

    private CameraSaveData GenerateCameraSaveData()
    {
        var cameraAnchorTransformData = GenerateTransformSaveData(this.playerCharacterMB.camAnchor.transform);
        var cameraTransformData = GenerateTransformSaveData(this.playerCharacterMB.cameraScript.cam.transform);

        var cameraRotationX = this.playerCharacterMB.cameraScript.rotationX;
        var cameraRotationY = this.playerCharacterMB.cameraScript.rotationY;
        var cameraDistance = this.playerCharacterMB.cameraScript.camZDistance;

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
        var rotation = transform.rotation;
        transformSaveData.x = transform.position.x;
        transformSaveData.y = transform.position.y;
        transformSaveData.z = transform.position.z;
        transformSaveData.rotationX = rotation.x;
        transformSaveData.rotationY = rotation.y;
        transformSaveData.rotationZ = rotation.z;
        transformSaveData.rotationW = rotation.w;
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

