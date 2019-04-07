using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrettyLinesLib
{
    public class RightAngledLine : IThick2DLine
    {
        private readonly GraphicsDevice _device;
        private readonly BasicEffect _effect;
        private readonly VertexBuffer _buffer;

        private Vector2 _start;
        private Vector2 _end;
        private Color _color;
        private float _thickness;

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

        public RightAngledLine(GraphicsDevice device, BasicEffect effect, Vector2 start, Vector2 end, Color color,
            float thickness)
        {
            _device = device;
            _effect = effect;
            _start = start;
            _end = end;
            _color = color;
            _thickness = thickness;
            _buffer = new VertexBuffer(device, typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
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
                _device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 6);
            }

            _device.SetVertexBuffer(null);
            _device.RasterizerState = temp;
        }

        private void UpdateBuffer()
        {
            Vector2 addDiagonal;
            Vector2 addVertical;

            Vector2 angle1;
            Vector2 angle2;

            if (_start.X < _end.X)
            {
                angle1 = new Vector2((_start.X + _end.X) * 0.5f, _start.Y);
                angle2 = new Vector2((_start.X + _end.X) * 0.5f, _end.Y);
                addVertical = Vector2.UnitY * _thickness * 0.5f;
                if (_start.Y < _end.Y)
                {
                    addDiagonal = new Vector2(1, -1) * _thickness * 0.5f;
                    _buffer.SetData(new[]
                    {
                        new VertexPositionColor(new Vector3(_start - addVertical, 0), _color),
                        new VertexPositionColor(new Vector3(_start + addVertical, 0), _color),
                        new VertexPositionColor(new Vector3(angle1 + addDiagonal, 0), _color),
                        new VertexPositionColor(new Vector3(angle1 - addDiagonal, 0), _color),
                        new VertexPositionColor(new Vector3(angle2 + addDiagonal, 0), _color),
                        new VertexPositionColor(new Vector3(angle2 - addDiagonal, 0), _color),
                        new VertexPositionColor(new Vector3(_end - addVertical, 0), _color),
                        new VertexPositionColor(new Vector3(_end + addVertical, 0), _color),
                    });
                }
                else
                {
                    addDiagonal = Vector2.One * _thickness * 0.5f;
                    _buffer.SetData(new[]
                    {
                        new VertexPositionColor(new Vector3(_start + addVertical, 0), _color),
                        new VertexPositionColor(new Vector3(_start - addVertical, 0), _color),
                        new VertexPositionColor(new Vector3(angle1 + addDiagonal, 0), _color),
                        new VertexPositionColor(new Vector3(angle1 - addDiagonal, 0), _color),
                        new VertexPositionColor(new Vector3(angle2 + addDiagonal, 0), _color),
                        new VertexPositionColor(new Vector3(angle2 - addDiagonal, 0), _color),
                        new VertexPositionColor(new Vector3(_end + addVertical, 0), _color),
                        new VertexPositionColor(new Vector3(_end - addVertical, 0), _color),
                    });
                }
                
            }
            else
            {
                angle1 = new Vector2(_start.X, (_start.Y + _end.Y) * 0.5f);
                angle2 = new Vector2(_end.X, (_start.Y + _end.Y) * 0.5f);
                addVertical = Vector2.UnitX * _thickness * 0.5f;
                if (_start.Y < _end.Y)
                {
                    addDiagonal = Vector2.One * _thickness * 0.5f;
                    _buffer.SetData(new[]
                    {
                        new VertexPositionColor(new Vector3(_start + addVertical, 0), _color),
                        new VertexPositionColor(new Vector3(_start - addVertical, 0), _color),
                        new VertexPositionColor(new Vector3(angle1 + addDiagonal, 0), _color),
                        new VertexPositionColor(new Vector3(angle1 - addDiagonal, 0), _color),
                        new VertexPositionColor(new Vector3(angle2 + addDiagonal, 0), _color),
                        new VertexPositionColor(new Vector3(angle2 - addDiagonal, 0), _color),
                        new VertexPositionColor(new Vector3(_end + addVertical, 0), _color),
                        new VertexPositionColor(new Vector3(_end - addVertical, 0), _color),
                    });
                }
                else
                {
                    addDiagonal = new Vector2(1, -1) * _thickness * 0.5f;
                    _buffer.SetData(new[]
                    {
                        new VertexPositionColor(new Vector3(_start + addVertical, 0), _color),
                        new VertexPositionColor(new Vector3(_start - addVertical, 0), _color),
                        new VertexPositionColor(new Vector3(angle1 + addDiagonal, 0), _color),
                        new VertexPositionColor(new Vector3(angle1 - addDiagonal, 0), _color),
                        new VertexPositionColor(new Vector3(angle2 + addDiagonal, 0), _color),
                        new VertexPositionColor(new Vector3(angle2 - addDiagonal, 0), _color),
                        new VertexPositionColor(new Vector3(_end + addVertical, 0), _color),
                        new VertexPositionColor(new Vector3(_end - addVertical, 0), _color),
                    });
                }
                
            }
        }
    }
}