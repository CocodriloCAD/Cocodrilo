# Cocodrilo

## Build & Install

Requires the [.NET SDK](https://dotnet.microsoft.com/download) (8.0+) and Rhino 8.

### Build

```bash
cd Cocodrilo/Cocodrilo
dotnet build -f net7.0      # Mac
dotnet build -f net48       # Windows
```

Output:
- `Cocodrilo/Cocodrilo/bin/Debug/<net7.0|net48>/Cocodrilo.rhp`
- `Cocodrilo/Cocodrilo_GH/bin/Debug/<net7.0|net48>/Cocodrilo_GH.gha` (build from `Cocodrilo/Cocodrilo_GH` the same way; it references `Cocodrilo` via project reference and rebuilds it automatically)

### Install (Mac)

1. Create the plugin folder and copy the build output into it:
   ```bash
   TARGET="$HOME/Library/Application Support/McNeel/Rhinoceros/8.0/MacPlugIns/Cocodrilo"
   SRC="Cocodrilo/Cocodrilo/bin/Debug/net7.0"
   mkdir -p "$TARGET"
   cp "$SRC/Cocodrilo.rhp" "$SRC/Newtonsoft.Json.dll" "$SRC/System.Resources.Extensions.dll" "$TARGET/"
   ```
2. Relaunch Rhino. Verify with **Window → Panels** (look for "Cocodrilo") or by running the `Cocodrilo_OpenPanel` command.

### Install (Windows)

Drag `Cocodrilo.rhp` onto a running Rhino window, or **Options → Plug-ins → Install...** and select it.

## Benchmarks

Please run benchmarks before commiting:

Rhino Command Line: `RunPythonScript -> ../Cocodrilo/Benchmarks/run_benchmarks.py`

## Reference

Within https://doi.org/10.1007/s00366-022-01732-4 is given an outline of the presented plugin.

Please refer to Cocodrilo as following:

```
@misc{Cocodrilo,
  author = {Tobias Teschemacher and Anna Maria Bauer and Ricky Aristio and Manuel Messmer and Roland Wüchner and Kai-Uwe Bletzinger},
  title = {Cocodrilo},
  howpublished = {\url{https://github.com/CocodriloCAD/Cocodrilo}},
}
@article{Teschemacher.2022,
	author = {Tobias Teschemacher and Anna M. Bauer and Ricky Aristio and Manuel Messmer and Roland Wüchner and Kai-Uwe Bletzinger},
	title = {Concepts of data collection for the CAD-integrated isogeometric analysis},
	journal = {Engineering with Computers},
	pages = {1435-5663},
	doi = {https://doi.org/10.1007/s00366-022-01732-4},
	year = {2022}
}
```
