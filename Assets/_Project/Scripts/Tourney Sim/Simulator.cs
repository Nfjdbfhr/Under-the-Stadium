using UnityEngine;
using System;
using System.Collections.Generic;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Tournament")]
    public List<Blade> contestants = new List<Blade>();

    public Bracket currentBracket;

    [Header("Winner Probability")]
    [Tooltip("Higher values make stats have a stronger influence on the winner.")]
    [Range(1f, 4f)]
    public float statPower = 2f;

    [Tooltip("Prevents either contestant from having a 0% or 100% chance.")]
    [Range(0f, 50f)]
    public float minimumChance = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RunTournament();
    }

    /// <summary>
    /// Generates a bracket and simulates every round until there is a winner.
    /// </summary>
    void RunTournament()
    {
        currentBracket = GenerateBracket();
        SimulateTournament();
    }

    /// <summary>
    /// Simulates rounds until only one contestant remains.
    /// </summary>
    void SimulateTournament()
    {
        while (currentBracket.matches.Count > 0)
        {
            currentBracket = SimulateRound(currentBracket);
        }
    }

    /// <summary>
    /// Randomly pairs all contestants into the first round.
    /// </summary>
    Bracket GenerateBracket()
    {
        Bracket bracket = new Bracket();

        // Make a copy so we don't modify the original contestant list.
        List<Blade> entries = new List<Blade>(contestants);

        while (entries.Count >= 2)
        {
            Blade contestantOne = GetRandomContestant(entries);
            Blade contestantTwo = GetRandomContestant(entries);

            Match match = new Match(contestantOne, contestantTwo);
            bracket.matches.Add(match);
        }

        // TODO: Handle an odd number of contestants if needed.
        if (entries.Count == 1)
        {
            Debug.LogWarning($"{entries[0].Name} was not placed into the bracket because there was no opponent.");
        }

        return bracket;
    }

    /// <summary>
    /// Removes and returns a random contestant from a list.
    /// </summary>
    Blade GetRandomContestant(List<Blade> entries)
    {
        int randomIndex = UnityEngine.Random.Range(0, entries.Count);

        Blade contestant = entries[randomIndex];
        entries.RemoveAt(randomIndex);

        return contestant;
    }

    /// <summary>
    /// Simulates every match in the current round and creates the next round.
    /// </summary>
    Bracket SimulateRound(Bracket bracket)
    {
        Debug.Log($"======= Beginning Round {bracket.roundNum} =======");

        List<Blade> winners = DetermineRoundWinners(bracket.matches);

        // We have our tournament winner.
        if (winners.Count == 1)
        {
            DisplayWinner(winners[0]);

            bracket.matches.Clear();
            return bracket;
        }

        bracket.matches = CreateNextRoundMatches(winners);
        bracket.roundNum++;

        return bracket;
    }

    /// <summary>
    /// Determines the winner of every match in a round.
    /// </summary>
    List<Blade> DetermineRoundWinners(List<Match> matches)
    {
        List<Blade> winners = new List<Blade>();

        foreach (Match match in matches)
        {
            Blade winner = DetermineMatchWinner(match);

            winners.Add(winner);
        }

        return winners;
    }

    /// <summary>
    /// Determines the winner of a single match based on contestant stats.
    /// </summary>
    Blade DetermineMatchWinner(Match match)
    {
        Blade one = match.contestantOne;
        Blade two = match.contestantTwo;

        double oneChance = CalculateWinChance(one, two);

        // Use a float so we can get a more precise random percentage.
        float roll = UnityEngine.Random.Range(0f, 100f);

        if (roll < oneChance)
        {
            LogMatchResult(one, two, oneChance, roll);
            return one;
        }

        LogMatchResult(two, one, 100 - oneChance, roll);
        return two;
    }

    /// <summary>
    /// Calculates the chance of contestant one defeating contestant two.
    /// 
    /// Squaring the stats gives stronger contestants more influence
    /// while still allowing weaker contestants to score an upset.
    /// </summary>
    double CalculateWinChance(Blade one, Blade two)
    {
        double oneStat = one.GetStatPercentage();
        double twoStat = two.GetStatPercentage();

        // Apply the stat weighting.
        double oneWeight = Math.Pow(oneStat, statPower);
        double twoWeight = Math.Pow(twoStat, statPower);

        // Convert the weights into a percentage.
        double chance = oneWeight / (oneWeight + twoWeight) * 100;

        // Keep the result within our minimum/maximum bounds.
        chance = Math.Max(minimumChance, chance);
        chance = Math.Min(100 - minimumChance, chance);

        return chance;
    }

    /// <summary>
    /// Creates the matches for the next round using the previous round's winners.
    /// </summary>
    List<Match> CreateNextRoundMatches(List<Blade> winners)
    {
        List<Match> newMatches = new List<Match>();

        for (int i = 0; i + 1 < winners.Count; i += 2)
        {
            Match match = new Match(winners[i], winners[i + 1]);
            newMatches.Add(match);
        }

        // TODO: Handle an odd number of winners if needed.
        if (winners.Count % 2 != 0)
        {
            Debug.LogWarning("There was an odd number of winners. One contestant was not paired.");
        }

        return newMatches;
    }

    /// <summary>
    /// Logs the result of a match, including the calculated probability.
    /// </summary>
    void LogMatchResult(Blade winner, Blade loser, double winnerChance, float roll)
    {
        Debug.Log(
            $"{winner.Name} defeated {loser.Name} " +
            $"({winnerChance:F1}% chance | Roll: {roll:F1})"
        );
    }

    /// <summary>
    /// Displays the final tournament winner.
    /// </summary>
    void DisplayWinner(Blade winner)
    {
        Debug.Log($"🏆 WINNER: {winner.Name}");
    }
}