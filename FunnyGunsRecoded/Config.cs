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
    }
}
