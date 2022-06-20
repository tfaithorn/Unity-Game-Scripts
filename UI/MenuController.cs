using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    public PlayerCharacterController playerCharacter;
    public RectTransform menuPanel;

    [Header("Menu Buttons")]
    public Button inventoryButton;
    public Button talentsButton;
    public Button abilitiesButton;
    public Button saveLoadButton;
    public Button settingsButton;
    public Button exitButton;

    [Header("Menu Panels")]
    public InventoryPanel inventoryPanel;
    public QuestPanel questPanel;
    public TalentsAndAbilitiesPanel talentsAndAbilitiesPanel;
    public SaveLoadPanel saveLoadPanel;

    private List<MenuPanel> menuPanels;
    private List<Button> menuButtons;

    public SavePanelController savePanelController;

    private MenuPanel.Type menuState;
    private bool isMenuShown = false;

    [Header("Tool Tips")]
    public ItemMouseOverTooltipController itemMouseOverPanel;

    [SerializeField]

    private void Awake()
    {
        menuButtons = new List<Button>()
        {
            inventoryButton,
            talentsButton,
            abilitiesButton,
            saveLoadButton,
            settingsButton
        };

        menuPanels = new List<MenuPanel>()
        {
            questPanel,
            inventoryPanel,
            talentsAndAbilitiesPanel,
            saveLoadPanel
        };
    }

    private void Start()
    {
        playerCharacter.keybindsController.activateGameplay.AddListener(EnableGameplay);
        playerCharacter.keybindsController.activateMenu.AddListener(EnableMenu);

        menuState = MenuPanel.Type.QUESTS;
    }

    private void EnableGameplay()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //change button scheme
        playerCharacter.keybindsController.EnableGameplay();

        HideCurrentMenuPanel();
        HideTooltipPanels();

        Time.timeScale = 1;
        menuPanel.gameObject.SetActive(false);
    }

    private void EnableMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0;

        playerCharacter.keybindsController.DisableGameplay();

        menuPanel.gameObject.SetActive(true);

        if (menuState != MenuPanel.Type.NONE)
        {
            SelectMenuPanel((int)menuState);
        }
    }

    public void ShowExitConfirmation()
    {


    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void SelectMenuPanel(int state)
    {

        HideCurrentMenuPanel();

        menuState = (MenuPanel.Type)state;

        switch (state) {

            case (int)MenuPanel.Type.QUESTS:
                EnableMenuPanel(questPanel);
                break;
            case (int)MenuPanel.Type.INVENTORY:
                EnableMenuPanel(inventoryPanel);
                break;
            case (int)MenuPanel.Type.TALENTSABILITIES:
                EnableMenuPanel(talentsAndAbilitiesPanel);
                break;
            case (int)MenuPanel.Type.SAVELOAD:
                EnableMenuPanel(saveLoadPanel);
                savePanelController.Init();
                break;
        }
    }

    private void EnableMenuPanel(MenuPanel currPanel)
    {
        foreach (MenuPanel panel in menuPanels)
        {
            if (panel != currPanel)
            {
                panel.gameObject.SetActive(false);
            }
        }

        if (!currPanel.gameObject.activeSelf)
        {
            currPanel.gameObject.SetActive(true);
        }
    }

    private void HideCurrentMenuPanel()
    {
        //deactivate previous state and hide windows
        if (menuState != MenuPanel.Type.NONE)
        {
            switch (menuState)
            {
                case MenuPanel.Type.QUESTS:

                    break;
                case MenuPanel.Type.INVENTORY:

                    break;
                case MenuPanel.Type.TALENTSABILITIES:

                    break;
                case MenuPanel.Type.SAVELOAD:

                    break;
                case MenuPanel.Type.SETTINGS:

                    break;
            }
        }
    }

    private void HideTooltipPanels()
    {
        itemMouseOverPanel.Deactivate();
    }
}
