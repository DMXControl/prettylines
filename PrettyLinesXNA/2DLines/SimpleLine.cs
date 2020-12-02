using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrettyLinesLib
{
    public class SimpleLine : Base2DLine
    {
        private Vector2 start;
        private Vector2 end;
        private Color color;

        #region Properties

        public override Vector2 Start
        {
            get { return start; }
            set
            {
                start = value;
                UpdateBuffer();
            }
        }

        public override Vector2 End
        {
            get { return end; }
            set
            {
                end = value;
                UpdateBuffer();
            }
        }

        public override Color Color
        {
            get { return color; }
            set
            {
                color = value;
                UpdateBuffer();
            }
        }

        #endregion

        public SimpleLine(Vector2 start, Vector2 end, Color color, BasicEffect effect, GraphicsDevice device, string label = "")
        {
            this.start = start;
            this.end = end;
            this.color = color;
            this.effect = effect;
            this.device = device;
            this.Label = label;
            buffer = new VertexBuffer(device, typeof(VertexPositionColor), 2, BufferUsage.WriteOnly);
            UpdateBuffer();
        }

        public override void Draw(Matrix transformationMatrix)
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