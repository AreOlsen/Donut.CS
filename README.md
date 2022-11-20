# Documentation behind the Donut C# Code.

This is a project based on the "Donut C" code by Andy Sloane, and the "Why a Spinning Donut is Pure Math" article by Marrten De Baecke.

![Spinning Donut gif](./spinning.gif)

## The mathematics behind it all.

To understand how this algorithm works and why it works it is important to understand the step by step process, and how mathematics achieves our desired goal.
The following diagram shows the step by step process over the algoritm.

### Diagram over the process

```mermaid
  graph TD;
    A(Create a 2d circle.) --> B(Extend 2d circle to 3d taurus.);
    B --> C(Rotate 3d taurus around Z and X axes.);
    C --> D(Display 3d taurus onto camera plane.);
    D --> E(Convert pixels to Ascii, render to console.);
```

### The mathematics: Step by Step.

#### Rendering a 2D circle.

TODO:
