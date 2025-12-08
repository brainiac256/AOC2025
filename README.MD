# My Advent of Code 2025 workspace

## This Year's Code

I'm not terribly concerned about rushing to complete an original solution or get on the leaderboard. I like to use Advent of Code to explore a new technology. Last year I tried to run everything as a console app and render algorithm visualizations in the terminal, which worked... not great. This year I'm going to try to use Blazor WASM to render algorithm visualizations in the web browser, which I expect will be even more difficult. Yay!

### Blazor WASM

I've been using Blazor SSR at my day job for a couple years now, to make boring line-of-business apps with <20 daily active users. Development goes pretty quick because I know C# pretty well. I generally avoid the WASM/hybrid/auto interactivity modes because the additional complexity seems like too much hassle for not much gain. So I'm hopping in full by making this a WASM standalone app that will hopefully deploy on Github Pages to showcase my work!

### Visualization

After doing practically no research at all I settled on using nkast.Wasm as a Canvas wrapper. The last time I wrote any sort of graphics code or anything at all like this was when I wrote a toy Asteroids clone in C++ with Allegro back in like 2012, so wish me luck!