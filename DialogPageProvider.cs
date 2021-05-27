namespace fpm_for_VS.Options
{
    /// <summary>
    /// A provider for custom <see cref="DialogPage" /> implementations.
    /// </summary>
    internal class DialogPageProvider
    {
        public class General : BaseOptionPage<GeneralOptions> { }
        public class Run : BaseOptionPage<RunOptions> { }
        public class Test : BaseOptionPage<TestOptions> { }
    }
}