using System.Formats.Tar;
using System.Numerics;
using Raylib_cs;

namespace RockExchange;

public static class GUI
{
    public static readonly Color BackgroundColor = new Color(111, 109, 149, 200);
    public static readonly Color BackgroundColorAlt = new Color(255, 255, 255, 20);

    private static bool Vector2InRec(Vector2 pos, Rectangle rec)
    {
        return pos.X > rec.X && pos.Y > rec.Y && pos.X < rec.X + rec.Width && pos.Y < rec.Y + rec.Height;
    }

    public static bool ButtonCenter(Vector2 pos, Vector2 size, string label)
    {
        return Button(new Rectangle(pos.X - size.X/2, pos.Y - size.Y / 2, size.X, size.Y), label);
    }
        
    public static bool Button(Rectangle rec, string label)
    {
        bool result = false; 
            
        Raylib.DrawRectangleRounded(rec, 0.2f, 4, BackgroundColor);
        if (Vector2InRec(Raylib.GetMousePosition(), rec))
        {
            Raylib.DrawRectangleRounded(rec, 0.2f, 4, BackgroundColorAlt);
            if (Raylib.IsMouseButtonDown(MouseButton.Left))
            {
                Raylib.DrawRectangleRounded(rec, 0.2f, 4, BackgroundColorAlt);
            }
            else if (Raylib.IsMouseButtonReleased(MouseButton.Left))
            {
                result = true;
            }
        }

        Vector2 labelSize = Raylib.MeasureTextEx(Assets.Instance.FontBold, label, Assets.FontSize, 0);
        Raylib.DrawTextEx(Assets.Instance.FontBold, label, new Vector2(rec.X + rec.Width/2 - labelSize.X/2, rec.Y + rec.Height/2 - labelSize.Y/2), Assets.FontSize, 0, Color.White);

        return result;
    }
}