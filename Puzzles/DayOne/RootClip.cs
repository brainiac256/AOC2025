using System.Numerics;
using AOC2025.Engine;

namespace AOC2025.Puzzles.DayOne;

internal class RootClip : Clip
{
    /// <summary>
    /// Virtual resolution
    /// </summary>
    public static Size vres = new Size(1600, 960);

    public RootClip() : base()
    {
        size = new Size(RootClip.vres.w, RootClip.vres.h);
        FpsClip fps = new FpsClip();
        fps.Position = new Vector2(10,20);
        Add(fps);

        FrameClip frame = new FrameClip();
        frame.Position = new Vector2(0,0);
        frame.Scale = 2;
        Add(frame);
    }

    public override void Update(UpdateContext uc)
    {
        float dt = (float)uc.dt.TotalSeconds;

        base.Update(uc);
    }


    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // dispose inner clips if necessary
        }

        base.Dispose(disposing); 
    }
}