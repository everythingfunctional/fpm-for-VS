using System.ComponentModel;

namespace fpm_for_VS.Options
{
    internal class GeneralOptions : BaseOptionModel<GeneralOptions>
    {
        /// (c) 2021 Sourcery, Inc.
        /// This software was developed for the U.S.Nuclear Regulatory Commission(US NRC) under contract # 31310020D0006:
        /// "Technical Assistance in Support of NRC Nuclear Regulatory Research for Materials, Waste, and Reactor Programs"

        [Category("Compilation")]
        [DisplayName("Pre-execution Script")]
        [Description("Anything that should be run before fpm?")]
        [DefaultValue("")]
        public string preExecScript { get; set; }

        [Category("Compilation")]
        [DisplayName("Compiler")]
        [Description("What compiler should fpm use to compile the code?")]
        [DefaultValue("")]
        public string compiler { get; set; }

        [Category("Compilation")]
        [DisplayName("Profile")]
        [Description("What compilation profile (default flags) should fpm use to compile the code?")]
        [DefaultValue("")]
        public string profile { get; set; }

        [Category("Compilation")]
        [DisplayName("Flags")]
        [Description("What (additional) flags should fpm use to compile the code?")]
        [DefaultValue("")]
        public string flags { get; set; }
    }
}