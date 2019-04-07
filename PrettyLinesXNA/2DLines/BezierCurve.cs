using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrettyLinesLib
{
    public class BezierCurve : IThick2DLine
    {
        private readonly BasicEffect _effect;
        private readonly GraphicsDevice _device;
        private VertexBuffer _buffer;

        private Vector2 _start;
        private Vector2 _end;
        private Color _color;
        private int _segments;
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

        public int Segments
        {
            get { return _segments; }
            set
            {
                _segments = value;
                ChangeResolution();
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

        public BezierCurve(BasicEffect effect, GraphicsDevice device, Vector2 start, Vector2 end, Color color,
            int segments, float thickness)
        {
            _effect = effect;
            _device = device;
            _start = start;
            _end = end;
            _color = color;
            _segments = segments;
            _thickness = thickness;
            _buffer = new VertexBuffer(device, typeof(VertexPositionColor), segments * 2 + 2, BufferUsage.WriteOnly);
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
                _device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, _segments * 2);
            }

            _device.SetVertexBuffer(null);
            _device.RasterizerState = temp;
        }

        private void ChangeResolution()
        {
            _buffer = new VertexBuffer(_device, typeof(VertexPositionColor), _segments * 2 + 2, BufferUsage.WriteOnly);
            UpdateBuffer();
        }

        private void UpdateBuffer()
        {
            List<VertexPositionColor> vertex = new List<VertexPositionColor>(_segments * 2 + 2);

            Vector2 toAdd = new Vector2(0, _thickness * -0.5f);

            Vector2 last = Vector2.Zero;

            for (int i = 0; i < _segments + 1; i++)
            {
                float bezierTime = 1.0f / _segments * i;
                Vector2 t = CalculateBezierPoint(bezierTime, _start, _end);

                if (i > 0 && i < _segments)
                {
                    t = Vector2.SmoothStep(last, t, 0.5f);
                }

                vertex.Add(new VertexPositionColor(new Vector3(t + toAdd, 0), _color));
                vertex.Add(new VertexPositionColor(new Vector3(t - toAdd, 0), _color));
                last = t;
            }

            _buffer.SetData(vertex.ToArray());
        }

        private Vector2 CalculateBezierPoint(float t, Vector2 v1, Vector2 v4)
        {
            Vector2 v2;
            Vector2 v3;

            if (v1.X<v4.X)
            {
                v2 = new Vector2((v1.X + v4.X) * 0.5f, v1.Y);
                v3 = new Vector2((v1.X + v4.X) * 0.5f, v4.Y);
            }
            else
            {
                v2 = new Vector2(v1.X, (v1.Y+v4.Y)*0.5f);
                v3 = new Vector2(v4.X, (v1.Y + v4.Y) * 0.5f);
            }
           

            Vector2 output = (1 - t) * (1 - t) * (1 - t) * v1 + 3 * t * (1 - t) * (1 - t) * v2 +
                             3 * t * t * (1 - t) * v3 + t * t * t * v4;

            return output;
        }
    }
}