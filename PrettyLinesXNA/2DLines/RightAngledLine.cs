using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrettyLinesLib
{
    public class RightAngledLine : IThick2DLine
    {
        private readonly GraphicsDevice device;
        private readonly BasicEffect effect;
        private readonly VertexBuffer buffer;

        private Vector2 start;
        private Vector2 end;
        private Color color;
        private float thickness;

        public Vector2 Start
        {
            get { return start; }
            set
            {
                start = value;
                UpdateBuffer();
            }
        }

        public Vector2 End
        {
            get { return end; }
            set
            {
                end = value;
                UpdateBuffer();
            }
        }

        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                UpdateBuffer();
            }
        }

        public float Thickness
        {
            get { return thickness; }
            set
            {
                thickness = value;
                UpdateBuffer();
            }
        }

        public RightAngledLine(GraphicsDevice device, BasicEffect effect, Vector2 start, Vector2 end, Color color,
            float thickness)
        {
            this.device = device;
            this.effect = effect;
            this.start = start;
            this.end = end;
            this.color = color;
            this.thickness = thickness;
            buffer = new VertexBuffer(device, typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
            UpdateBuffer();
        }

        public void Draw()
        {
            device.SetVertexBuffer(buffer);

            var temp = device.RasterizerState;

            var state = new RasterizerState();

            state.MultiSampleAntiAlias = true;
            state.CullMode = CullMode.None;

            device.RasterizerState = state;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 6);
            }

            device.SetVertexBuffer(null);
            device.RasterizerState = temp;
        }

        private void UpdateBuffer()
        {
            Vector2 addDiagonal;
            Vector2 addVertical;

            Vector2 angle1;
            Vector2 angle2;

            if (start.X < end.X)
            {
                angle1 = new Vector2((start.X + end.X) * 0.5f, start.Y);
                angle2 = new Vector2((start.X + end.X) * 0.5f, end.Y);
                addVertical = Vector2.UnitY * thickness * 0.5f;
                if (start.Y < end.Y)
                {
                    addDiagonal = new Vector2(1, -1) * thickness * 0.5f;
                    buffer.SetData(new[]
                    {
                        new VertexPositionColor(new Vector3(start - addVertical, 0), color),
                        new VertexPositionColor(new Vector3(start + addVertical, 0), color),
                        new VertexPositionColor(new Vector3(angle1 + addDiagonal, 0), color),
                        new VertexPositionColor(new Vector3(angle1 - addDiagonal, 0), color),
                        new VertexPositionColor(new Vector3(angle2 + addDiagonal, 0), color),
                        new VertexPositionColor(new Vector3(angle2 - addDiagonal, 0), color),
                        new VertexPositionColor(new Vector3(end - addVertical, 0), color),
                        new VertexPositionColor(new Vector3(end + addVertical, 0), color),
                    });
                }
                else
                {
                    addDiagonal = Vector2.One * thickness * 0.5f;
                    buffer.SetData(new[]
                    {
                        new VertexPositionColor(new Vector3(start + addVertical, 0), color),
                        new VertexPositionColor(new Vector3(start - addVertical, 0), color),
                        new VertexPositionColor(new Vector3(angle1 + addDiagonal, 0), color),
                        new VertexPositionColor(new Vector3(angle1 - addDiagonal, 0), color),
                        new VertexPositionColor(new Vector3(angle2 + addDiagonal, 0), color),
                        new VertexPositionColor(new Vector3(angle2 - addDiagonal, 0), color),
                        new VertexPositionColor(new Vector3(end + addVertical, 0), color),
                        new VertexPositionColor(new Vector3(end - addVertical, 0), color),
                    });
                }
                
            }
            else
            {
                angle1 = new Vector2(start.X, (start.Y + end.Y) * 0.5f);
                angle2 = new Vector2(end.X, (start.Y + end.Y) * 0.5f);
                addVertical = Vector2.UnitX * thickness * 0.5f;
                if (start.Y < end.Y)
                {
                    addDiagonal = Vector2.One * thickness * 0.5f;
                    buffer.SetData(new[]
                    {
                        new VertexPositionColor(new Vector3(start + addVertical, 0), color),
                        new VertexPositionColor(new Vector3(start - addVertical, 0), color),
                        new VertexPositionColor(new Vector3(angle1 + addDiagonal, 0), color),
                        new VertexPositionColor(new Vector3(angle1 - addDiagonal, 0), color),
                        new VertexPositionColor(new Vector3(angle2 + addDiagonal, 0), color),
                        new VertexPositionColor(new Vector3(angle2 - addDiagonal, 0), color),
                        new VertexPositionColor(new Vector3(end + addVertical, 0), color),
                        new VertexPositionColor(new Vector3(end - addVertical, 0), color),
                    });
                }
                else
                {
                    addDiagonal = new Vector2(1, -1) * thickness * 0.5f;
                    buffer.SetData(new[]
                    {
                        new VertexPositionColor(new Vector3(start + addVertical, 0), color),
                        new VertexPositionColor(new Vector3(start - addVertical, 0), color),
                        new VertexPositionColor(new Vector3(angle1 + addDiagonal, 0), color),
                        new VertexPositionColor(new Vector3(angle1 - addDiagonal, 0), color),
                        new VertexPositionColor(new Vector3(angle2 + addDiagonal, 0), color),
                        new VertexPositionColor(new Vector3(angle2 - addDiagonal, 0), color),
                        new VertexPositionColor(new Vector3(end + addVertical, 0), color),
                        new VertexPositionColor(new Vector3(end - addVertical, 0), color),
                    });
                }
                
            }
        }
    }
}