using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Audio;

public class SlidingPuzzleManager : MonoBehaviour
{
    public GameObject puzzlePanel;
    public List<Button> tileButtons; 
    public Sprite emptySprite; 
    private int emptyIndex;
    public GameObject npcObject; 
    public AudioSource audioSource;    
    public AudioClip successClip;      
    public Button closeButton; 
    private bool puzzleCompleted = false;
    public GameObject RightBookGameObject;
    private bool RightBookSpawned = false;


    private void Start()
    {
        puzzlePanel.SetActive(false);
    }

    public void OpenPuzzle()
    {
        if (puzzleCompleted) return;

        puzzlePanel.SetActive(true);
        if (closeButton != null) closeButton.gameObject.SetActive(true);
        ShuffleTiles();
    }

    public void ClosePuzzle()
    {
        puzzlePanel.SetActive(false);
        if (closeButton != null) closeButton.gameObject.SetActive(false);
    }

    public void ShuffleTiles()
    {
        List<Sprite> sprites = tileButtons.Select(b => b.image.sprite).ToList();
        do
        {
            sprites = sprites.OrderBy(x => Random.value).ToList();
        } while (!IsSolvable(sprites));

        for (int i = 0; i < tileButtons.Count; i++)
        {
            tileButtons[i].image.sprite = sprites[i];
            int index = i; // capture for lambda
            tileButtons[i].onClick.RemoveAllListeners();
            tileButtons[i].onClick.AddListener(() => TryMove(index));
            if (sprites[i] == emptySprite)
                emptyIndex = i;
        }
    }

    public void TryMove(int clickedIndex)
    {
        if (IsAdjacent(clickedIndex, emptyIndex))
        {
            // Swap sprites
            Sprite temp = tileButtons[clickedIndex].image.sprite;
            tileButtons[clickedIndex].image.sprite = emptySprite;
            tileButtons[emptyIndex].image.sprite = temp;

            emptyIndex = clickedIndex;
            //Debug.Log($"Tile clikcuit index: {clickedIndex}, index la gol: {emptyIndex}");


            if (IsSolved())
            {
                Debug.Log("Puzzle Solved!");
                puzzleCompleted = true;
                if (!RightBookSpawned && RightBookGameObject != null)
                {
                    RightBookGameObject.SetActive(true);  // Activate Leo
                    RightBookSpawned = true;
                }

                ClosePuzzle();
                if (npcObject != null)
                {
                    npcObject.SetActive(true);
                }

                if (audioSource != null && successClip != null)
                {
                    audioSource.PlayOneShot(successClip);
                }
            }
        }
    }

    bool IsAdjacent(int i, int j)
    {
        int row1 = i / 3, col1 = i % 3;
        int row2 = j / 3, col2 = j % 3;
        return Mathf.Abs(row1 - row2) + Mathf.Abs(col1 - col2) == 1;
    }

    bool IsSolved()
    {
        string[] correctOrder = {
        "1_0", "2_0", "3_0",
        "4_0", "5_0", "6_0",
        "7_0", "8_0", "9_0"
    };

        for (int i = 0; i < tileButtons.Count; i++)
        {
            string currentName = tileButtons[i].image.sprite.name;
            if (currentName != correctOrder[i])
            {
                Debug.Log($"Not solved: index {i}, expected {correctOrder[i]}, got {currentName}");
                return false;
            }
        }
        return true;
    }


    bool IsSolvable(List<Sprite> sprites)
    {
        List<int> numbers = new List<int>();

        foreach (var sprite in sprites)
        {
            if (sprite.name == "2_0") continue;
            int num = int.Parse(sprite.name.Split('_')[0]);
            numbers.Add(num);
        }

        int inversionCount = 0;
        for (int i = 0; i < numbers.Count - 1; i++)
        {
            for (int j = i + 1; j < numbers.Count; j++)
            {
                if (numbers[i] > numbers[j])
                    inversionCount++;
            }
        }

        return inversionCount % 2 == 0;
    }
}


