using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrettyLinesLib
{
    public class SimpleLine : I2DLine
    {
        private readonly BasicEffect effect;
        private readonly GraphicsDevice device;
        private readonly VertexBuffer buffer;

        private Vector2 start;
        private Vector2 end;
        private Color color;

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

        #endregion

        public SimpleLine(Vector2 start, Vector2 end, Color color, BasicEffect effect, GraphicsDevice device)
        {
            this.start = start;
            this.end = end;
            this.color = color;
            this.effect = effect;
            this.device = device;
            buffer = new VertexBuffer(device, typeof(VertexPositionColor), 2, BufferUsage.WriteOnly);
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
                device.DrawPrimitives(PrimitiveType.LineList, 0, 1);
            }

            device.SetVertexBuffer(null);
            device.RasterizerState = temp;
        }

        private void UpdateBuffer()
        {
            buffer.SetData(new[]
            {
                new VertexPositionColor(new Vector3(Start, 0), color),
                new VertexPositionColor(new Vector3(End, 0), color)
            });
        }
    }
}