using System.Numerics;
using AOC2025.Engine;
using nkast.Wasm.Canvas;

namespace AOC2025.Puzzles.Boids
{
    public partial class FrameClip : Clip
    {
        Vector2 mousepos;
        float mouseAnimationOffset;
        public FrameClip() : base()
        {
            mousepos = new Vector2(-1,-1);
            mouseAnimationOffset = 0.0f;
            base.size = new Size(800, 480);
        }
        public override void Draw(DrawContext dc)
        {
            var cs = dc.CanvasContext;
            if(dc.Layer == 1)
            {
                cs.Save();
                cs.Translate(0,0);
                cs.Scale(2,2);
                DrawRect(cs, 0,0,800,480, "#aa3333");
                DrawRect(cs, 50,50,700,380, "#3333aa");
                if(mousepos.X >= 0 && mousepos.X <= 800 && mousepos.Y >= 0 && mousepos.Y <= 480)
                {
                    DrawMouse(cs, mousepos);
                }
                cs.Restore();
            }
            
            base.Draw(dc);
        }

        public override void Update(UpdateContext uc)
        {
            mousepos = uc.ToLocal(
                uc.CurrTouchState.IsPressed ?
                uc.CurrTouchState.Position :
                uc.CurrMouseState.Position) / 2.0f;
            float dt_sec = (float)uc.dt.TotalSeconds;
            mouseAnimationOffset += dt_sec;
            base.Update(uc);
        }
        private void DrawMouse(ICanvasRenderingContext cs, Vector2 center)
        {
            cs.Save();
            cs.Translate(center.X, center.Y);
            cs.LineWidth = 3;
            cs.BeginPath();
            cs.Arc(
                0, 
                0, 
                40, 
                (float) (0.5 * Math.PI + mouseAnimationOffset), 
                (float) ( 2*Math.PI + mouseAnimationOffset),
                false);
            cs.Stroke();
            cs.Restore();
        }
        private void DrawRect(ICanvasRenderingContext cs, float startx, float starty, float width, float height, string color)
        {
            cs.Save();

            cs.BeginPath();
            cs.MoveTo(startx, starty);
            cs.LineWidth = 6.0f;
            cs.StrokeStyle = color;
            cs.GlobalAlpha = 0.4f;
            cs.LineTo(startx + width, starty);
            cs.LineTo(startx + width, starty + height);
            cs.LineTo(startx, starty + height);
            cs.LineTo(startx, starty);
            cs.Stroke();
            cs.Restore();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }

            base.Dispose(disposing);
        }
    }
}