using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyGunsRecoded
{
    public class Config : Qurre.API.Addons.IConfig
    {
        public string Name { get; set; } = "FunnyGuns";

        // Depricated because NotInfiniteAmmo is not implemented
        // public bool NotInfiniteAmmoMutator { get; set; } = true;
    }
}
