using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Dashboards;

public class GetDashboardInput
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}
