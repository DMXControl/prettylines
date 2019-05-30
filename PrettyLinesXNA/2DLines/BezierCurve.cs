using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrettyLinesLib
{
    public class BezierCurve : IThick2DLine
    {
        private readonly BasicEffect effect;
        private readonly GraphicsDevice device;
        private VertexBuffer buffer;

        private Vector2 start;
        private Vector2 end;
        private Color color;
        private int segments;
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

        public int Segments
        {
            get { return segments; }
            set
            {
                segments = value;
                ChangeResolution();
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

        public BezierCurve(BasicEffect effect, GraphicsDevice device, Vector2 start, Vector2 end, Color color,
            int segments, float thickness)
        {
            this.effect = effect;
            this.device = device;
            this.start = start;
            this.end = end;
            this.color = color;
            this.segments = segments;
            this.thickness = thickness;
            buffer = new VertexBuffer(device, typeof(VertexPositionColor), segments * 2 + 2, BufferUsage.WriteOnly);
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
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, segments * 2);
            }

            device.SetVertexBuffer(null);
            device.RasterizerState = temp;
        }

        private void ChangeResolution()
        {
            buffer = new VertexBuffer(device, typeof(VertexPositionColor), segments * 2 + 2, BufferUsage.WriteOnly);
            UpdateBuffer();
        }

        private void UpdateBuffer()
        {
            List<VertexPositionColor> vertex = new List<VertexPositionColor>(segments * 2 + 2);

            Vector2 toAdd = new Vector2(0, thickness * -0.5f);

            Vector2 last = Vector2.Zero;

            for (int i = 0; i < segments + 1; i++)
            {
                float bezierTime = 1.0f / segments * i;
                Vector2 t = CalculateBezierPoint(bezierTime, start, end);

                if (i > 0 && i < segments)
                {
                    t = Vector2.SmoothStep(last, t, 0.5f);
                }

                vertex.Add(new VertexPositionColor(new Vector3(t + toAdd, 0), color));
                vertex.Add(new VertexPositionColor(new Vector3(t - toAdd, 0), color));
                last = t;
            }

            buffer.SetData(vertex.ToArray());
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