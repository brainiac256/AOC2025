using System.Diagnostics;
using System.Numerics;
using Microsoft.JSInterop;
using nkast.Wasm.Dom;
using nkast.Wasm.Canvas;
using AOC2025.Engine;
using AOC2025.Puzzles.Boids;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace AOC2025.Layout;

public partial class SimContainer<T> : ComponentBase
    where T : Clip, new()
{
    Stopwatch _sw = new Stopwatch();
    TimeSpan _prevt;

    private T? SimulationRootClip;
    private ElementReference canvas;
    private ElementPosition? ep;
    [Inject] public IJSRuntime JsRuntime {get; set;} = default!;
    [Parameter] public string? InitialData { get; set; }

    // Summary:
    //     Method invoked when the component is ready to start, having received its initial
    //     parameters from its parent in the render tree.
    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        base.OnAfterRender(firstRender);
        if (firstRender)
        {
            // create new simulation
            Init();
            ep = await JsRuntime.InvokeAsync<ElementPosition>("getElementAbsolutePosition", canvas);
            // hook up canvas rendering ticks to our TickDotNet invokable
            await JsRuntime.InvokeAsync<object>("initRenderJS", DotNetObjectReference.Create(this));
        }
    }

    private void Init()
    {
        SimulationRootClip = new();
        SimulationRootClip.SetState(InitialData ?? "");
    }

    public async Task Reset()
    {
        Init();
        ep = await JsRuntime.InvokeAsync<ElementPosition>("getElementAbsolutePosition", canvas);
    }

    Canvas? cs;
    ICanvasRenderingContext? cx;

    MouseState currMouseState;
    MouseState prevMouseState;
    TouchState currTouchState;
    TouchState prevTouchState;

    [JSInvokable]
    public async Task TickDotNet()
    {
        if(SimulationRootClip is null) return;
        if (cs == null)
        {
            cs = Window.Current.Document.GetElementById<Canvas>("theCanvas");
            ContextAttributes attribs = new ContextAttributes();
            attribs.Alpha = true;
            attribs.Desynchronized = null;
            cx = cs.GetContext<ICanvasRenderingContext>(attribs);

            Window.Current.OnResize += this.OnResize;
            Window.Current.OnMouseMove += this.OnMouseMove;
            Window.Current.OnMouseDown += this.OnMouseDown;
            Window.Current.OnMouseUp += this.OnMouseUp;
            Window.Current.OnMouseWheel += this.OnMouseWheel;

            Window.Current.OnTouchStart += this.OnTouchStart;
            Window.Current.OnTouchMove += this.OnTouchMove;
            Window.Current.OnTouchEnd += this.OnTouchEnd;

            _sw.Start();
            _prevt = _sw.Elapsed;
        }
        
        // run gameloop tick

        TimeSpan t  = _sw.Elapsed;
        TimeSpan dt = t - _prevt;
        _prevt = t;
        // reset canvas
        
        cx!.SetTransform(1, 0, 0, 1, (float)-ep!.Top, (float)-ep.Left);
        cx.Translate((float)ep.Top,(float)ep.Left);
        cx.ClearRect(0, 0, cs.Width, cs.Height);

        // scale to virtual resolution
        float bbscalew = cs.Width / RootClip.vres.w;
        float bbscaleh = cs.Height / RootClip.vres.h;
        cx.Scale(bbscalew, bbscaleh);

        UpdateContext uc = new UpdateContext(
            t, dt,
            currMouseState, prevMouseState,
            currTouchState, prevTouchState
            );
        prevMouseState = currMouseState;
        prevTouchState = currTouchState;
        uc.tx = uc.tx * Matrix4x4.CreateScale(bbscalew, bbscaleh, 1)
          * Matrix4x4.CreateTranslation((float)ep.Left,(float)ep.Top,0);

        SimulationRootClip.Update(uc);

        DrawContext dc = new DrawContext()
        {
            CanvasContext = cx,
            Layer = 0,
            t  = t,
            dt = dt
        };
        
        for (int l = 0; l < 3; l++)
        {
            dc.Layer = l;
            SimulationRootClip.Draw(dc);
        }

    }

    private async void OnResize(object sender)
    {
        ep = await JsRuntime.InvokeAsync<ElementPosition>("getElementAbsolutePosition", canvas);
    }

    private void OnMouseMove(object sender, int x, int y)
    {
        currMouseState.Position = new Vector2(x, y);
    }

    private void OnMouseDown(object sender, int x, int y, int buttons)
    {
        currMouseState.Position = new Vector2(x, y);
        currMouseState.LeftButton = (buttons & 1) != 0;
    }

    private void OnMouseUp(object sender, int x, int y, int buttons)
    {
        currMouseState.Position = new Vector2(x, y);
        currMouseState.LeftButton = (buttons & 1) != 0;
    }

    public void OnMouseWheel(object sender, int deltaX, int deltaY, int deltaZ, int deltaMode)
    {
        currMouseState.Wheel += (float)deltaY;
    }

    private void OnTouchStart(object sender, float x, float y, int identifier)
    {
        currTouchState.Position.X = x;
        currTouchState.Position.Y = y;
        currTouchState.IsPressed = true;
        prevTouchState = currTouchState;
    }

    private void OnTouchMove(object sender, float x, float y, int identifier)
    {
        currTouchState.Position.X = x;
        currTouchState.Position.Y = y;
    }

    private void OnTouchEnd(object sender, float x, float y, int identifier)
    {
        currTouchState.Position.X = x;
        currTouchState.Position.Y = y;
        currTouchState.IsPressed = false;
    }


    public void Dispose()
    {
        SimulationRootClip?.Dispose();
        SimulationRootClip = null;

        cx?.Dispose();
        cx = null;
    }
    
}
public class ElementPosition
{
    public double Top {get; set; }
    public double Left {get; set;}
    public double Right {get; set; }
    public double Bottom{get; set;}
    public double Width {get; set;}
    public double Height {get; set; }
}