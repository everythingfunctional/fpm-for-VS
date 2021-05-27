using System.ComponentModel;

namespace fpm_for_VS.Options
{
    internal class TestOptions : BaseOptionModel<TestOptions>
    {
        [Category("Execution")]
        [DisplayName("Target")]
        [Description("What should be run?")]
        [DefaultValue(null)]
        public string target { get; set; }

        [Category("Execution")]
        [DisplayName("Executable Arguments")]
        [Description("Any arguments that should be passed to the executable")]
        [DefaultValue(null)]
        public string extraArgs { get; set; }

    }
}