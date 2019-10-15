# Drexel.Arguments
## WARNING - CAVEAT EMPTOR
This package is in a prerelease state. Existing functionality may not work as intended, and expected functionality may
be missing. Breaking changes are likely to be frequent and wide-sweeping. Basically, if you're not me, don't rely on
this package.

## Motivation
Parsing command-line arguments (also known as "options") is a common task. Existing libraries do not offer advanced
functionality such as:
* Multi-value arguments (ex. `/first value1 value2 /second value3 value4`)
* Choice from simple (DOS, POSIX, etc.) to advanced (GNU, MSBuild, etc.) formatting
* Argument dependency trees (ex. `/password P00rs3cur1typr@ctices!` depends on `/username DOMAIN\Max`)

## Use
* `ArgumentParser` will parse arguments according to a specified style.
* `ArgumentComposer` consumes an `ArgumentParser`, and exposes the ability to "compose" objects (similar to MEF).
  + TODO: Composer doesn't actually compose anything

## TODO:
* Implement `GnuParser`, `UnityParser`, and `WindowsParser`
* Implement `ArgumentComposer` actually composing objects
  + Need some sort of `ArgumentAttribute`, but because `Argument` is an abstract base class instead of an interface,
  will need to make some sort of method like `ArgumentAttribute.Build()` to return an `Argument` instance proper
* Implement support for help/version arguments that, when present, suppress `ArgumentParser` failures and somehow
report back to the caller that the help/version information was requested.
* Implement dependency trees
  + Could either copy-paste the tree logic from the old `CommandLineArguments` library, use `Drexel.Collections`, or
  re-write it from scratch (probably most expedient to just steal the old version)