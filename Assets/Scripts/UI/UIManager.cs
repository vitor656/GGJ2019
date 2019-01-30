using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public Button nextBtn;

    public Image winImg;

    public Image gameOverImg;
    public GameObject monitorScreen;

    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;

    public GameObject playerTextBox;
    public TextMeshProUGUI playerText;
    public float timeLapse = 0.05f;

    public GameObject responseBox;

    public TextMeshProUGUI dateText;
    public TextMeshProUGUI timeText;

    public GameObject[] onomaTecTec;

    private bool isShowingMonitor = false;

    private string currentTextDialog;
    private string currentPlayerDialogText;

    void Start()
    {
        dialogBox.SetActive(false);
        responseBox.SetActive(false);
        monitorScreen.SetActive(false);
        playerTextBox.SetActive(false);
    }

    void Update()
    {
        UpdateFields();
        ManageFields();
    }

    void ManageFields()
    {
        if(GameManager.manager.isCurrentState(GameState.PREPARE))
        {
            nextBtn.gameObject.SetActive(true);
        }
        else
        {
            nextBtn.gameObject.SetActive(false);
        }

        if(GameManager.manager.isCurrentState(GameState.GAME_OVER))
        {
            gameOverImg.gameObject.SetActive(true);
        } else {
            gameOverImg.gameObject.SetActive(false);
        }

        if(GameManager.manager.isCurrentState(GameState.WIN))
        {
            winImg.gameObject.SetActive(true);
        } else {
            winImg.gameObject.SetActive(false);
        }
    }

    void UpdateFields()
    {
        dateText.text = GameManager.manager.currentDate.ToShortDateString();
        timeText.text = GameManager.manager.currentDate.ToShortTimeString();

        //monitorScreen.SetActive(isShowingMonitor);
    }

    public void GuestDialog(string text)
    {
        if(!dialogBox.gameObject.activeInHierarchy)
        {
            dialogBox.gameObject.SetActive(true);
            dialogText.text = string.Empty;
            currentTextDialog = text;
            //FindObjectOfType<AudioManager>().Play(Sounds.KOREAN_NEUTRAL);
            StartCoroutine(ShowTextDialog());
        }
    }

    public void PlayerDialog(string text)
    {
        if(!playerTextBox.gameObject.activeInHierarchy)
        {
            playerTextBox.gameObject.SetActive(true);
            playerText.text = string.Empty;
            currentPlayerDialogText = text;
            
            StartCoroutine(ShowPlayerTextDialog());
        }
    }

    private IEnumerator ShowTextDialog(){
        for (int i = 0; i < currentTextDialog.Length; i++){
            dialogText.text = string.Concat(dialogText.text, currentTextDialog[i]);
            yield return new WaitForSeconds(timeLapse);
        }

        StartCoroutine(WaitAndDeactivate(dialogBox));
    }

    private IEnumerator ShowPlayerTextDialog(){
        for (int i = 0; i < currentPlayerDialogText.Length; i++){
            playerText.text = string.Concat(playerText.text, currentPlayerDialogText[i]);
            yield return new WaitForSeconds(timeLapse);
        }

        StartCoroutine(WaitAndDeactivate(playerTextBox));
    }

    private IEnumerator WaitAndDeactivate(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        obj.SetActive(false);

        GameManager.manager.ContinueInteraction();
    }

    public void ShowChoices(string[] choices)
    {
        responseBox.SetActive(true);

        int choiceIndex = 0;
        if(choices.Length >= 1)
        {
            foreach(Transform child in responseBox.transform)
            {
                if(choiceIndex < choices.Length)
                {
                    child.GetComponent<TextMeshProUGUI>().text = choices[choiceIndex];
                    choiceIndex++;
                }
                
            }
        }
    }

    public void ShowMonitor()
    {
        isShowingMonitor = true;
        GameManager.manager.monitorChecked = true;
    }

    public void HideMonitor()
    {
        isShowingMonitor = false;
    }

    public IEnumerator TecTec()
    {
        for(int i = 0; i < 2; i++)
        {   
            GameObject tec = onomaTecTec[Random.RandomRange(0, onomaTecTec.Length)];
            tec.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            tec.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            tec.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            tec.gameObject.SetActive(false);
        }
    }

}
