using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Resources
{
    public string name;
    public int count;
    public int limit;
    ResourceTypes types;

    public Resources(string name, int count, int limit, ResourceTypes types)
    {
        this.name = name;
        this.count = count;
        this.limit = limit;
        this.types = types;
    }
}
