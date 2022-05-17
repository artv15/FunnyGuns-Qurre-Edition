using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyGunsRecoded
{
    public class Config : Qurre.API.Addons.IConfig
    {

        public string Name { get; set; } = "FunnyGuns";

        [Description("Should plugin automatically update itself?")]
        public bool Autoupdates { get; set; } = true;

        [Description("Avaliable locales: ru, en")]
        public string Locale { get; set; } = "ru";

        [Description("Used for debug build updating. Do not edit it unless you know what are you doing!")]
        public string APIKey { get; set; } = "test";
    }
}
