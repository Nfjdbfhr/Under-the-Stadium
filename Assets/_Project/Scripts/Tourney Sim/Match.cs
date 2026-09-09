using UnityEngine;

[System.Serializable]
public class Match
{
    public Blade contestantOne;
    public Blade contestantTwo;

    public Match(Blade one, Blade two)
    {
        contestantOne = one;
        contestantTwo = two;
    }
}
