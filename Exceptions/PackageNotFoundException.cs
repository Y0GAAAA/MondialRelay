using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MondialRelay.Exceptions
{
    public class PackageNotFoundException : Exception
    {
        public PackageNotFoundException() : base("no package could be found with the specified tracking id and postal code") {}
    }
}
