using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReplayInputs
{
    public List<PlayerInputEntry> InputEntries;
    public Vector3 StartPosition;
    public BugType BugType;
}
