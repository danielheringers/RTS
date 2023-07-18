using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIDGenerator
{
    private static int currentID = 0;

    public static int GenerateUniqueID()
    {
        return currentID++;
    }
}
