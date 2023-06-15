using System;

namespace Equinor.ProCoSys.PCS5.Command;

public class ValidationException : Exception
{
    public ValidationException(string error) : base(error)
    {
    }
}
