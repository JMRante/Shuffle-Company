using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TexturePatternType
{
    SimpleOne,
    SimpleTwo,
    SimpleFour,
    Blob,
    Edge
}

[System.Serializable]
public class StageTextureDefinition
{
    public int x;
    public int y;
    public int pattern;

    [System.NonSerialized]
    private static readonly Dictionary<int, Vector2Int> blobTable = new Dictionary<int, Vector2Int>
    {
        {   0, new Vector2Int(0, 0) },
        {   4, new Vector2Int(1, 0) },
        {  92, new Vector2Int(2, 0) },
        { 124, new Vector2Int(3, 0) },
        { 116, new Vector2Int(4, 0) },
        {  80, new Vector2Int(5, 0) },

        {  16, new Vector2Int(0, 1) },
        {  20, new Vector2Int(1, 1) },
        {  87, new Vector2Int(2, 1) },
        { 223, new Vector2Int(3, 1) },
        { 241, new Vector2Int(4, 1) },
        {  21, new Vector2Int(5, 1) },
        {  64, new Vector2Int(6, 1) },

        {  29, new Vector2Int(0, 2) },
        { 117, new Vector2Int(1, 2) },
        {  85, new Vector2Int(2, 2) },
        {  71, new Vector2Int(3, 2) },
        { 221, new Vector2Int(4, 2) },
        { 125, new Vector2Int(5, 2) },
        { 112, new Vector2Int(6, 2) },

        {  31, new Vector2Int(0, 3) },
        { 253, new Vector2Int(1, 3) },
        { 113, new Vector2Int(2, 3) },
        {  28, new Vector2Int(3, 3) },
        { 127, new Vector2Int(4, 3) },
        { 247, new Vector2Int(5, 3) },
        { 209, new Vector2Int(6, 3) },

        {  23, new Vector2Int(0, 4) },
        { 199, new Vector2Int(1, 4) },
        { 213, new Vector2Int(2, 4) },
        {  95, new Vector2Int(3, 4) },
        { 255, new Vector2Int(4, 4) },
        { 245, new Vector2Int(5, 4) },
        {  81, new Vector2Int(6, 4) },

        {   5, new Vector2Int(0, 5) },
        {  84, new Vector2Int(1, 5) },
        {  93, new Vector2Int(2, 5) },
        { 119, new Vector2Int(3, 5) },
        { 215, new Vector2Int(4, 5) },
        { 193, new Vector2Int(5, 5) },
        {  17, new Vector2Int(6, 5) },

        {   1, new Vector2Int(1, 6) },
        {   7, new Vector2Int(2, 6) },
        { 197, new Vector2Int(3, 6) },
        {  69, new Vector2Int(4, 6) },
        {  68, new Vector2Int(5, 6) },
        {  65, new Vector2Int(6, 6) }
    };

    public StageTextureDefinition(int x, int y, TexturePatternType pattern)
    {
        this.x = x;
        this.y = y;
        this.pattern = (int)pattern;
    }

    public TexturePatternType GetTexturePatternType()
    {
        return (TexturePatternType)pattern;
    }

    public void SetTexturePatternType(TexturePatternType pattern)
    {
        this.pattern = (int)pattern;
    }

    public int CalculateTextureIndex(Vector3Int cellPosition, Chunk chunk, Vector3Int direction)
    {
        Vector2Int textureCoordinate = new Vector2Int(x, y);

        switch (GetTexturePatternType())
        {
            case TexturePatternType.SimpleOne:
            {
                break;
            }
            case TexturePatternType.SimpleTwo:
            {
                Vector3Int cellWorldPosition = Vector3Int.FloorToInt(StageChunks.ChunkPositionToWorldPosition(cellPosition, chunk.transform.position));
                textureCoordinate += CalculateMultiTileIndex(cellWorldPosition, direction, 2);
                break;
            }
            case TexturePatternType.SimpleFour:
            {
                Vector3Int cellWorldPosition = Vector3Int.FloorToInt(StageChunks.ChunkPositionToWorldPosition(cellPosition, chunk.transform.position));
                textureCoordinate += CalculateMultiTileIndex(cellWorldPosition, direction, 4);
                break;
            }
            case TexturePatternType.Blob:
            {
                textureCoordinate += CalculateBlobTextureIndex(cellPosition, chunk, direction);
                break;
            }
            case TexturePatternType.Edge:
            {
                break;
            }
        }

        return textureCoordinate.x + (textureCoordinate.y * 32);
    }

    public Vector2Int CalculateMultiTileIndex(Vector3Int cellPosition, Vector3Int direction, int size)
    {
        if (direction.x > 0)
        {
            int x = Mathf.Abs(cellPosition.z) % size;
            int y = size - Mathf.Abs(cellPosition.y) % size;
            y = y == size ? 0 : y;
            return new Vector2Int(x, y);
        }
        else if (direction.y > 0)
        {
            int x = Mathf.Abs(cellPosition.x) % size;
            int y = size - Mathf.Abs(cellPosition.z) % size;
            y = y == size ? 0 : y;
            return new Vector2Int(x, y);
        }
        else if (direction.z > 0)
        {
            int x = size - Mathf.Abs(cellPosition.x) % size;
            int y = size - Mathf.Abs(cellPosition.y) % size;
            x = x == size ? 0 : x;
            y = y == size ? 0 : y;
            return new Vector2Int(x, y);
        }
        else if (direction.x < 0)
        {
            int x = size - (Mathf.Abs(cellPosition.z) % size);
            int y = size - (Mathf.Abs(cellPosition.y) % size);
            x = x == size ? 0 : x;
            y = y == size ? 0 : y;
            return new Vector2Int(x, y);
        }
        else if (direction.y < 0)
        {
            int x = Mathf.Abs(cellPosition.x) % size;
            int y = size - (Mathf.Abs(cellPosition.z) % size);
            y = y == size ? 0 : y;
            return new Vector2Int(x, y);
        }
        else if (direction.z < 0)
        {
            int x = Mathf.Abs(cellPosition.x) % size;
            int y = size - (Mathf.Abs(cellPosition.y) % size);
            y = y == size ? 0 : y;
            return new Vector2Int(x, y);
        }

        return Vector2Int.zero;
    }

    public Vector2Int CalculateBlobTextureIndex(Vector3Int cellPosition, Chunk chunk, Vector3Int direction)
    {
        Vector3Int north = Vector3Int.zero;
        Vector3Int east = Vector3Int.zero;

        if (direction == Vector3Int.up)
        {
            north = Vector3Int.forward;
            east = Vector3Int.right;
        }
        else if (direction == Vector3Int.down)
        {
            north = Vector3Int.forward;
            east = Vector3Int.right;
        }
        else if (direction == Vector3Int.forward)
        {
            north = Vector3Int.up;
            east = Vector3Int.left;
        }
        else if (direction == Vector3Int.back)
        {
            north = Vector3Int.up;
            east = Vector3Int.right;
        }
        else if (direction == Vector3Int.right)
        {
            north = Vector3Int.up;
            east = Vector3Int.forward;
        }
        else if (direction == Vector3Int.left)
        {
            north = Vector3Int.up;
            east = Vector3Int.back;
        }

        int blobIndex = 0;

        bool n = chunk.GetChunkCell(cellPosition + north).IsFilled();
        bool ne = chunk.GetChunkCell(cellPosition + north + east).IsFilled();
        bool e = chunk.GetChunkCell(cellPosition + east).IsFilled();
        bool se = chunk.GetChunkCell(cellPosition + -north + east).IsFilled();
        bool s = chunk.GetChunkCell(cellPosition + -north).IsFilled();
        bool sw = chunk.GetChunkCell(cellPosition + -north + -east).IsFilled();
        bool w = chunk.GetChunkCell(cellPosition + -east).IsFilled();
        bool nw = chunk.GetChunkCell(cellPosition + north + -east).IsFilled();

        // N
        if (n)
        {
            blobIndex += 1;
        }

        // NE
        if (ne && n && e)
        {
            blobIndex += 2;
        }

        // E
        if (e)
        {
            blobIndex += 4;
        }

        // SE
        if (se && s && e)
        {
            blobIndex += 8;
        }

        // S
        if (s)
        {
            blobIndex += 16;
        }

        // SW
        if (sw && s && w)
        {
            blobIndex += 32;
        }

        // W
        if (w)
        {
            blobIndex += 64;
        }

        // NW
        if (nw && n && w)
        {
            blobIndex += 128;
        }

        if (blobTable.ContainsKey(blobIndex))
        {
            return blobTable[blobIndex];
        }
        else
        {
            return Vector2Int.zero;
        }
    }
}
