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
        foreach (RockKind kind in Enum.GetValues(typeof(RockKind)))
        {
            // ReSharper disable once NullableWarningSuppressionIsUsed
            Rocks.Add(kind, Raylib.LoadTexture($"assets/textures/{Enum.GetName(kind)!.ToLower()}-rock.png"));  
        }
        
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
    
    
    
    public Dictionary<RockKind, Texture2D> Rocks = new();
    public Dictionary<RockKind, Texture2D> HoverRocks = new();
    public Texture2D Icon = Raylib.LoadTexture("assets/textures/icon.png");
    public Texture2D Background = Raylib.LoadTexture("assets/textures/background@720.png");
    public const int FontSize = 28;
    public const int FontSizeHeader = 64;
    public Font Font = Raylib.LoadFontEx("assets/fonts/LobsterTwo-Regular.ttf", FontSize, null, 0);
    public Font FontBold = Raylib.LoadFontEx("assets/fonts/LobsterTwo-Bold.ttf", FontSize, null, 0);
    public Font FontHeader = Raylib.LoadFontEx("assets/fonts/LobsterTwo-Bold.ttf", FontSizeHeader, null, 0);
}
