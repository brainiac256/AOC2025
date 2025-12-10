using System.Numerics;
using AOC2025.Engine;

namespace AOC2025.Puzzles.DayOne;

internal class RootClip : Clip
{
    /// <summary>
    /// Virtual resolution
    /// </summary>
    public static Size vres = new Size(1600, 960);

    private DialClip dial;

    public RootClip() : base()
    {
        size = new Size(RootClip.vres.w, RootClip.vres.h);
        
        dial = new DialClip();
        dial.Position = new Vector2(0,0);
        Add(dial);
        
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
        base.Update(uc);
    }

    public override void SetState(string NewStateData)
    {
        List<string> PuzzleData = NewStateData.Split("\n").Select(s => s.Trim()).ToList();
        dial.SetPuzzleData(PuzzleData);
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