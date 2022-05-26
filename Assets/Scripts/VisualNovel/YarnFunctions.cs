using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Yarn.Unity;

/// <summary> Static Yarn Spinner functions. Functions return variables (int, string, etc.) </summary>
public static class YarnFunctions
{
    [YarnFunction("RandomRange")]
    public static int RandomRange(int a, int b)
    {
        System.Random rng = new System.Random();
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

    [YarnFunction("GymMinigameWon")]
    public static bool GymMinigameWon()
    {
        return GymMinigameController.minigameWon;
    }

    [YarnFunction("PlazaMinigameWon")]
    public static bool PlazaMinigameWon()
    {
        return PlayerController.gameWon;
    }

    [YarnFunction("CafeMinigameWon")]
    public static bool CafeMinigameWon()
    {
        return CafeMinigameController.gameWon;
    }

    [YarnFunction("LoadVariable")]
    public static dynamic LoadVariable(string name)
    {
        if (PlayerPrefs.GetInt(name, int.MinValue) > int.MinValue)
        {
            return PlayerPrefs.GetInt(name, int.MinValue);
        }
        else if (PlayerPrefs.GetFloat(name, float.MinValue) > int.MinValue)
        {
            return PlayerPrefs.GetFloat(name, float.MinValue);
        }
        else
        {
            return PlayerPrefs.GetString(name, "");
        }
    }

    [YarnFunction("IsComputer")]
    public static bool IsComputer()
    {
        return !Application.isMobilePlatform;
    }
}
