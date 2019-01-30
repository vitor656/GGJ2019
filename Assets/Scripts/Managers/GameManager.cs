using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState{
    PREPARE,
    GUEST_ARRIVED, 
    GUEST_SHOWS_DOC,
    GUEST_INTERACTION,
    GUEST_LEAVE,
    GAME_OVER,
    WIN
}

public class GameManager : MonoBehaviour
{
    
    public static GameManager manager { get; private set;}
    
    [HideInInspector]
    public System.DateTime currentDate;
    private UIManager ui;
    private Guest currentGuest;
    private bool isStateReady = false;

    [SerializeField]
    private DialogueLine currentLine;
    private int currentLineIndex;
    private bool continueInteraction = false;

    [HideInInspector]
    public bool firstCardChecked = false;
    [HideInInspector]
    public bool monitorChecked = false;
    private bool isCheckingPcAndCard = false;

    [HideInInspector]
    public string selectedChoice = string.Empty;

    public Guest guestHovered;

    [SerializeField]
    private GameState currentState;
    public Transform guestSpawnPosition;

    public GameObject currentDraggedObject;

    public List<Guest> guests;
    

    void Awake()
    {
        if(manager == null)
        {
            manager = this;
        }
        else
        {
            Destroy(manager);
        }

        DontDestroyOnLoad(manager);
    }

    void Start()
    {
        ui = FindObjectOfType<UIManager>();

        InitGame();
    }

    void Update()
    {
        ManageStates();

        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        

        if(Input.GetKeyDown(KeyCode.R))
            RestartScene();
        
        if(Input.GetKeyDown(KeyCode.F))
            Screen.fullScreen = !Screen.fullScreen;

    }

    void ManageStates()
    {
        switch(currentState)
        {
            case GameState.GUEST_ARRIVED:
                SetupGuestArrived();
            break;
            case GameState.GUEST_INTERACTION:
                SetupInteraction();
                ManageInteraction();
            break;
            case GameState.GUEST_LEAVE:
                SetupGuestLeave();
            break;
            case GameState.GAME_OVER:
                SetupGameOver();
            break;
        }
    }

    void SetupGameOver()
    {
        
    }

    void InitGame()
    {
        currentDate = new System.DateTime(1992, 01, 26, 10, 0, 0);
        SwitchState(GameState.PREPARE);
    }

    void SetupGuestLeave()
    {
        if(!isStateReady)
        {

            RemoveGuestOnList();
            Destroy(GameObject.FindGameObjectWithTag("Doc"));

            isStateReady = true;
        }

        if(currentGuest.CheckGuestLeft())
        {
            if(currentGuest.humor == Humor.GAMEOVER)
            {
                SwitchState(GameState.GAME_OVER);
            } else {
                if(guests.Count > 0)
                    SwitchState(GameState.PREPARE);
                else
                    SwitchState(GameState.WIN);
            }

            Destroy(currentGuest);
            
        }
    }

    void RemoveGuestOnList()
    {
        for(int i = 0; i < guests.Count; i++)
        {
            if(guests[i].nacionalidade == currentGuest.nacionalidade)
            {
                guests.RemoveAt(i);
                break;
            }
        }
    }

    void SetupGuestArrived()
    {
        if(!isStateReady)
        {
            
            Guest newGuest = ShuffleGuest();
            //currentGuest = newGuest;
            Vector3 spaPos = guestSpawnPosition.transform.position;
            spaPos.x -= 12;
            currentGuest = Instantiate(newGuest, spaPos, Quaternion.identity);
         
            currentLineIndex = -1;
        
            isStateReady = true;
        }
    }

    void SetupResponseToGuest()
    {
        if(!isStateReady)
        {

            ui.ShowChoices(currentGuest.choices);

            isStateReady = true;
        }
    }

    void SetupInteraction()
    {
        if(!isStateReady)
        {
            //ExecuteDialogueLines();
            continueInteraction = true;

            isStateReady = true;
        }
    }

    void ManageInteraction()
    {

        // Do once
        if(continueInteraction)
        {
            ExecuteDialogueLines();
            continueInteraction = false;
        }


        // Do on update
        if(isCheckingPcAndCard)
        {
            if(monitorChecked && firstCardChecked)
            {
                ContinueInteraction();
                isCheckingPcAndCard = false;
            }
        }

        CheckChangeExpectedKey();
        CheckGivingKeys();
        CheckChoices();
    }

    void CheckChangeExpectedKey()
    {
        if(currentLine.changeExpectedKey)
        {
            currentGuest.expectedKey = currentLine.newExpectedKey;
        }
    }

    void CheckChoices()
    {
        if(currentLine.lineType == DialogueLineType.CHOICES)
        {
            if(selectedChoice != string.Empty)
            {
                if(selectedChoice == "Choice 1")
                {
                    currentLineIndex = currentLine.choiceOneNextLine - 1;
                }
                else
                {
                    currentLineIndex = currentLine.choiceTwoNextLine - 1;
                }

                selectedChoice = string.Empty;
                ContinueInteraction();
            }
        }
    }

    void CheckGivingKeys()
    {
        if(currentLine.lineType == DialogueLineType.KEY_TIME)
        {
            if(currentDraggedObject != null && currentDraggedObject.tag == "Key")
            {
                if(guestHovered != null)
                {
                
                    if(currentGuest.CheckGivenKey(currentDraggedObject.name))
                    {
                        currentDraggedObject.gameObject.SetActive(false);

                        currentLineIndex = currentLine.rightKeyNextIndex - 1;

                    } else {
                        currentLineIndex = currentLine.wrongKeyNextIndex - 1;
                        currentDraggedObject.GetComponent<Dragable>().canDrag = false;
                        currentDraggedObject.GetComponent<BackToOrigin>().ForceBack();
                        currentDraggedObject = null;
                    }

                    ContinueInteraction();

                    
                }
            }
        }
    }

    Guest ShuffleGuest()
    {
        int index = Random.Range(0, guests.Count);
        return guests[index];
    }

    public void SwitchState(GameState nextState)
    {
        currentState = nextState;
        isStateReady = false;
    }

    public bool isCurrentState(GameState stateToCheck)
    {
        return (currentState == stateToCheck);
    }

    public void ExecuteDialogueLines()
    {
        currentLine = GetNextDialogueLine();

        switch(currentLine.lineType)
        {

            case DialogueLineType.GUEST:
                ui.GuestDialog(currentLine.text);
                FindObjectOfType<AudioManager>().PlayVoice(currentGuest.nacionalidade);
            break;
            case DialogueLineType.PLAYER:
                ui.PlayerDialog(currentLine.text);
            break;
            case DialogueLineType.WAIT:
                PauseInteraction();
            break;
            case DialogueLineType.CHOICES:
                ui.ShowChoices(currentLine.choices);
            break;
            case DialogueLineType.CHECK_PC_CARD:
                isCheckingPcAndCard = true;
                monitorChecked = false;
                firstCardChecked = false;
            break;
            case DialogueLineType.KEY_TIME:
            break;
            case DialogueLineType.END:
                SwitchState(GameState.GUEST_LEAVE);
            break;
        }

        if(currentLine.checkHumorAndJump)
        {
            CheckNextDialogueIndex(currentLine);
        }

        if(currentLine.changeHumor)
        {
            currentGuest.SwitchHumor(currentLine.humorToGo);
        }
    }

    private void CheckNextDialogueIndex(DialogueLine line)
    {
        switch(currentGuest.humor)
        {
            case Humor.BAD:
                currentLineIndex = line.badNextLineIndex - 1;
            break;
            case Humor.NORMAL:
                currentLineIndex = line.normalNextLineIndex - 1;
            break;
            case Humor.GOOD:
                currentLineIndex = line.goodNextLineIndex - 1;
            break;
        }
        

    }

    private DialogueLine GetNextDialogueLine()
    {
        DialogueLine line = null;
        if(currentGuest != null)
        {
            if(currentGuest.dialogueLines.Length > 0 && currentLineIndex < currentGuest.dialogueLines.Length)
            {
                currentLineIndex++;
                line = currentGuest.dialogueLines[currentLineIndex];
            }
        }

        return line;
    }

    public void ContinueInteraction()
    {
        if(isCurrentState(GameState.GUEST_INTERACTION))
        {
            continueInteraction = true;
        }
    }
    
    public void PauseInteraction()
    {
        if(isCurrentState(GameState.GUEST_INTERACTION))
        {
            continueInteraction = false;
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitGame();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
