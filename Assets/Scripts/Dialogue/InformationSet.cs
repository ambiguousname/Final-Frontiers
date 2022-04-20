using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public static class InformationSet
{
    public static HashSet<string> knownInformation;

    [YarnCommand("setInfo")]
    public static void AddInformation(string info) {
        knownInformation.Add(info);
    }

    [YarnFunction("hasInfo")]
    public static bool HasInformation(string info) {
        return knownInformation.Contains(info);
    }
}
