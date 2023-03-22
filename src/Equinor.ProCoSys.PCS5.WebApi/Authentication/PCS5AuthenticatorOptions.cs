using System;

namespace Equinor.ProCoSys.PCS5.WebApi.Authentication
{
    /// <summary>
    /// Options for Authentication. Read from application configuration via IOptionsMonitor.
    /// "Mapped" to the generic IAuthenticatorOptions
    /// </summary>
    public class PCS5AuthenticatorOptions
    {
        public string? Instance { get; set; }

        public string? PCS5ApiClientId { get; set; }
        public string? PCS5ApiSecret { get; set; }
        public Guid PCS5ApiObjectId { get; set; }

        public bool DisableProjectUserDataClaims { get; set; }
        public bool DisableRestrictionRoleUserDataClaims { get; set; }

        public string? MainApiScope { get; set; }
    }
}
