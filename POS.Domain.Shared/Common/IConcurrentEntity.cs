using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Shared.Common;

public interface IConcurrentEntity
{
    byte[] RowVersion { get; set; }
}
