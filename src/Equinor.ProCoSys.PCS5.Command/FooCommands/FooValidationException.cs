using System;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands;

public class FooValidationException : Exception
{
    public FooValidationException(string error) : base(error)
    {
    }
}