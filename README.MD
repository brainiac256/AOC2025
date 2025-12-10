[![Deploy to GitHub Pages](https://github.com/brainiac256/AOC2025/actions/workflows/main.yml/badge.svg)](https://brainiac256.github.io/AOC2025/index.html)

# My Advent of Code 2025 workspace

## This Year's Code

I'm not terribly concerned about rushing to complete an original solution or get on the leaderboard. I like to use Advent of Code to explore a new technology. Last year I tried to run everything as a console app and render algorithm visualizations in the terminal, which worked... not great. This year I'm going to try to use Blazor WASM to render algorithm visualizations in the web browser, which I expect will be even more difficult. Yay!

### Blazor WASM

I've been using Blazor SSR at my day job for a couple years now, to make boring line-of-business apps with <20 daily active users. Development goes pretty quick because I know C# pretty well. I generally avoid the WASM/hybrid/auto interactivity modes because the additional complexity seems like too much hassle for not much gain. So I'm jumping in full depth by making this a WASM standalone app that will hopefully deploy on Github Pages to showcase my work! Also, I usually use Visual Studio at work, as we have licenses for that, but I don't do a lot of dev on my home PC, so I'm using only VS Code and the free C# kit to make this.

### Visualization

After doing practically no research at all I settled on using nkast.Wasm as a Canvas wrapper. The last time I wrote any sort of graphics code or anything at all like this was when I wrote a toy Asteroids clone in C++ with Allegro back in like 2012, so wish me luck!

## Where to view

Whenever I finish a puzzle I'll push new code which should update the GitHub Pages deployment automatically! I had to disable trimming in order to get the Canvas integration to work. [Click here!](https://brainiac256.github.io/AOC2025/index.html)