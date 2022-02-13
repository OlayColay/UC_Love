using System.Collections;
using System.Collections.Generic;
using System;
using Yarn.Unity;

/// <summary> Static Yarn Spinner functions. Functions return variables (int, string, etc.) </summary>
public static class YarnFunctions
{
    [YarnFunction("RandomRange")]
    public static int RandomRange(int a, int b)
    {
        Random rng = new Random();
        return rng.Next(a, b + 1);
    }

    [YarnFunction("Random")]
    public static float Random()
    {
        return (float)RandomRange(0, 100) / 100;
    }

    [YarnFunction("Dice")]
    public static int Dice(int a)
    {
        return RandomRange(1, a);
    }

    [YarnFunction("GetRelationshipScore")]
    public static int GetRelationshipScore(string character)
    {
        // TODO: Return relationship score based on enum converted from string
        return 0;
    }
}
