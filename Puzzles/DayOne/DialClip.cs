using System.Diagnostics;
using nkast.Wasm.Canvas;
using AOC2025.Engine;
using AOC2025.Extensions;
using MudBlazor;

namespace AOC2025.Puzzles;

public partial class DialClip: Clip
{
    List<string> DialingInstructions = new();
    int index;
    int zero_counter;
    bool is_complete;
    int old_dial_position;
    int new_dial_position;
    string instruction = "";
    TimeSpan NextAnimationAt;
    static TimeSpan AnimationDelay = TimeSpan.FromSeconds(0.001);
    bool is_error;
    public DialClip() : base()
    {
        SetPuzzleData([]);
        NextAnimationAt = TimeSpan.FromTicks(0);
    }

    public override void Draw(DrawContext dc)
    {
        //skip if not layer 0
        if(dc.Layer == 0)
        {
            var cs = dc.CanvasContext;
            cs.Save();
            try
            {
                DrawDialFace(cs);
                DrawText(cs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            cs.Restore();
        }
        base.Draw(dc);
    }
    private void DrawDialFace(ICanvasRenderingContext cs)
    {
        cs.Save();
        cs.Translate(0,0);
        cs.BeginPath();
        cs.StrokeStyle = "#111111";
        cs.LineWidth = 8;
        cs.Arc(400,400,150,0,(float)(2*Math.PI),false);
        cs.Stroke();
        cs.ClosePath();
        cs.BeginPath();
        cs.StrokeStyle = "#111111";
        cs.LineWidth = 15;
        cs.Arc(400,400,15,0,(float)(2*Math.PI),false);
        cs.Stroke();
        cs.ClosePath();
        cs.FillStyle = "#555555";
        cs.Font = "bold 48px monospace";
        cs.TextAlign = TextAlign.Center;
        cs.TextBaseline = TextBaseline.Middle;
        cs.FillText("50", 205,400);
        cs.FillText("75", 400,215);
        cs.FillText("00", 595,400);
        cs.FillText("25", 400,585);
        cs.LineWidth = 4;
        for(float n = 0; n <= 95; n+=5)
        {
            double angle = (2*Math.PI)*(n/100d);
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            cs.BeginPath();
            cs.MoveTo((float)(400 + 140 * cos), (float) (400 + 140 * sin));
            cs.LineTo((float)(400+120 * cos), (float)(400+120 * sin));
            cs.Stroke();
            cs.ClosePath();
        }
        bool was_clockwise = instruction.Length > 0 && instruction[0] == 'L';
        double old_angle = (2*Math.PI) * old_dial_position / 100d;
        double dial_angle = (2*Math.PI) * new_dial_position / 100d;
        double dial_cos = Math.Cos(dial_angle);
        double dial_sin = Math.Sin(dial_angle);
        cs.StrokeStyle = "#3333dd";
        cs.BeginPath();
        cs.MoveTo((float)(400 + 110*dial_cos), (float)(400+110*dial_sin));
        cs.LineTo((float)(400 + 20 *dial_cos), (float)(400+20*dial_sin));
        cs.Stroke();
        cs.ClosePath();
        cs.StrokeStyle = "#229999";
        cs.BeginPath();
        cs.Arc(400,400,110, (float)old_angle, (float)dial_angle, was_clockwise);
        cs.Stroke();
        cs.ClosePath();
        cs.Restore();
    }
    private void DrawText(ICanvasRenderingContext cs)
    {
        cs.Save();
        cs.Translate(0,0);
        cs.FillStyle = "#555555";
        cs.Font = "bold 24px monospace";
        cs.TextAlign = TextAlign.Right;
        cs.TextBaseline = TextBaseline.Middle;
        cs.FillText($"Animating #: {index}", 900,45);
        cs.FillText($"Instructions: {DialingInstructions.Count}", 600, 45);
        cs.Font = "bold 48px monospace";
        cs.FillText($"Old dial position: {old_dial_position,5}", 1300, 200);
        cs.FillText($"Instruction: {instruction,3}", 1300,300);
        cs.FillText($"New dial position: {new_dial_position,5}", 1300, 400);
        cs.FillStyle = "#228822";
        cs.FillText($"Password is: {zero_counter,3}", 1300, 500);
        if(is_error) cs.FillText("Error encountered.", 600, 920);
        if(is_complete) cs.FillText("Done", 1300,600);
        cs.Restore();
    }
    public override void Update(UpdateContext uc)
    {
        if(uc.t > NextAnimationAt)
        {
            Advance();
            NextAnimationAt = uc.t.Add(AnimationDelay);
        }
        base.Update(uc);
    }

    private void Advance()
    {
        if(is_error || index >= DialingInstructions.Count || index < 0) return;
        try
        {
            instruction = DialingInstructions[index];
            char direction = instruction[0];
            ReadOnlySpan<char> amount_text = instruction.AsSpan(1);
            bool isValid = int.TryParse(amount_text, out int amt);
            isValid = isValid && (direction == 'L' || direction == 'R');
            if(!isValid)
            {
                is_error = true;
                return;
            }
            old_dial_position = new_dial_position;
            new_dial_position =
                direction == 'R'
                ? new_dial_position + amt
                : new_dial_position - amt;

            new_dial_position %= 100;
            if(new_dial_position < 0) new_dial_position += 100;

            if(new_dial_position == 0) zero_counter += 1;
            index += 1;
            if(index >= DialingInstructions.Count) is_complete = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            is_error = true;
        }
    }

    public override void SetState(string NewStateData)
    {
        base.SetState(NewStateData);
    }

    public void SetPuzzleData(List<string> NewDialingInstructions)
    {
        DialingInstructions = NewDialingInstructions;
        index = 0;
        zero_counter = 0;
        is_complete = false;
        is_error = false;
        old_dial_position = 50;
        new_dial_position = 50;
        instruction = "";
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }
}