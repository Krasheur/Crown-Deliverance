using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueCam;
    [SerializeField] Cinemachine.CinemachineVirtualCamera cinemachineVirtual;
    [SerializeField] Cinemachine.CinemachineDollyCart dollyCart;
    [SerializeField] GameObject healEffect;

    private static DialogueManager main = null;

    public Text nameText;
    public Text dialogueText;
    public float dialogueSpeed = 0.01f;
    public string currentSentence;

    public Transform talker;

    private bool isWriting;

    public Animator animator;

    private Queue<string> sentences;

    public List<string> voices;
    public int voicesCount = 0;

    public static DialogueManager Main { get => main; }    

    void Awake()
    {
        if (main != null)
        {
            Destroy(gameObject);
            return;
        }
        main = this;        

        sentences = new Queue<string>();
        voices = new List<string>();        
    }

    private void Start()
    {
        GameObject dialBox = GameObject.Find("Dialogue Box");
        animator = dialBox.GetComponent<Animator>();
        nameText = dialBox.GetComponentsInChildren<Text>()[0];
        dialogueText = dialBox.GetComponentsInChildren<Text>()[1];
        dialBox.GetComponentInChildren<Button>().onClick.AddListener(DisplayNextSentence);
    }
    public void StartDialogue(Dialogue dialogue)
    {
        dialogueCam.transform.position = talker.position + new Vector3((PlayerManager.main.FocusedCharacter.transform.position.x - talker.position.x) / 2f,
                                                                        ((PlayerManager.main.FocusedCharacter.transform.position.y - talker.position.y) / 2f) + 2f,
                                                                        (PlayerManager.main.FocusedCharacter.transform.position.z - talker.position.z) / 2f);
        dialogueCam.transform.rotation = Quaternion.Euler(PlayerManager.main.FocusedCharacter.transform.rotation.eulerAngles + new Vector3(0f, -90f, 0f));
        cinemachineVirtual.enabled = true;

        animator.SetBool("isOpen", true);
        nameText.text = dialogue.name;

        talker.GetComponent<Character>().State = CHARACTER_STATE.LOCKED;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        AkSoundEngine.PostEvent("MenuSelect_Play", gameObject); // Play Click Sound
        if (sentences.Count == 0)
        {
            if (!isWriting)
            {
                EndDialogue();
                return;
            }
        }
        if(voices.Count != 0 && voicesCount < voices.Count) AkSoundEngine.PostEvent(voices[voicesCount], gameObject);

        if (isWriting)
        {            
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            isWriting = false;
        }
        else
        {
            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        voicesCount++;
    }

    IEnumerator TypeSentence(string sentence)
    {
        isWriting = true;
        dialogueText.text = "";
        currentSentence = sentence;
        foreach(char letter in sentence.ToCharArray())
        {
            yield return new WaitForSeconds(dialogueSpeed);
            dialogueText.text += letter;
            yield return null;
        }
        isWriting = false;
    }
    
    public void EndDialogue()
    {
        dollyCart.m_Position = 1f;
        cinemachineVirtual.enabled = false;

        if (TradingManager.main.CurrentTrader != null)
        {
            talker.GetComponent<Character>().MarchandInventoryMake();
            TradingManager.main.OpenTrade();
        }
        else
        {
            PlayerManager.main.FocusedCharacter.State = CHARACTER_STATE.FREE;            
        }

        if (talker != null)
        {
            if (talker.gameObject.GetComponent<Character>().IsHealer)
            {
                PlayerManager.main.PlayerFairy.PV = PlayerManager.main.PlayerFairy.PvMax;
                PlayerManager.main.PlayerFairy.Armor = PlayerManager.main.PlayerFairy.ArmorMax;
                PlayerManager.main.PlayerFairy.State = CHARACTER_STATE.FREE;
                PlayerManager.main.PlayerFairy.GetComponent<Character>().enabled = true;
                if (PlayerManager.main.PlayerFairy.GetComponent<NavMeshAgent>().hasPath)
                {
                    PlayerManager.main.PlayerFairy.GetComponent<NavMeshAgent>().ResetPath();
                }
                PlayerManager.main.PlayerFairy.GetComponent<NavMeshAgent>().enabled = false;                
                Instantiate(healEffect, PlayerManager.main.PlayerFairy.transform);

                PlayerManager.main.PlayerRogue.PV = PlayerManager.main.PlayerRogue.PvMax;
                PlayerManager.main.PlayerRogue.Armor = PlayerManager.main.PlayerRogue.ArmorMax;
                PlayerManager.main.PlayerRogue.State = CHARACTER_STATE.FREE;
                PlayerManager.main.PlayerRogue.GetComponent<Character>().enabled = true;
                if (PlayerManager.main.PlayerRogue.GetComponent<NavMeshAgent>().hasPath)
                {
                    PlayerManager.main.PlayerRogue.GetComponent<NavMeshAgent>().ResetPath();
                }
                PlayerManager.main.PlayerRogue.GetComponent<NavMeshAgent>().enabled = false;                
                Instantiate(healEffect, PlayerManager.main.PlayerRogue.transform);

                PlayerManager.main.PlayerTank.PV = PlayerManager.main.PlayerTank.PvMax;
                PlayerManager.main.PlayerTank.Armor = PlayerManager.main.PlayerTank.ArmorMax;
                PlayerManager.main.PlayerTank.State = CHARACTER_STATE.FREE;
                PlayerManager.main.PlayerTank.GetComponent<Character>().enabled = true;
                if (PlayerManager.main.PlayerTank.GetComponent<NavMeshAgent>().hasPath)
                {
                    PlayerManager.main.PlayerTank.GetComponent<NavMeshAgent>().ResetPath();
                }
                PlayerManager.main.PlayerTank.GetComponent<NavMeshAgent>().enabled = false;                
                Instantiate(healEffect, PlayerManager.main.PlayerTank.transform);


                if (PlayerManager.main.FocusedCharacter == PlayerManager.main.PlayerFairy)
                {
                    PlayerManager.main.PlayerRogue.transform.position = PlayerManager.main.FocusedCharacter.transform.position + 2.0f * (-PlayerManager.main.FocusedCharacter.transform.forward + PlayerManager.main.FocusedCharacter.transform.right);
                    PlayerManager.main.PlayerRogue.transform.rotation = PlayerManager.main.FocusedCharacter.transform.rotation;
                    PlayerManager.main.PlayerTank.transform.position = PlayerManager.main.FocusedCharacter.transform.position + 2.0f * (-PlayerManager.main.FocusedCharacter.transform.forward + (-PlayerManager.main.FocusedCharacter.transform.right));
                    PlayerManager.main.PlayerTank.transform.rotation = PlayerManager.main.FocusedCharacter.transform.rotation;
                }
                else if (PlayerManager.main.FocusedCharacter == PlayerManager.main.PlayerRogue)
                {
                    PlayerManager.main.PlayerFairy.transform.position = PlayerManager.main.FocusedCharacter.transform.position + 2.0f * (-PlayerManager.main.FocusedCharacter.transform.forward + PlayerManager.main.FocusedCharacter.transform.right);
                    PlayerManager.main.PlayerFairy.transform.rotation = PlayerManager.main.FocusedCharacter.transform.rotation;
                    PlayerManager.main.PlayerTank.transform.position = PlayerManager.main.FocusedCharacter.transform.position + 2.0f * (-PlayerManager.main.FocusedCharacter.transform.forward + (-PlayerManager.main.FocusedCharacter.transform.right));
                    PlayerManager.main.PlayerTank.transform.rotation = PlayerManager.main.FocusedCharacter.transform.rotation;
                }
                else if (PlayerManager.main.FocusedCharacter == PlayerManager.main.PlayerTank)
                {
                    PlayerManager.main.PlayerFairy.transform.position = PlayerManager.main.FocusedCharacter.transform.position + 2.0f * (-PlayerManager.main.FocusedCharacter.transform.forward + PlayerManager.main.FocusedCharacter.transform.right);
                    PlayerManager.main.PlayerFairy.transform.rotation = PlayerManager.main.FocusedCharacter.transform.rotation;
                    PlayerManager.main.PlayerRogue.transform.position = PlayerManager.main.FocusedCharacter.transform.position + 2.0f * (-PlayerManager.main.FocusedCharacter.transform.forward + (-PlayerManager.main.FocusedCharacter.transform.right));
                    PlayerManager.main.PlayerRogue.transform.rotation = PlayerManager.main.FocusedCharacter.transform.rotation;
                }

                PlayerManager.main.PlayerFairy.GetComponent<NavMeshAgent>().enabled = true;
                PlayerManager.main.PlayerFairy.GetComponent<NavMeshAgent>().ResetPath();
                PlayerManager.main.PlayerRogue.GetComponent<NavMeshAgent>().enabled = true;
                PlayerManager.main.PlayerRogue.GetComponent<NavMeshAgent>().ResetPath();
                PlayerManager.main.PlayerTank.GetComponent<NavMeshAgent>().enabled = true;
                PlayerManager.main.PlayerTank.GetComponent<NavMeshAgent>().ResetPath();
            }
        }

        animator.SetBool("isOpen", false);
        GameObject.Find("PermanentHUD").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("PermanentHUD").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("PermanentHUD").GetComponent<CanvasGroup>().blocksRaycasts = true;
        talker = null;

        GameObject obj;
        if ((obj = GameObject.Find("Loading")) != null)
        {
            obj.GetComponent<Animator>().SetBool("Fondu", true);
        }

        voices.Clear();
    }
}
