using System;

namespace Equinor.ProCoSys.PCS5.WebApi.Misc;

public class InValidProjectException : Exception
{
    public InValidProjectException(string error) : base(error)
    {
    }
}