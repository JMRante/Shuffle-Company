using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChangeRecord
{
    private StateChangeType type;
    private GameObject obj;
    private int hashCode;

    public StateChangeRecord(StateChangeType type, GameObject obj)
    {
        this.type = type;
        this.obj = obj;

        string hashCodeString = type.ToString() + obj.ToString();
        this.hashCode = hashCodeString.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is StateChangeRecord)
        {
            StateChangeRecord otherObj = (obj as StateChangeRecord);
            return (this.type == otherObj.type && this.obj == otherObj.obj);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return hashCode;
    }

    public override string ToString()
    {
        return "SCR: " + type.ToString() + ", " + obj.ToString();
    }
}