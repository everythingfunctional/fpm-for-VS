using System.ComponentModel;

namespace fpm_for_VS.Options
{
    internal class GeneralOptions : BaseOptionModel<GeneralOptions>
    {
        [Category("Compilation")]
        [DisplayName("Compiler")]
        [Description("What compiler should fpm use to compile the code?")]
        [DefaultValue(null)]
        public string compiler { get; set; }

        [Category("Compilation")]
        [DisplayName("Profile")]
        [Description("What compilation profile (default flags) should fpm use to compile the code?")]
        [DefaultValue(null)]
        public string profile { get; set; }

        [Category("Compilation")]
        [DisplayName("Flags")]
        [Description("What (additional) flags should fpm use to compile the code?")]
        [DefaultValue(null)]
        public string flags { get; set; }
    }
}