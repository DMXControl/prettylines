using Microsoft.Xna.Framework;

namespace PrettyLinesLib
{
    public interface I2DLine
    {
        Vector2 Start { get; set; }
        Vector2 End { get; set; }
        Color Color { get; set; }

        void Draw(Matrix transformationMatrix);
    }
}