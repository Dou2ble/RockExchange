using System.Numerics;
using Raylib_cs;

namespace RockExchange;

public class Menu
{
    private Vector2 _buttonSize = new Vector2(160, 64);
    private const float Margin = 10f;
    private const float YOffset = 128f;
    private const string Title = "Rock Exchange";
    
    public GameMode? Draw()
    {
        GameMode? result = null; 
        
        float width = Raylib.GetScreenWidth();
        float height = Raylib.GetScreenHeight();
        
        Raylib.DrawTextureV(Assets.Instance.Icon, new Vector2(width/2 - 128, height/2 - _buttonSize.Y*4.8f), Color.White);
        Vector2 textSize = Raylib.MeasureTextEx(Assets.Instance.FontHeader, Title, Assets.FontSizeHeader, 0f);
        Raylib.DrawTextEx(Assets.Instance.FontHeader, Title, new Vector2(width/2 - textSize.X/2, height/2 - textSize.Y/2 - _buttonSize.Y*0.25f), Assets.FontSizeHeader, 0f, Color.White);

        if (GUI.ButtonCenter(new Vector2(width / 2, height / 2 + YOffset - _buttonSize.Y - Margin), _buttonSize, "Start"))
        {
            result = GameMode.Game;
        }
        
        // if (GUI.ButtonCenter(new Vector2(width / 2, height / 2 + YOffset), _buttonSize, "Options"))
        // {
        //     result = GameMode.Options;
        // }
        
        if (GUI.ButtonCenter(new Vector2(width / 2, height / 2 + YOffset + _buttonSize.Y + Margin), _buttonSize, "Quit"))
        {
            Raylib.CloseWindow();
        }

        return result;
    }
}