using UnityEngine;

[System.Serializable]
public class Blade
{
    public string Name;
    public int Attack;
    public int Defense;
    public int Stamina;
    public ContestantTags Tags;

    public Blade(string name = "", int attack = 0, int defense = 0, int stamina = 0)
    {
        Name = name;
        Attack = attack;
        Defense = defense;
        Stamina = stamina;
        Tags = new ContestantTags();
    }


    public int GetStatPercentage()
    {
        int total = Attack + Defense + Stamina;

        int percentage = total * 100 / 600;

        return percentage;
    }
}
