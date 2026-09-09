using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum ContestantTag
{
    Underdog,
    Favored,
    Dark_Horse,
    Longshot
};

[Serializable]
public class ContestantTags
{
    public List<ContestantTag> Tags = new List<ContestantTag>();

    public void AddTag(ContestantTag tag)
    {
        if (!Tags.Contains(tag))
        {
            Tags.Add(tag);
        }
    }

    public void RemoveTag(ContestantTag tag)
    {
        Tags.Remove(tag);
    }

    public bool HasTag(ContestantTag tag)
    {
        return Tags.Contains(tag);
    }

    public void ClearTags()
    {
        Tags.Clear();
    }

    public override string ToString()
    {
        if (Tags.Count == 0)
            return "No Tags";

        return string.Join(", ", Tags);
    }
}
