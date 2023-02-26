using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WS
{
    public Dictionary<string, object> worldState = new Dictionary<string, object>();

    public WS()
    {
        
    }

    public HashSet<string> GetPropertyKeys()
    {
        var keys = new HashSet<string>();

        foreach (var entry in worldState)
        {
            keys.Add(entry.Key);
        }

        return keys;
    }

    public bool HasProperty(string keyProperty) => worldState.ContainsKey(keyProperty);

    public object GetProperty( string keyProperty )
    {
        foreach (var entry in worldState)
        {
            if (entry.Key == keyProperty)
            {
                return entry.Value;
            }
        }

        return null;
    }

    public void SetProperty( string keyProperty, object value )
    {
        if (worldState.ContainsKey(keyProperty))
        {
            worldState[keyProperty] = value;
            return;
        }
            
        worldState.Add(keyProperty, value);
    }

    public int GetNumWorldStateDifferences(WS worldStateB)
    {
        HashSet<string> keysThis = GetPropertyKeys();
        HashSet<string> keysOther = worldStateB.GetPropertyKeys();

        HashSet<string> sharedKeys = new HashSet<string>(keysThis);
        sharedKeys.UnionWith(keysOther);

        int diffs = 0;

        bool hasThis, hasOther;

        foreach (string keyProperty in sharedKeys)
        {
            hasThis = HasProperty(keyProperty);
            hasOther = worldStateB.HasProperty(keyProperty);

            if (hasThis && hasOther)
            {
                if (!GetProperty(keyProperty).Equals(worldStateB.GetProperty(keyProperty)))
                    ++diffs;
            }
            else if (hasThis || hasOther)
                ++diffs;
        }

        return diffs;
    }

    public int GetNumUnsatisfiedWorldStateProps(WS worldStateB)
    {
        HashSet<string> keysThis = GetPropertyKeys();
        HashSet<string> keysOther = worldStateB.GetPropertyKeys();

        HashSet<string> sharedKeys = new HashSet<string>(keysThis);
        sharedKeys.UnionWith(keysOther);

        int unsatisfied = 0;

        foreach (string keyProperty in sharedKeys)
        {
            if (!HasProperty(keyProperty))
                continue;

            if (!worldStateB.HasProperty(keyProperty))
                ++unsatisfied;

            if (!GetProperty(keyProperty).Equals(worldStateB.GetProperty(keyProperty)))
                ++unsatisfied;
        }

        return unsatisfied;
    }

    public void ResetWS() => worldState.Clear();
    public void CopyWorldState(WS worldState) => this.worldState = new Dictionary<string, object>(worldState.worldState); //this looks weird but whatever


}

