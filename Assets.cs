using System.Net.Mime;
using System.Numerics;
using Raylib_cs;

namespace RockExchange;

public class Assets
{
    private static Assets? _instance;
    public const int RockSize = 256;
    private const int HoverSize = 330;

    private Assets()
    {
        // Private constructor to prevent instantiation
        
        // Load backgrounds
        Backgrounds.Add(Time.Day, Raylib.LoadTexture("assets/textures/day-background.png"));
        Backgrounds.Add(Time.Night, Raylib.LoadTexture("assets/textures/night-background.png"));
        
        // Load rock textures
        Rocks.Add(RockKind.Bag, Raylib.LoadTexture("assets/textures/bag-rock.png"));
        Rocks.Add(RockKind.Blue, Raylib.LoadTexture("assets/textures/blue-rock.png"));
        Rocks.Add(RockKind.Coin, Raylib.LoadTexture("assets/textures/coin-rock.png"));
        Rocks.Add(RockKind.Cyan, Raylib.LoadTexture("assets/textures/cyan-rock.png"));
        Rocks.Add(RockKind.Dark, Raylib.LoadTexture("assets/textures/dark-rock.png"));
        Rocks.Add(RockKind.Green, Raylib.LoadTexture("assets/textures/green-rock.png"));
        Rocks.Add(RockKind.Lime, Raylib.LoadTexture("assets/textures/lime-rock.png"));
        Rocks.Add(RockKind.Horizontal, Raylib.LoadTexture("assets/textures/map-horizontal-rock.png"));
        Rocks.Add(RockKind.Perpendicular, Raylib.LoadTexture("assets/textures/map-perpendicular-rock.png"));
        Rocks.Add(RockKind.Vertical, Raylib.LoadTexture("assets/textures/map-vertical-rock.png"));
        Rocks.Add(RockKind.Navy, Raylib.LoadTexture("assets/textures/navy-rock.png"));
        Rocks.Add(RockKind.Orange, Raylib.LoadTexture("assets/textures/orange-rock.png"));
        Rocks.Add(RockKind.Potion, Raylib.LoadTexture("assets/textures/potion-rock.png"));
        Rocks.Add(RockKind.Purple, Raylib.LoadTexture("assets/textures/purple-rock.png"));
        Rocks.Add(RockKind.Red, Raylib.LoadTexture("assets/textures/red-rock.png"));
        Rocks.Add(RockKind.Stone, Raylib.LoadTexture("assets/textures/stone-rock.png"));
        Rocks.Add(RockKind.Violet, Raylib.LoadTexture("assets/textures/violet-rock.png"));
        Rocks.Add(RockKind.Yellow, Raylib.LoadTexture("assets/textures/yellow-rock.png"));
        
        foreach (KeyValuePair<RockKind, Texture2D> rock in Rocks)
        {
            Image image = Raylib.LoadImageFromTexture(rock.Value);
            unsafe
            {
                Raylib.ImageResize(&image, HoverSize, HoverSize);
                // Raylib.ImageCrop(&image, new Rectangle((float)HoverSize/RockSize/2, (float)HoverSize/RockSize/2, RockSize, RockSize));
                Raylib.ImageBlurGaussian(&image, 14);
                Raylib.ImageColorContrast(&image, -100);
                Raylib.ImageColorBrightness(&image, 100);
                Raylib.ImageColorBrightness(&image, 100);
                Raylib.ImageColorBrightness(&image, 100);
            }
            
            Texture2D imageTexture = Raylib.LoadTextureFromImage(image);  
            Raylib.UnloadImage(image);
            
            RenderTexture2D hoverRender = Raylib.LoadRenderTexture(RockSize, RockSize);
            
            Raylib.BeginTextureMode(hoverRender);
            Raylib.ClearBackground(Raylib.ColorAlpha(Color.Black, 0f));
            Raylib.DrawTextureRec(imageTexture, 
                new Rectangle(0, HoverSize, HoverSize, -HoverSize), 
                new Vector2(-(float)(HoverSize-RockSize)/2, -(float)(HoverSize-RockSize)/2), 
                Color.White);
            
            Raylib.EndTextureMode();
            
            Raylib.UnloadTexture(imageTexture);
            HoverRocks.Add(rock.Key, hoverRender.Texture);
        }
    }
    
    public static Assets Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Assets();
            }
            return _instance;
        }
    }
    
    public static void Reinstantiate()
    {
        _instance = new Assets();
    }
    
    
    
    public Dictionary<RockKind, Texture2D> Rocks = new();
    public Dictionary<RockKind, Texture2D> HoverRocks = new();
    public Dictionary<Time, Texture2D> Backgrounds = new();
}
