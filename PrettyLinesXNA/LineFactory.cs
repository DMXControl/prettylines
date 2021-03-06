﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrettyLinesLib
{
    public class LineFactory
    {
        private readonly GraphicsDevice device;
        private readonly BasicEffect effect;

        public LineFactory(GraphicsDevice device)
        {
            this.device = device;
            effect = new BasicEffect(this.device);

            effect.World = Matrix.CreateTranslation(Vector3.Zero);

            effect.View = Matrix.CreateLookAt(Vector3.Backward, Vector3.Forward, Vector3.Up);
            effect.Projection =
                Matrix.CreateOrthographicOffCenter(0, this.device.Viewport.Width, this.device.Viewport.Height, 0, -1, 1);

            effect.VertexColorEnabled = true;
        }

        public SimpleLine GetSimpleLine(Vector2 start, Vector2 end, Color color, string label = "")
        {
            return new SimpleLine(start, end, color, effect, device, label);
        }

        public ThickLine GetThickLine(Vector2 start, Vector2 end, Color color, float thickness, string label = "")
        {
            return new ThickLine(start, end, color, thickness, effect, device, label);
        }

        public BezierCurve GetBezierCurve(Vector2 start, Vector2 end, Color color, float thickness, string label = "")
        {
            return new BezierCurve(effect, device, start, end, color, 25, thickness, label);
        }

        public RightAngledLine GetAngledLine(Vector2 start, Vector2 end,Color color, float thickness, string label = "")
        {
            return new RightAngledLine(device, effect, start, end, color, thickness, label);
        }
    }
}