using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrettyLinesLib
{
    public class ThickLine : IThick2DLine
    {
        private readonly BasicEffect effect;
        private readonly GraphicsDevice device;
        private readonly VertexBuffer buffer;

        private Vector2 start;
        private Vector2 end;
        private Color color;
        private float thickness;

        #region Properties

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

        #endregion

        public ThickLine(Vector2 start, Vector2 end, Color color, float thickness, BasicEffect effect,
            GraphicsDevice device)
        {
            this.start = start;
            this.end = end;
            this.color = color;
            this.thickness = thickness;
            this.effect = effect;
            this.device = device;
            buffer = new VertexBuffer(device, typeof(VertexPositionColor), 4, BufferUsage.WriteOnly);
            UpdateBuffer();
        }

        public void Draw(Matrix transformationMatrix)
        {
            device.SetVertexBuffer(buffer);

            var temp = device.RasterizerState;

            var state = new RasterizerState();

            state.MultiSampleAntiAlias = true;
            state.CullMode = CullMode.None;

            device.RasterizerState = state;

            effect.Projection = transformationMatrix * Matrix.CreateOrthographicOffCenter(0,
                                    device.Viewport.Width,
                                    device.Viewport.Height,
                                    0,
                                    0,
                                    1);

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }

            device.SetVertexBuffer(null);
            device.RasterizerState = temp;
        }

        private void UpdateBuffer()
        {
            var matrix = Matrix.CreateRotationZ(MathHelper.PiOver2);
            var transform = Vector2.Transform((start - end), matrix);
            transform.Normalize();
            var toAdd = transform * thickness * 0.5f;
            var data = new[]
            {
                new VertexPositionColor(new Vector3(start + toAdd, 0), color),
                new VertexPositionColor(new Vector3(start - toAdd, 0), color),
                new VertexPositionColor(new Vector3(end + toAdd, 0), color),
                new VertexPositionColor(new Vector3(end - toAdd, 0), color),
            };
            buffer.SetData(data);
        }
    }
}