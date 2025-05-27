using UnityEngine;

[CreateAssetMenu(fileName = "NewInteraction", menuName = "Interaction/Interaction Data")]
public class InteractionData : ScriptableObject
{
    [TextArea(2, 5)]
    public string[] interactionTexts;
    public Sprite portrait;
}
