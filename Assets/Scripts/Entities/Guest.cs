using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Nacionalidade
{
    MARROCOS, JAPAO, SIRIA, BRASIL, CHINA, CANADA, COREIA_NORTE, FRANCA, INGLATERRA, ARGELIA, AFRICA_SUL, BOLI, ARGENTINA, CUBA
}

public enum Humor
{
    GAMEOVER, BAD, NORMAL, GOOD
}

public enum DialogueLineType
{
    GUEST, PLAYER, WAIT, CHOICES, END, CHECK_PC_CARD, KEY_TIME
}

[System.Serializable]
public class DialogueLine
{
    public DialogueLineType lineType;
    public string text;
    public string[] choices;

    public bool changeExpectedKey;
    public string newExpectedKey;

    public bool checkHumorAndJump;
    public bool changeHumor;
    public Humor humorToGo;
    public int badNextLineIndex;
    public int normalNextLineIndex;
    public int goodNextLineIndex;

    public int rightKeyNextIndex;
    public int wrongKeyNextIndex;

    public int choiceOneNextLine;
    public int choiceTwoNextLine;
}

public class Guest : MonoBehaviour
{
    
    public GameObject miniDocsObj;

    public string expectedKey;
    public bool isCouple = false;
    public Nacionalidade nacionalidade;
    public Humor humor;

    public string[] choices;

    public DialogueLine[] dialogueLines;

    private Vector3 leavingPosition;

    private Table table;
    private bool guestLeft = false;

    void Start()
    {
        table = FindObjectOfType<Table>();

        SetupInitialHumor();
    }

    void Update()
    {
        if(GameManager.manager.isCurrentState(GameState.GUEST_ARRIVED))
        {
            transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, GameManager.manager.guestSpawnPosition.transform.position.x, 0.2f),
                transform.position.y,
                transform.position.z
            );

            if(Vector3.Distance(transform.position, GameManager.manager.guestSpawnPosition.transform.position) < .2f)
            {
                ShowDocs();
                GameManager.manager.SwitchState(GameState.GUEST_SHOWS_DOC);
                leavingPosition = transform.position;
                leavingPosition.x -= 15;
            }
        }

        if(GameManager.manager.isCurrentState(GameState.GUEST_LEAVE))
        {
            transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, leavingPosition.x, 0.05f),
                transform.position.y,
                transform.position.z
            );

            if(Vector3.Distance(transform.position, leavingPosition) < .2f)
            {
                guestLeft = true;
            }
        }


        ManageHumor();
    }

    void SetupInitialHumor()
    {
        if(table.IsItemOnTable(nacionalidade))
        {
            humor = Humor.GOOD;
        }
        else
        {
            humor = Humor.NORMAL;
        }
    }

    public void ShowDocs()
    {
        Instantiate(miniDocsObj, transform.position, Quaternion.identity);
    }

    public void SwitchHumor(Humor humorToTurn)
    {
        humor = humorToTurn;
    }

    void ManageHumor()
    {

        if(isCouple)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                ChangeHumorSprite(humor, transform.GetChild(i).gameObject);
            }
        }
        else
        {
            ChangeHumorSprite(humor, gameObject);
        }
    }

    void ChangeHumorSprite(Humor currentHumor, GameObject guest)
    {
        switch(currentHumor)
        {
            case Humor.BAD:
                guest.GetComponent<SpriteRenderer>().sprite = guest.GetComponent<GuestSprites>().badSprite;
            break;
            case Humor.NORMAL:
                guest.GetComponent<SpriteRenderer>().sprite = guest.GetComponent<GuestSprites>().normalSprite;
            break;
            case Humor.GOOD:
                guest.GetComponent<SpriteRenderer>().sprite = guest.GetComponent<GuestSprites>().goodSprite;
            break;
        }
    }

    public bool CheckGivenKey(string key)
    {
        return (key == expectedKey);
    }

    private void OnMouseEnter()
    {
        GameManager.manager.guestHovered = this;
    }

    private void OnMouseExit()
    {
        GameManager.manager.guestHovered = null;
    }

    public bool CheckGuestLeft()
    {
        return guestLeft;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Key")
        {
            GameManager.manager.guestHovered = this;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "Key")
        {
            GameManager.manager.guestHovered = null;
        }
    }
}
