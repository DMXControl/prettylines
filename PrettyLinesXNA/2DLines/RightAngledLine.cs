﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrettyLinesLib
{
    public class RightAngledLine : Thick2DLine
    {
        private Vector2 start;
        private Vector2 end;
        private Color color;
        private float thickness;

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

        public override float Thickness
        {
            get { return thickness; }
            set
            {
                thickness = value;
                UpdateBuffer();
            }
        }

        public RightAngledLine(GraphicsDevice device, BasicEffect effect, Vector2 start, Vector2 end, Color color,
            float thickness, string label = "")
        {
            this.device = device;
            this.effect = effect;
            this.start = start;
            this.end = end;
            this.color = color;
            this.thickness = thickness;
            this.Label = label;
            buffer = new VertexBuffer(device, typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
            UpdateBuffer();
        }

        public override void Draw(Matrix transformationMatrix)
        {
            device.SetVertexBuffer(buffer);

            var temp = device.RasterizerState;

            var state = new RasterizerState
            {
                MultiSampleAntiAlias = true,
                CullMode = CullMode.None
            };


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