using UnityEngine;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public GameObject player;
    public DialogueManager dialogueManager;
    public LetterboxController letterbox;

    public Sprite introPortrait; 
    public string speakerName = "???";

    private PlayerMovement movementScript;

    void Start()
    {
        movementScript = player.GetComponent<PlayerMovement>();

        if (GameState.isLoadingFromSave)
        {
            
            movementScript.canMove = true;
            letterbox.HideBars();

            GameState.isLoadingFromSave = false; 
            return;
        }

        movementScript.canMove = false;

        StartCoroutine(PlayIntroCutscene());
    }

    IEnumerator PlayIntroCutscene()
    {
        letterbox.ShowBars();
     
        yield return MoveInDirection(Vector2.down, 1.5f);  
        yield return MoveInDirection(Vector2.right, 1f);   
        yield return MoveInDirection(Vector2.left, 1f);    
        yield return MoveInDirection(Vector2.down, 1.0f);  

        
        string[] lines = new string[]
        {
            "Ah...finally here",
            "My favorite place, strange but so familiar",
            "I should look around. Maybe Mango is in trouble"
        };

        dialogueManager.StartDialogue(lines, introPortrait, speakerName);

       
        yield return new WaitUntil(() => !dialogueManager.dialogueBox.activeSelf);

       
        letterbox.HideBars();
        movementScript.canMove = true;
    }

    IEnumerator MoveInDirection(Vector2 direction, float duration)
    {
        movementScript.ForceMove(direction);
        yield return new WaitForSeconds(duration);
        movementScript.StopForcedMove();
    }
}
