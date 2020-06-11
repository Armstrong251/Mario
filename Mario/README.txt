NES Mario font is included in Content folder
To install font: Right click, "install for all users". Normal install does not work.

Ignored warnings:

CA1014: Mark assemblies with CLSCompliant. Ignored because we were told to.
CA2213: IDisposable errors. Ignored because those variable are part of a default Monogame project.
CA1814: Ignored because we have a valid reason to use multidimensional arrays
CA2211 in Bounds.cs: Ignored because of the way it's initialized.
All warnings about calling a virtual method from a superclass: Yes - that's all intended
CA2225: Ignored because, as the Visual Studio Docs say, "Applications can ignore a warning from this rule."
CA1502: Ignored on some methods because they either can't have reduced complexity or the analyzer's complexity
measurement is inaccurate.
CA1811: Ignored because the docs say "It is safe to suppress a warning from this rule." Plus, actually resolving them
doesn't get rid of the warning on some anyway.