using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextureAtlas
{

    public static readonly TextureAtlas _Instance = new TextureAtlas();

    public static Texture2D _ATLAS
    {
        get; private set;
    }

    public void CreateAtlas()
    {

        string[] blockTextures = Directory.GetFiles("textures/blocks/");

        foreach (string s in blockTextures)
        {
            int pixelSize = 16;

            int pixelWidth = pixelSize;
            int pixelHeight = pixelSize;

            int blockTextLen = blockTextures.Length - 1;

            int atlasWidth = Mathf.CeilToInt(Mathf.Sqrt(blockTextLen)) * pixelSize;
            int atlasHeight = Mathf.CeilToInt(Mathf.Sqrt(blockTextLen)) * pixelSize;

            Texture2D Atlas = new Texture2D((atlasWidth+(1*pixelSize)), atlasHeight);

            int c = 0;

            for (int x = 0; x < atlasWidth / pixelWidth; x++)
            {
                for (int y = 0; y < atlasWidth / pixelWidth; y++)
                {

                    if (c > blockTextures.Length - 1)
                    {
                        goto end;
                    }

                    Texture2D temp = new Texture2D(0, 0);
                    temp.LoadImage(File.ReadAllBytes(blockTextures[c]));

                    Atlas.SetPixels(x * pixelWidth, y * pixelHeight, pixelWidth, pixelHeight, temp.GetPixels());

                    float startX = x * pixelWidth;
                    float startY = y * pixelHeight;

                    float perPixelRatioX = 1.0f / Atlas.width;
                    float perPixelRatioY = 1.0f / Atlas.height;

                    startX *= perPixelRatioX;
                    startY *= perPixelRatioY;

                    float endX = startX + (perPixelRatioX * pixelWidth);
                    float endY = startY + (perPixelRatioY * pixelHeight);

                    UvMap m = new UvMap(blockTextures[c], new Vector2[]{
                        new Vector2(startX, startY),
                        new Vector2(startX, endY),
                        new Vector2(endX, startY),
                        new Vector2(endX, endY)
                    });

                    m.Register();

                    c++;
                }
            }

        end:
            ;

            _ATLAS = Atlas;
            File.WriteAllBytes("atlas.png", Atlas.EncodeToPNG());
        }
    }

}