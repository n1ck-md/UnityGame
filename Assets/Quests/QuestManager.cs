using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("UI References")]
    public GameObject questMenuPanel;
    public GameObject questDetailsPanel;
    public TMP_Text questTitleText;
    public TMP_Text questDescriptionText;
    public TMP_Text questObjectivesText;
    public TMP_Text questRewardsText;
    public Transform activeQuestsPanel;
    public Transform completedQuestsPanel;
    public GameObject questButtonPrefab;

    [Header("Settings")]
    public Color activeQuestColor = Color.white;
    public Color completedQuestColor = new Color(0.7f, 0.7f, 0.7f);

    private List<Quest> activeQuests = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();
    private PlayerControls controls;
    private bool isQuestMenuOpen;
    private Quest currentQuest;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        controls = new PlayerControls();
        controls.Player.QuestMenu.performed += ctx => ToggleQuestMenu();
    }

    private void Start()
    {
        questMenuPanel.SetActive(false);
        questDetailsPanel.SetActive(false);
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void ToggleQuestMenu()
    {
        isQuestMenuOpen = !isQuestMenuOpen;
        questMenuPanel.SetActive(isQuestMenuOpen);

        if (isQuestMenuOpen)
        {
            UpdateQuestListUI();
        }
        else
        {
            CloseQuestDetails();
        }
    }

    public void UpdateQuestListUI()
    {
        ClearQuestButtons(activeQuestsPanel);
        ClearQuestButtons(completedQuestsPanel);

        foreach (Quest quest in activeQuests)
        {
            CreateQuestButton(quest, activeQuestsPanel, false);
        }

        foreach (Quest quest in completedQuests)
        {
            CreateQuestButton(quest, completedQuestsPanel, true);
        }
    }

    private void ClearQuestButtons(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateQuestButton(Quest quest, Transform parent, bool isCompleted)
    {
        GameObject buttonObj = Instantiate(questButtonPrefab, parent);
        buttonObj.name = "QuestBtn_" + quest.title.Replace(" ", ""); 

        TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            buttonText.text = quest.title;
            buttonText.color = isCompleted ? completedQuestColor : activeQuestColor;
            buttonText.fontStyle = isCompleted ? FontStyles.Italic : FontStyles.Normal;
        }

        Button button = buttonObj.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => ShowQuestDetails(quest));
    }

    public void ShowQuestDetails(Quest quest)
    {
        currentQuest = quest;
        questTitleText.text = quest.title;
        questDescriptionText.text = quest.description;

      
        questObjectivesText.text = "";
        foreach (string objective in quest.objectives)
        {
            questObjectivesText.text += "• " + objective + "\n";
        }

        
        questRewardsText.text = $"XP: {quest.xpReward}   Gold: {quest.goldReward}";

      
        if (quest.isCompleted)
        {
            questDescriptionText.text += "\n\n<color=#AAAAAA>(Completed)</color>";
        }

        questDetailsPanel.SetActive(true);
    }

    public void CloseQuestDetails()
    {
        questDetailsPanel.SetActive(false);
    }

    public void AddQuest(Quest quest)
    {
        if (!activeQuests.Contains(quest) && !completedQuests.Contains(quest))
        {
            activeQuests.Add(quest);
            Debug.Log($"Quest Added: {quest.title}");
            if (isQuestMenuOpen) UpdateQuestListUI();
        }
    }

    public void CompleteQuest(string questTitle)
    {
        Quest quest = activeQuests.Find(q => q.title == questTitle);
        if (quest != null)
        {
            quest.isCompleted = true;
            activeQuests.Remove(quest);
            completedQuests.Add(quest);
            Debug.Log($"Quest Completed: {quest.title}");
            if (isQuestMenuOpen) UpdateQuestListUI();
        }
    }

    public bool HasQuest(string questTitle, bool checkCompleted = false)
    {
        bool inActive = activeQuests.Exists(q => q.title == questTitle);
        bool inCompleted = completedQuests.Exists(q => q.title == questTitle);
        return checkCompleted ? (inActive || inCompleted) : inActive;
    }
}