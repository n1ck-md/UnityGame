using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Quest
{
    public string title;                    
    public string description;              
    public List<string> objectives;         
    public int xpReward;                    
    public int goldReward;                  
    public bool isCompleted;                

    public Quest(string title, string description, List<string> objectives, int xpReward, int goldReward)
    {
        this.title = title;
        this.description = description;
        this.objectives = objectives;
        this.xpReward = xpReward;
        this.goldReward = goldReward;
        this.isCompleted = false;
    }
}
