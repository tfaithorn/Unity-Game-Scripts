using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Data;
using UnityEngine.Events;

public class KeybindsController : MonoBehaviour
{
    public delegate void LookEvent(InputAction.CallbackContext ctx);
    public event LookEvent lookEvent;

    public delegate void EscEvent(InputAction.CallbackContext ctx);
    public UnityEvent activateGameplay;
    public UnityEvent activateMenu;

    public InputActionAsset inputActionAsset;   

    //TO DO: look at json API for saving and reloading keybinds
    public enum KeyType
    {
        MOVEMENT = 0,
        LEFT_CLICK = 1,
        RIGHT_CLICK = 2,
        ABILITY_1 = 3,
        ABILITY_2 = 4,
        ABILITY_3 = 5,
        ABILITY_4 = 6,
        ABILITY_5 = 7,
        ABILITY_6 = 8,
        ABILITY_7 = 9,
        ABILITY_8 = 10,
        ABILITY_9 = 11,
        ABILITY_10 = 12,
        TOGGLE_MENU = 13,
        INTERACT = 14,
        ZOOM = 15,
        LOOK = 16,
        TOGGLE_RUN = 17,
        TOGGLE_GAMEPLAY = 18,
        MOUSE_POSITION = 19,
        MENU_LEFT_CLICK = 20
    }

    private UIController uIController;

    public bool keyBindTest = false;

    private InputAction waitForInputAction = new InputAction(binding: "/*/<button>");
    private string rebindInputEffectivePath;
    private string rebindInputActualPath;
    private int rebindInputBindingKeyID;

    private AbilityController abilityController;
    public MenuController menuController;
    private PlayerCharacter playerCharacter;
    private CameraScript cameraScript;

    public InputAction movementAction;
    public InputAction lookAction;
    public InputAction zoomAction;
    public InputAction leftClickAction;
    public InputAction rightClickAction;

    public Dictionary<KeyType, Ability> abilityKeys = new Dictionary<KeyType, Ability>() {
        {KeyType.LEFT_CLICK, null},
        {KeyType.RIGHT_CLICK, null},
        {KeyType.ABILITY_1, null},
        {KeyType.ABILITY_2, null},
        {KeyType.ABILITY_3, null},
        {KeyType.ABILITY_4, null},
        {KeyType.ABILITY_5, null},
        {KeyType.ABILITY_6, null},
        {KeyType.ABILITY_7, null},
        {KeyType.ABILITY_8, null},
        {KeyType.ABILITY_9, null},
        {KeyType.ABILITY_10, null}
    };

    public Dictionary<KeyType, InputAction> keybinds;

    public delegate void AbilityAdded(KeyType key, Ability ability);
    public event AbilityAdded abilityAddedKeyEvent;

    public delegate void MousePositionEvent(InputAction.CallbackContext ctx);
    public event MousePositionEvent mousePositionEvent;

    private void Awake()
    {
        //playerInput = GetComponent<PlayerInput>();
        cameraScript = GetComponent<CameraScript>();
        abilityController = GetComponent<AbilityController>();
        uIController = GetComponent<UIController>();
        playerCharacter = GetComponent<PlayerCharacter>();

        inputActionAsset.Enable();

        keybinds = new Dictionary<KeyType, InputAction>()
        {
            { KeyType.MOVEMENT, inputActionAsset.FindActionMap("Gameplay").FindAction("Movement")},
            { KeyType.LEFT_CLICK, inputActionAsset.FindActionMap("Gameplay").FindAction("Left Click")},
            { KeyType.RIGHT_CLICK, inputActionAsset.FindActionMap("Gameplay").FindAction("Right Click")},
            { KeyType.ABILITY_1, inputActionAsset.FindActionMap("Gameplay").FindAction("Ability 1")},
            { KeyType.ABILITY_2, inputActionAsset.FindActionMap("Gameplay").FindAction("Ability 2")},
            { KeyType.ABILITY_3, inputActionAsset.FindActionMap("Gameplay").FindAction("Ability 3")},
            { KeyType.ABILITY_4, inputActionAsset.FindActionMap("Gameplay").FindAction("Ability 4")},
            { KeyType.ABILITY_5, inputActionAsset.FindActionMap("Gameplay").FindAction("Ability 5")},
            { KeyType.ABILITY_6, inputActionAsset.FindActionMap("Gameplay").FindAction("Ability 6")},
            { KeyType.ABILITY_7, inputActionAsset.FindActionMap("Gameplay").FindAction("Ability 7")},
            { KeyType.ABILITY_8, inputActionAsset.FindActionMap("Gameplay").FindAction("Ability 8")},
            { KeyType.ABILITY_9, inputActionAsset.FindActionMap("Gameplay").FindAction("Ability 9")},
            { KeyType.ABILITY_10, inputActionAsset.FindActionMap("Gameplay").FindAction("Ability 10")},
            { KeyType.TOGGLE_MENU, inputActionAsset.FindActionMap("Gameplay").FindAction("Toggle Menu")},
            { KeyType.INTERACT, inputActionAsset.FindActionMap("Gameplay").FindAction("Interact")},
            { KeyType.TOGGLE_RUN, inputActionAsset.FindActionMap("Gameplay").FindAction("Run")},
            { KeyType.ZOOM, inputActionAsset.FindActionMap("Gameplay").FindAction("Zoom")},
            { KeyType.LOOK, inputActionAsset.FindActionMap("Gameplay").FindAction("Look")},
            { KeyType.TOGGLE_GAMEPLAY, inputActionAsset.FindActionMap("Menu").FindAction("Toggle Gameplay")},
            { KeyType.MOUSE_POSITION, inputActionAsset.FindActionMap("Menu").FindAction("Mouse")},
            { KeyType.MENU_LEFT_CLICK, inputActionAsset.FindActionMap("Menu").FindAction("Left Click")}
        };

        inputActionAsset.FindActionMap("Gameplay").Enable();
        inputActionAsset.FindActionMap("Menu").Disable();


        activateGameplay = new UnityEvent();
        activateMenu = new UnityEvent();

        EnableGameplay();

    }

    private void Start()
    {
        //Gameplay keybinds
        keybinds[KeyType.INTERACT].performed += context => 
        {
            Debug.Log("button pressed");
        };

        keybinds[KeyType.ZOOM].performed += context => 
        {
            //cameraScript.CameraInput(context);
        };

        //mouse scroll
        keybinds[KeyType.LOOK].performed += context => 
        {
            lookEvent?.Invoke(context);
        };

        keybinds[KeyType.TOGGLE_GAMEPLAY].performed += context =>
        {
            if (activateGameplay != null)
            {
                activateGameplay.Invoke();
            }
        };

        keybinds[KeyType.TOGGLE_MENU].performed += context => 
        {
            if (activateMenu != null)
            {
                activateMenu.Invoke();
            }
        };
    }

    private void MousePositionContext(InputAction.CallbackContext context)
    {
        mousePositionEvent?.Invoke(context);
    }

    public void AddAbilityKey(KeyType key, Ability ability)
    {
        abilityKeys[key] = ability;
        abilityAddedKeyEvent?.Invoke(key, ability);
    }

    public string GetActionKeyName(KeyType key)
    {
        string displayName = keybinds[key].bindings[0].ToDisplayString(InputBinding.DisplayStringOptions.DontIncludeInteractions);

        switch (displayName){
            case "LMB":
                return "L";
            case "RMB":
                return "R";
            default:
                return displayName;
        }
    }

    public List<KeyType> GetKeysByAbility(Ability ability)
    {
        List<KeyType> keysList = new List<KeyType>();

        foreach (var abilityKey in abilityKeys)
        {
            if (abilityKey.Value == ability)
            {
                keysList.Add(abilityKey.Key);
            }
        }

        return keysList;
    }

    public InputAction GetInputAction(KeyType keyType)
    {
        return keybinds[keyType];
    }

    public void EnableGameplay()
    {
        inputActionAsset.FindActionMap("Gameplay").Enable();
        inputActionAsset.FindActionMap("Menu").Disable();

        //set up ability events
        InputAction abilityLeftClickAction = keybinds[KeyType.LEFT_CLICK];
        InputAction abilityRightClickAction = keybinds[KeyType.RIGHT_CLICK];
        InputAction ability1Action = keybinds[KeyType.ABILITY_1];
        InputAction ability2Action = keybinds[KeyType.ABILITY_2];
        InputAction ability3Action = keybinds[KeyType.ABILITY_3];
        InputAction ability4Action = keybinds[KeyType.ABILITY_4];
        InputAction ability5Action = keybinds[KeyType.ABILITY_5];
        InputAction ability6Action = keybinds[KeyType.ABILITY_6];
        InputAction ability7Action = keybinds[KeyType.ABILITY_7];
        InputAction ability8Action = keybinds[KeyType.ABILITY_8];
        InputAction ability9Action = keybinds[KeyType.ABILITY_9];
        InputAction ability10Action = keybinds[KeyType.ABILITY_10];

        //bind events for ability keys
        abilityLeftClickAction.started += AbilityLeftClickStarted;
        abilityRightClickAction.started += AbilityRightClickStarted;
        ability1Action.started += Ability1Started;
        ability2Action.started += Ability2Started;
        ability3Action.started += Ability3Started;
        ability4Action.started += Ability4Started;
        ability5Action.started += Ability5Started;
        ability6Action.started += Ability6Started;
        ability7Action.started += Ability7Started;
        ability8Action.started += Ability8Started;
        ability9Action.started += Ability9Started;
        ability10Action.started += Ability10Started;

        //when ability is no longer held
        abilityLeftClickAction.canceled += AbilityLeftClickEnded;
        abilityRightClickAction.canceled += AbilityRightClickEnded;
        ability1Action.canceled += Ability1Ended;
        ability2Action.canceled += Ability2Ended;
        ability3Action.canceled += Ability3Ended;
        ability4Action.canceled += Ability4Ended;
        ability5Action.canceled += Ability5Ended;
        ability6Action.canceled += Ability6Ended;
        ability7Action.canceled += Ability7Ended;
        ability8Action.canceled += Ability8Ended;
        ability9Action.canceled += Ability9Ended;
        ability10Action.canceled += Ability10Ended;
    }

    public void DisableGameplay()
    {
        //set up ability events
        InputAction abilityLeftClickAction = keybinds[KeyType.LEFT_CLICK];
        InputAction abilityRightClickAction = keybinds[KeyType.RIGHT_CLICK];
        InputAction ability1Action = keybinds[KeyType.ABILITY_1];
        InputAction ability2Action = keybinds[KeyType.ABILITY_2];
        InputAction ability3Action = keybinds[KeyType.ABILITY_3];
        InputAction ability4Action = keybinds[KeyType.ABILITY_4];
        InputAction ability5Action = keybinds[KeyType.ABILITY_5];
        InputAction ability6Action = keybinds[KeyType.ABILITY_6];
        InputAction ability7Action = keybinds[KeyType.ABILITY_7];
        InputAction ability8Action = keybinds[KeyType.ABILITY_8];
        InputAction ability9Action = keybinds[KeyType.ABILITY_9];
        InputAction ability10Action = keybinds[KeyType.ABILITY_10];

        //bind events for ability keys
        abilityLeftClickAction.started += AbilityLeftClickStarted;
        abilityRightClickAction.started += AbilityRightClickStarted;
        ability1Action.started -= Ability1Started;
        ability2Action.started -= Ability2Started;
        ability3Action.started -= Ability3Started;
        ability4Action.started -= Ability4Started;
        ability5Action.started -= Ability5Started;
        ability6Action.started -= Ability6Started;
        ability7Action.started -= Ability7Started;
        ability8Action.started -= Ability8Started;
        ability9Action.started -= Ability9Started;
        ability10Action.started -= Ability10Started;

        //when ability is no longer held
        abilityLeftClickAction.canceled -= AbilityLeftClickEnded;
        abilityRightClickAction.canceled -= AbilityRightClickEnded;
        ability1Action.canceled -= Ability1Ended;
        ability2Action.canceled -= Ability2Ended;
        ability3Action.canceled -= Ability3Ended;
        ability4Action.canceled -= Ability4Ended;
        ability5Action.canceled -= Ability5Ended;
        ability6Action.canceled -= Ability6Ended;
        ability7Action.canceled -= Ability7Ended;
        ability8Action.canceled -= Ability8Ended;
        ability9Action.canceled -= Ability9Ended;
        ability10Action.canceled -= Ability10Ended;

        inputActionAsset.FindActionMap("Gameplay").Disable();
        inputActionAsset.FindActionMap("Menu").Enable();

    }

    private void AbilityLeftClickStarted(InputAction.CallbackContext ctx)
    {
        playerCharacter.StartedKeyPress(abilityKeys[KeyType.LEFT_CLICK]);
    }
    private void AbilityRightClickStarted(InputAction.CallbackContext ctx)
    {
        playerCharacter.StartedKeyPress(abilityKeys[KeyType.RIGHT_CLICK]);
    }

    private void Ability1Started(InputAction.CallbackContext ctx)
    {
        playerCharacter.StartedKeyPress(abilityKeys[KeyType.ABILITY_1]);
    }

    private void Ability2Started(InputAction.CallbackContext ctx)
    {
        playerCharacter.StartedKeyPress(abilityKeys[KeyType.ABILITY_2]);
    }

    private void Ability3Started(InputAction.CallbackContext ctx)
    {
        playerCharacter.StartedKeyPress(abilityKeys[KeyType.ABILITY_3]);
    }

    private void Ability4Started(InputAction.CallbackContext ctx)
    {
        playerCharacter.StartedKeyPress(abilityKeys[KeyType.ABILITY_4]);
    }

    private void Ability5Started(InputAction.CallbackContext ctx)
    {
        playerCharacter.StartedKeyPress(abilityKeys[KeyType.ABILITY_5]);
    }

    private void Ability6Started(InputAction.CallbackContext ctx)
    {
        playerCharacter.StartedKeyPress(abilityKeys[KeyType.ABILITY_6]);
    }

    private void Ability7Started(InputAction.CallbackContext ctx)
    {
        playerCharacter.StartedKeyPress(abilityKeys[KeyType.ABILITY_7]);
    }

    private void Ability8Started(InputAction.CallbackContext ctx)
    {
        playerCharacter.StartedKeyPress(abilityKeys[KeyType.ABILITY_8]);
    }
    private void Ability9Started(InputAction.CallbackContext ctx)
    {
        playerCharacter.StartedKeyPress(abilityKeys[KeyType.ABILITY_9]);
    }

    private void Ability10Started(InputAction.CallbackContext ctx)
    {
        playerCharacter.StartedKeyPress(abilityKeys[KeyType.ABILITY_10]);
    }

    private void AbilityLeftClickEnded(InputAction.CallbackContext ctx)
    {
        playerCharacter.EndedKeyPress(KeyType.LEFT_CLICK);
    }

    private void AbilityRightClickEnded(InputAction.CallbackContext ctx)
    {
        playerCharacter.EndedKeyPress(KeyType.RIGHT_CLICK);
    }

    private void Ability1Ended(InputAction.CallbackContext ctx)
    {
        playerCharacter.EndedKeyPress(KeyType.ABILITY_1);
    }

    private void Ability2Ended(InputAction.CallbackContext ctx)
    {
        playerCharacter.EndedKeyPress(KeyType.ABILITY_2);
    }

    private void Ability3Ended(InputAction.CallbackContext ctx)
    {
        playerCharacter.EndedKeyPress(KeyType.ABILITY_3);
    }

    private void Ability4Ended(InputAction.CallbackContext ctx)
    {
        playerCharacter.EndedKeyPress(KeyType.ABILITY_4);
    }

    private void Ability5Ended(InputAction.CallbackContext ctx)
    {
        playerCharacter.EndedKeyPress(KeyType.ABILITY_5);
    }

    private void Ability6Ended(InputAction.CallbackContext ctx)
    {
        playerCharacter.EndedKeyPress(KeyType.ABILITY_6);
    }

    private void Ability7Ended(InputAction.CallbackContext ctx)
    {
        playerCharacter.EndedKeyPress(KeyType.ABILITY_7);
    }

    private void Ability8Ended(InputAction.CallbackContext ctx)
    {
        playerCharacter.EndedKeyPress(KeyType.ABILITY_8);
    }

    private void Ability9Ended(InputAction.CallbackContext ctx)
    {
        playerCharacter.EndedKeyPress(KeyType.ABILITY_9);
    }

    private void Ability10Ended(InputAction.CallbackContext ctx)
    {
        playerCharacter.EndedKeyPress(KeyType.ABILITY_10);
    }
}