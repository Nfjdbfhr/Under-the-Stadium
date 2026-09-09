using UnityEngine;

[System.Serializable]
public class Blade
{
    public string Name;
    public int Attack;
    public int Defense;
    public int Stamina;

    public int GetStatPercentage()
    {
        int total = Attack + Defense + Stamina;

        int percentage = total * 100 / 600;

        return percentage;
    }
}
