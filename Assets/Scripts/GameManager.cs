using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevellingUp
    }

    public GameState currentState;
    public GameState previousState;

    [Header("UI")]
    [Header("Screens")]
    public GameObject pauseMenu;
    public GameObject resultsScreen;
    public GameObject levellingUpScreen;

    [Header("Results Screen")]
    public Image charImage;
    public Text charName;
    public Text levelReachedNumber;
    public Text timeSurvivedNumber;
    public List<Image> weaponSlotsUI = new List<Image>(5);
    public List<Image> passiveItemSlotsUI = new List<Image>(5);

    [Header("Stopwatch")]
    public float timeLimitSeconds;
    float currentStopwatchTime;
    public Text stopwatchDisplay;

    //flags for game states
    public bool isGameOver = false;
    public bool choosingItem = false;

    public GameObject playerObj;

    void GameStateManager()
    {
        switch(currentState)
        {
            case GameState.Gameplay:
                PauseAndResumeInput();
                UpdateStopwatch(); //stopwatch only ticks while game is actually running
                break;

            case GameState.Paused:
                PauseAndResumeInput();
                break;

            case GameState.GameOver:
                if(!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f; //stops the game
                    Debug.Log("Game over dude. ");
                    DisplayResults();
                }
                break;
            case GameState.LevellingUp:
                if(!choosingItem)
                {
                    choosingItem = true;
                    Time.timeScale = 0f; //stops the game

                    Debug.Log("Level Up Screen Show");
                    levellingUpScreen.SetActive(true);
                }
                break;

            default:
                Debug.LogWarning("State error");
                break;
        }
    }

    void TestSwitchState()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            currentState++;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            currentState--;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }   

    public void PauseGame()
    {
        if(currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f; //pause
            pauseMenu.SetActive(true);

            Debug.Log("Paused. ");
        }
        
    }

    public void ResumeGame()
    {
        if(currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f; //resume
            pauseMenu.SetActive(false);

            Debug.Log("Resuming game. ");
        }
    }

    void PauseAndResumeInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && currentState != GameState.Paused)
        {
            PauseGame();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && currentState == GameState.Paused)
        {
            ResumeGame();
        }
    }

    void DisableAllScreens()
    {
        pauseMenu.SetActive(false);
        resultsScreen.SetActive(false);
        levellingUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedNumber.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);
    }
    
    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void ShowCharacterInResults(CharacterScriptableObject character)
    {
        charImage.sprite = character.CharSprite;
        charName.text = character.CharName;
    }

    public void ShowLevelInResults(int level)
    {
        levelReachedNumber.text = level.ToString();
    }

    public void ShowWeaponsAndItemsInResults(List<Image> weaponsToShow, List<Image> itemsToShow)
    {
        if(weaponsToShow.Count != weaponSlotsUI.Count || itemsToShow.Count != passiveItemSlotsUI.Count)
        {
            Debug.LogWarning("Weapon or item lists have wrong lengths");
            return;
        }

        //loop through player inventory at death and show sprites of weapons in the results screen
        for (int i = 0; i < weaponSlotsUI.Count; i++)
        {
            if(weaponsToShow[i].sprite)
            {
                weaponSlotsUI[i].enabled = true;
                weaponSlotsUI[i].sprite = weaponsToShow[i].sprite;
            }
            else
            {
                weaponSlotsUI[i].enabled = false;
            }
        }

        //loop through player inventory at death and show sprites of items in the results screen
        for (int i = 0; i < passiveItemSlotsUI.Count; i++)
        {
            if(itemsToShow[i].sprite)
            {
                passiveItemSlotsUI[i].enabled = true;
                passiveItemSlotsUI[i].sprite = itemsToShow[i].sprite;
            }
            else
            {
                passiveItemSlotsUI[i].enabled = false;
            }
        }
    }

    void UpdateStopwatch()
    {
        currentStopwatchTime += Time.deltaTime;

        //constantly update the stopwatch display
        UpdateStopwatchDisplay();

        if(currentStopwatchTime >= timeLimitSeconds)
        {
            GameOver();
        }
    }

    void UpdateStopwatchDisplay()
    {
        //grab seconds and use modulo to get minutes in seconds to show
        int minutes = Mathf.FloorToInt(currentStopwatchTime / 60f);
        int seconds = Mathf.FloorToInt(currentStopwatchTime % 60f);

        // Update the UI text
        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevellingUp);
        playerObj.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp()
    {
        choosingItem = false;
        Time.timeScale = 1;
        levellingUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else{
            Debug.LogWarning("Extra " + this + "deleted");
        }

        DisableAllScreens();
    }

    void Update()
    {
        GameStateManager();       
    }
}
