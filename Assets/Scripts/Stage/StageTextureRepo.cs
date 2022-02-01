using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTextureRepo
{
    private const int TEXTURE_MIPMAP_COUNT = 9;
    private const int TEXTURE_ANISOTROPIC_FILTERING = 16;
    private const FilterMode TEXTURE_FILTER_TYPE = FilterMode.Bilinear;
    private const TextureWrapMode TEXTURE_WRAP = TextureWrapMode.Clamp;
    private const int BASE_TEXTURE_WIDTH = 256;
    private const int BASE_TEXTURE_HEIGHT = 256;

    private Dictionary<string, Texture2D[]> textures;
    private Dictionary<string, Texture2DArray> textureArrays;

    public void LoadTextures(string pack)
    {
        textures = new Dictionary<string, Texture2D[]>();
        textureArrays = new Dictionary<string, Texture2DArray>();

        LoadTextureGroup("Albedo", "Textures/StageTextures/" + pack + "/Albedo");
        LoadTextureGroup("Smoothness", "Textures/StageTextures/" + pack + "/Smoothness");
        LoadTextureGroup("Metallic", "Textures/StageTextures/" + pack + "/Metallic");
        LoadTextureGroup("Normal", "Textures/StageTextures/" + pack + "/Normal");
    }

    public void LoadTexturesToMaterial(Material material)
    {
        if (textures != null)
        {
            foreach (KeyValuePair<string, Texture2D[]> textureGroup in textures)
            {
                if (textureGroup.Value.Length > 0)
                {
                    Texture2DArray array = null;

                    if (textureGroup.Key == "Normal")
                    {
                        array = new Texture2DArray(textureGroup.Value[0].width, textureGroup.Value[0].height, textureGroup.Value.Length, TextureFormat.DXT5, TEXTURE_MIPMAP_COUNT > 1);
                    }
                    else
                    {
                        array = new Texture2DArray(textureGroup.Value[0].width, textureGroup.Value[0].height, textureGroup.Value.Length, TextureFormat.DXT1, TEXTURE_MIPMAP_COUNT > 1);
                    }

                    array.anisoLevel = TEXTURE_ANISOTROPIC_FILTERING;
                    array.filterMode = TEXTURE_FILTER_TYPE;
                    array.wrapMode = TEXTURE_WRAP;

                    BuildTextureArray(textureGroup.Value, array);

                    material.SetTexture("_" + textureGroup.Key, array);
                }
            }
        }
    }

    private void LoadTextureGroup(string name, string path)
    {
        Texture2D[] unorderedTextures = Resources.LoadAll<Texture2D>(path);
        // Texture2D[] orderedTextures = new Texture2D[unorderedTextures.Length];

        // for (int i = 0; i < unorderedTextures.Length; i++)
        // {
        //     Texture2D texture = unorderedTextures[i];

        //     if (texture.width > BASE_TEXTURE_WIDTH || texture.height > BASE_TEXTURE_HEIGHT)
        //     {
        //         SplitMultiTexture(orderedTextures, i, texture);
        //     }
        //     else
        //     {
        //         orderedTextures[Convert.ToInt32(texture.name)] = texture;
        //     }
        // }

        textures.Add(name, unorderedTextures);
    }

    private void BuildTextureArray(Texture2D[] textures, Texture2DArray array)
    {
        int i = 0;

        foreach (Texture2D texture in textures)
        {            
            for (int mipLevel = 0; mipLevel < texture.mipmapCount; mipLevel++)
            {
                Graphics.CopyTexture(texture, 0, mipLevel, array, i, mipLevel);
            }

            i++;
        }
    }

    public KeyValuePair<string, int> GetTextureId(string name)
    {
        foreach (KeyValuePair<string, Texture2D[]> textureGroup in textures)
        {
            for (int i = 0; i < textureGroup.Value.Length; i++)
            {
                if (textureGroup.Value[i].name == name)
                {
                    return new KeyValuePair<string, int>(textureGroup.Key, i);
                }
            }
        }

        return new KeyValuePair<string, int>("", -1);
    }

    private void SplitMultiTexture(Texture2D[] textureArray, int startingIndex, Texture2D multiTexture)
    {
        for (int i = 0; i < multiTexture.width / BASE_TEXTURE_WIDTH; i++)
        {
            for (int j = 0; j < multiTexture.height / BASE_TEXTURE_HEIGHT; j++)
            {
                Texture2D tile = null;
                // if (multiTexture.name.StartsWith("ta") || multiTexture.name.StartsWith("tn") || multiTexture.name.StartsWith("th"))
                tile = new Texture2D(BASE_TEXTURE_WIDTH, BASE_TEXTURE_HEIGHT, TextureFormat.RGBA32, true);
                // else
                //     tile = new Texture2D(BASE_TEXTURE_WIDTH, BASE_TEXTURE_HEIGHT, TextureFormat.RGB24, true);
                tile.name = multiTexture.name + "_" + i + "_" + j;
                tile.SetPixels(0, 0, BASE_TEXTURE_WIDTH, BASE_TEXTURE_HEIGHT, multiTexture.GetPixels(i * BASE_TEXTURE_WIDTH, j * BASE_TEXTURE_HEIGHT, BASE_TEXTURE_WIDTH, BASE_TEXTURE_HEIGHT));
                tile.Apply(true, false);
                tile.Compress(true);

                textureArray[startingIndex + j + (i * (multiTexture.height / BASE_TEXTURE_HEIGHT))] = tile;
            }
        }
    }
}
