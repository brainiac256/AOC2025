using System;
using System.Numerics;
using System.Diagnostics;
using nkast.Aether.Physics2D.Collision;
using nkast.Wasm.Canvas;
using AOC2025.Engine;
using aeVector2 = nkast.Aether.Physics2D.Common.Vector2;
using AOC2025.Extensions;

namespace AOC2025.Puzzles
{
    public partial class FpsClip: Clip
    {
        Stopwatch _sw = new Stopwatch();
        private RingBufferFloat frametimes;

        public FpsClip() : base()
        {
            _sw.Start();
            frametimes = new RingBufferFloat(30);
        }
        
        public override void Draw(DrawContext dc)
        {
            var cs = dc.CanvasContext;

            if (dc.Layer == 2)
            {
                long ms = _sw.ElapsedMilliseconds;
                _sw.Restart();
                frametimes.Write(ms);

                // draw fps
                cs.Save();
                cs.FillStyle = "#603090";
                cs.Font = $"24px bold Verdana";
                cs.TextAlign = TextAlign.Left;
                cs.TextBaseline = TextBaseline.Middle;
                cs.FillText($"Avg frame time: {ms} ms", 50, 50);
                cs.Restore();                
            }

            base.Draw(dc);
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
