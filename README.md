# Fortran Package Manager for Visual Studio (fpm for VS)

[![Build status](https://ci.appveyor.com/api/projects/status/a6qv09a5tgl4l6na/branch/main?svg=true)](https://ci.appveyor.com/project/everythingfunctional/fpm-for-vs/branch/main)

Download this extension from the [VS Gallery](https://marketplace.visualstudio.com/items?itemName=SourceryInstitute.fpm-for-VS)
or get the [CI build](https://www.vsixgallery.com/extension/fpm_for_VS.7add2675-0a9c-493f-9886-9d674f711c42).

------------

This extension adds options and menu entries for building, running and testing your Fortran project using the [Fortran Package Manager (fpm)](https://github.com/fortran-lang/fpm).

## Usage

In the Tools->Options dialog, find the entry "fpm Options", and specify the appropriate values for the various options.
Note that leaving an option blank is valid, as it means the extension will simply not include that in the options passed to fpm, and thus fpm will simply use the default.
Use one of the entries in the Extensions->fpm menu to execute `fpm build`, `fpm run` or `fpm test`, using the options specified in the previous step.

## Contributing

For background material and getting started, I found [this video](https://channel9.msdn.com/Events/Build/2016/B886) to be a great start, and of course [the official documentation](https://docs.microsoft.com/en-us/visualstudio/extensibility/starting-to-develop-visual-studio-extensions?view=vs-2019).
Please [open an issue](https://github.com/everythingfunctional/fpm-for-VS/issues/new/choose) to report a bug, or request a feature so that it can be discussed before submitting a [pull request](https://github.com/everythingfunctional/fpm-for-VS/compare).

## Acknowledgements

We would like to acknowledge the Nuclear Regulatory Commission (NRC) for providing the initial funding to develop this extension and for allowing us to make it open-source.
