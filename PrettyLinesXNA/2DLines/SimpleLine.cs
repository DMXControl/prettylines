using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrettyLinesLib
{
    public class SimpleLine : I2DLine
    {
        private readonly BasicEffect _effect;
        private readonly GraphicsDevice _device;
        private readonly VertexBuffer _buffer;

        private Vector2 _start;
        private Vector2 _end;
        private Color _color;

        #region Properties

        public Vector2 Start
        {
            get { return _start; }
            set
            {
                _start = value;
                UpdateBuffer();
            }
        }

        public Vector2 End
        {
            get { return _end; }
            set
            {
                _end = value;
                UpdateBuffer();
            }
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                UpdateBuffer();
            }
        }

        #endregion

        public SimpleLine(Vector2 start, Vector2 end, Color color, BasicEffect effect, GraphicsDevice device)
        {
            _start = start;
            _end = end;
            _color = color;
            _effect = effect;
            _device = device;
            _buffer = new VertexBuffer(device, typeof(VertexPositionColor), 2, BufferUsage.WriteOnly);
            UpdateBuffer();
        }

        public void Draw()
        {
            _device.SetVertexBuffer(_buffer);

            var temp = _device.RasterizerState;

            var state = new RasterizerState();

            state.MultiSampleAntiAlias = true;
            state.CullMode = CullMode.None;

            _device.RasterizerState = state;


            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _device.DrawPrimitives(PrimitiveType.LineList, 0, 1);
            }

            _device.SetVertexBuffer(null);
            _device.RasterizerState = temp;
        }

        private void UpdateBuffer()
        {
            _buffer.SetData(new[]
            {
                new VertexPositionColor(new Vector3(Start, 0), _color),
                new VertexPositionColor(new Vector3(End, 0), _color)
            });
        }
    }
}