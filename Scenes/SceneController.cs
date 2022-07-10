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
        saveController = GetComponent<SaveController>();
    }

    /// <summary>
    /// Coroutine that loads the scene. If a nodeName is provided the player will instantiated at that node
    /// </summary>
    /// <param name="sceneZone"></param>
    /// <param name="nodeName"></param>
    /// <returns></returns>
    public IEnumerator LoadScene(SceneZone sceneZone, string nodeName = null)
    {
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
                    placeCharacterOptions.position = node.transform.position;

                    var go = PlaceCharacter(playerCharacter, placeCharacterOptions);
                    saveController.thisPlayerCharacterMB = (PlayerCharacterMB)go;
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
            instantiatedCharacterTransform.position = options.position;
        }

        if (options.rotation != null)
        {
            instantiatedCharacterTransform.rotation = Quaternion.Euler(options.rotation.x, options.rotation.y, options.rotation.z);
        }

        if (options.scale != null)
        {
            instantiatedCharacterTransform.localScale = options.scale;
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
