using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ChunkCell
{
    public byte isFilled;

    public byte cellDefinition;

    public byte decalDefinitionTop;
    public byte decalDefinitionLeft;
    public byte decalDefinitionRight;
    public byte decalDefinitionForward;
    public byte decalDefinitionBack;

    public byte inset;

    public ChunkCell(byte isFilled, byte cellDefinition)
    {
        this.isFilled = isFilled;

        this.cellDefinition = cellDefinition;

        this.decalDefinitionTop = 0;
        this.decalDefinitionLeft = 0;
        this.decalDefinitionRight = 0;
        this.decalDefinitionForward = 0;
        this.decalDefinitionBack = 0;

        this.inset = 0;
    }


    public bool IsFilled()
    {
        return this.isFilled == 1 ? true : false;
    }

    public void SetFilled(bool isFilled)
    {
        if (isFilled)
        {
            this.isFilled = 1;
        }
        else
        {
            this.isFilled = 0;
        }
    }

    public bool IsInsetForward()
    {
        return (byte) (0b0000_0001 & this.inset) != 0;
    }

    public void SetInsetForward(bool isInset)
    {
        if (isInset)
        {
            this.inset |= 0b0000_0001;
        }
        else
        {
            this.inset &= 0b1111_1110;
        }
    }

    public bool IsInsetBack()
    {

        return (byte) (0b0000_0010 & this.inset) != 0;
    }

    public void SetInsetBack(bool isInset)
    {
        if (isInset)
        {
            this.inset |= 0b0000_0010;
        }
        else
        {
            this.inset &= 0b1111_1101;
        }
    }

    public bool IsInsetRight()
    {
        return (byte)(0b0000_0100 & this.inset) != 0;
    }

    public void SetInsetRight(bool isInset)
    {
        if (isInset)
        {
            this.inset |= 0b0000_0100;
        }
        else
        {
            this.inset &= 0b1111_1011;
        }
    }

    public bool IsInsetLeft()
    {
        return (byte)(0b0000_1000 & this.inset) != 0;
    }

    public void SetInsetLeft(bool isInset)
    {
        if (isInset)
        {
            this.inset |= 0b0000_1000;
        }
        else
        {
            this.inset &= 0b1111_0111;
        }
    }
}