using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Bracket
{
    public List<Match> matches;
    public int roundNum;

    public Bracket()
    {
        matches = new List<Match>();
        roundNum = 1;
    }
}
