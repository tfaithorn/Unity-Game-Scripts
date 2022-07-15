using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SceneController : MonoBehaviour
{
    public SceneZone currentSceneZone;
    private SaveController saveController;

    void Awake()
    {
        currentSceneZone = SceneZoneDatabase.GetSceneZone(1);
        saveController = GetComponent<SaveController>();
    }

    /// <summary>
    /// Note: The non-async method is to allow non-persistent scripts to call it otherwise the corountine would never finish
    /// </summary>
    /// <param name="sceneZone"></param>
    /// <param name="nodeName"></param>
    public void LoadScene(SceneZone sceneZone, string nodeName = null)
    {
        StartCoroutine(LoadSceneAsync(sceneZone,nodeName));
    }

    /// <summary>
    /// Coroutine that loads the scene. When a nodeName is provided the player will be instantiated at the node
    /// </summary>
    /// <param name="sceneZone"></param>
    /// <param name="nodeName"></param>
    /// <returns></returns>
    public IEnumerator LoadSceneAsync(SceneZone sceneZone, string nodeName = null, PlayerCharacterMB playerCharacterMB = null)
    {
        if (playerCharacterMB != null)
        {
            playerCharacterMB.keybindsController.lookAction = null;
            saveController.SaveGame("Auto Save", sceneZone.GetSceneId(), playerCharacterMB.cam.GetComponent<Camera>(), true);
        }

        var sceneName = sceneZone.GetSceneName();
        var progress = SceneManager.LoadSceneAsync(sceneName);

        currentSceneZone = sceneZone;

        while (!progress.isDone)
        {
            yield return null;
        }

        if (nodeName != null)
        {
            List<SceneNode> nodes = GameObject.FindGameObjectsWithTag(Constants.sceneNodeTagName).ToList().ConvertAll(x => x.GetComponent<SceneNode>());
            
            foreach (SceneNode node in nodes)
            {
                if (node.name == nodeName)
                {
                    PlayerCharacterMB playerCharacter = GetPlayerPrefab();

                    var placeCharacterOptions = new PlaceCharacterOptions();
                    placeCharacterOptions.SetPosition(node.transform.position);

                    var go = PlaceCharacter(playerCharacter, placeCharacterOptions);
                    saveController.InitialisePlayerCharacter((PlayerCharacterMB)go);
                }
            }
        }
    }

    /// <summary>
    /// Instantiates the character and places them in the game world.
    /// A reference to the instantiated character will be returned
    /// </summary>
    /// <param name="character"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public CharacterMB PlaceCharacter(CharacterMB characterMB, PlaceCharacterOptions options)
    {
        var instantiatedCharacter = Instantiate(characterMB);
        
        Transform instantiatedCharacterTransform = instantiatedCharacter.characterModelTransform;

        if (options.position != null)
        {
            instantiatedCharacterTransform.position = options.position.vector3;
        }

        if (options.rotation != null)
        {
            instantiatedCharacterTransform.rotation = options.rotation.quaternion;
        }

        return instantiatedCharacter;
    }

    public PlayerCharacterMB GetPlayerPrefab()
    {
        return Resources.Load<PlayerCharacterMB>(Constants.playerPrefabPath);
    }

    public static SceneController FindSceneController()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag(Constants.persistentScriptsTagName);

        if (go.Length != 0)
        {
            return go[0].GetComponent<SceneController>();
        }
        else
        {
            return null;
        }
    }
}
