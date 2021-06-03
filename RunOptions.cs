using System.ComponentModel;

namespace fpm_for_VS.Options
{
    internal class RunOptions : BaseOptionModel<RunOptions>
    {
        /// (c) 2021 Sourcery, Inc.
        /// This software was developed for the U.S.Nuclear Regulatory Commission(US NRC) under contract # 31310020D0006:
        /// "Technical Assistance in Support of NRC Nuclear Regulatory Research for Materials, Waste, and Reactor Programs"

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

        [Category("Execution")]
        [DisplayName("Example")]
        [Description("Run example executables instead?")]
        [DefaultValue(false)]
        public bool example { get; set; } = false;

    }
}