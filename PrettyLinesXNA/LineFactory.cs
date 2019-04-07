using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrettyLinesLib
{
    public class LineFactory
    {
        private GraphicsDevice device;
        private BasicEffect effect;

        public LineFactory(GraphicsDeviceManager manager)
        {
            device = manager.GraphicsDevice;
            effect = new BasicEffect(device);

            effect.World = Matrix.CreateTranslation(Vector3.Zero);

            effect.View = Matrix.CreateLookAt(Vector3.Backward, Vector3.Forward, Vector3.Up);
            effect.Projection =
                Matrix.CreateOrthographicOffCenter(0, device.Viewport.Width, device.Viewport.Height, 0, -1, 1);

            effect.VertexColorEnabled = true;
        }

        public SimpleLine GetSimpleLine(Vector2 start, Vector2 end, Color color)
        {
            return new SimpleLine(start, end, color, effect, device);
        }

        public ThickLine GetThickLine(Vector2 start, Vector2 end, Color color, float thickness)
        {
            return new ThickLine(start, end, color, thickness, effect, device);
        }

        public BezierCurve GetBezierCurve(Vector2 start, Vector2 end, Color color, float thickness)
        {
            return new BezierCurve(effect, device, start, end, color, 25, thickness);
        }

        public RightAngledLine GetAngledLine(Vector2 start, Vector2 end,Color color, float thickness)
        {
            return new RightAngledLine(device, effect, start, end, color, thickness);
        }
    }
}