# MauiGridClockApp

A tiny application that uses Grid, a MAUI layout element, to represent time.

## What I learnt from the project

- How to create a new MAUI project in Visual Studio
- File structure of a MAUI project
- How to do data binding and basic MVVM design
- How to update UI with timer
- How to add elements to a grid layout dynamically
- How to debug on an physical Android device
- How to change Splash screen and app icon
- How to configure default screen orientation on Android
- How to get screen (window) size


### UI update by timer

We can use Timer (System.Timers) to repeatedly invoke some simple processing. 
But I had faced the probrem in execution about threading-related error. I found that Dispatcher (System.Windows.Threading) solves this probrem like as following.

```csharp
// Create timer
timer = Dispatcher.CreateTimer();
timer.Interval = TimeSpan.FromSeconds(updateSpan);
timer.Tick += (s, e) => ClockUpdateCallback(s, e, vm);
timer.Start();
```

### (Android) Screen orientation setting

In MainActivity.cs under the Platforms/Android directory, Add 

```csharp
e.g.) ScreenOrientation = ScreenOrientation.Landscape
```

as a Activity option. 


### Callback event of screen size allocation

For size initialization of visual elements, we can utilize OnSizeAllocated callback in the code-behaind script of Pages. 

```csharp
protected override void OnSizeAllocated(double width, double height)
{
    base.OnSizeAllocated(width, height);

    ...
}
```


## Reference

[MAUI official documentation](https://learn.microsoft.com/en-us/dotnet/maui/?view=net-maui-7.0)

[MVVM community toolkits documentation](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/)