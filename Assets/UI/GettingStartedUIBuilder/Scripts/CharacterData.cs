using UnityEngine;

public enum ECharacterClass
{
    Knight, Ranger, Wizard
}

[CreateAssetMenu] //This adds an entry to the **Create** menu
public class CharacterData : ScriptableObject
{
    public string CharacterName;
    public ECharacterClass Class;
    public Sprite PortraitImage;
}