using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.Domain.Events
{
    public interface IHaveDisplayName : IPostSaveDomainEvent
    {
        string Name { get; }
    }
}
