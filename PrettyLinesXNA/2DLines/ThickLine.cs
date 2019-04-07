using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrettyLinesLib
{
    public class ThickLine : IThick2DLine
    {
        private readonly BasicEffect _effect;
        private readonly GraphicsDevice _device;
        private readonly VertexBuffer _buffer;

        private Vector2 _start;
        private Vector2 _end;
        private Color _color;
        private float _thickness;

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

        public float Thickness
        {
            get { return _thickness; }
            set
            {
                _thickness = value;
                UpdateBuffer();
            }
        }

        #endregion

        public ThickLine(Vector2 start, Vector2 end, Color color, float thickness, BasicEffect effect,
            GraphicsDevice device)
        {
            _start = start;
            _end = end;
            _color = color;
            _thickness = thickness;
            _effect = effect;
            _device = device;
            _buffer = new VertexBuffer(device, typeof(VertexPositionColor), 4, BufferUsage.WriteOnly);
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
                _device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }

            _device.SetVertexBuffer(null);
            _device.RasterizerState = temp;
        }

        private void UpdateBuffer()
        {
            var matrix = Matrix.CreateRotationZ(MathHelper.PiOver2);
            var transform = Vector2.Transform((_start - _end), matrix);
            transform.Normalize();
            var toAdd = transform * _thickness * 0.5f;
            var data = new[]
            {
                new VertexPositionColor(new Vector3(_start + toAdd, 0), _color),
                new VertexPositionColor(new Vector3(_start - toAdd, 0), _color),
                new VertexPositionColor(new Vector3(_end + toAdd, 0), _color),
                new VertexPositionColor(new Vector3(_end - toAdd, 0), _color),
            };
            _buffer.SetData(data);
        }
    }
}