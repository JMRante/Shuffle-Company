using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ChunkCell
{
    public byte isFilled;

    public byte geometryType;

    public short textureDefinition;

    public byte decalDefinitionTop;
    public byte decalDefinitionLeft;
    public byte decalDefinitionRight;
    public byte decalDefinitionForward;
    public byte decalDefinitionBack;

    public byte inset;

    public ChunkCell(byte isFilled, short tilesetPatternIndex)
    {
        this.isFilled = isFilled;

        this.geometryType = 0;

        this.textureDefinition = tilesetPatternIndex;

        this.decalDefinitionTop = 0;
        this.decalDefinitionLeft = 0;
        this.decalDefinitionRight = 0;
        this.decalDefinitionForward = 0;
        this.decalDefinitionBack = 0;

        this.inset = 0;
    }
}