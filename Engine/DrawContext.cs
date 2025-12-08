using System;
using nkast.Wasm.Canvas;

namespace AOC2025.Engine
{
    public class DrawContext
    {
        public required ICanvasRenderingContext CanvasContext;
        public int Layer;
        public TimeSpan t, dt;

        public DrawContext()
        {
        }
    }
}