# Fortran Package Manager for Visual Studio (fpm for VS)

This extension adds options and menu entries for building, running and testing your Fortran project using the [Fortran Package Manager (fpm)](https://github.com/fortran-lang/fpm).

## Usage

In the Tools->Options dialog, find the entry "fpm Options", and specify the appropriate values for the various options.
Note that leaving an option blank is valid, as it means the extension will simply not include that in the options passed to fpm, and thus fpm will simply use the default.
Use one of the entries in the Extensions->fpm menu to execute `fpm build`, `fpm run` or `fpm test`, using the options specified in the previous step.

## Contributing

For background material and getting started, I found [this video](https://channel9.msdn.com/Events/Build/2016/B886) to be a great start, and of course [the official documentation](https://docs.microsoft.com/en-us/visualstudio/extensibility/starting-to-develop-visual-studio-extensions?view=vs-2019).
Please [open an issue](https://github.com/everythingfunctional/fpm-for-VS/issues/new/choose) to report a bug, or request a feature so that it can be discussed before submitting a [pull request](https://github.com/everythingfunctional/fpm-for-VS/compare).
