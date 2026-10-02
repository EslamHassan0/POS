using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.POSSettings;

public class UpdatePOSSetting : CreatePOSSetting
{
    public bool RemoveLogo { get; set; } 
}