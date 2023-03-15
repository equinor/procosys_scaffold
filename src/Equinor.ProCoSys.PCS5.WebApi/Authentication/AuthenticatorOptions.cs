using System;
using System.Collections.Generic;
using Equinor.ProCoSys.Auth.Authentication;
using Microsoft.Extensions.Options;

namespace Equinor.ProCoSys.PCS5.WebApi.Authentication
{
    /// <summary>
    /// "Mapping" between application options read by IOptionsMonitor to generic IAuthenticatorOptions
    /// Needed because keys for configuration differ from application to application
    /// </summary>
    public class AuthenticatorOptions : IAuthenticatorOptions
    {
        protected readonly IOptionsMonitor<PCS5AuthenticatorOptions> _options;

        private readonly IDictionary<string, string> _scopes = new Dictionary<string, string>();
        
        public AuthenticatorOptions(IOptionsMonitor<PCS5AuthenticatorOptions> options)
        {
            _options = options;
            _scopes.Add(MainApiAuthenticator.MainApiScopeKey, _options.CurrentValue.MainApiScope);
        }

        public string Instance => _options.CurrentValue.Instance;

        public string ClientId => _options.CurrentValue.PCS5ApiClientId;

        public string Secret => _options.CurrentValue.PCS5ApiSecret;

        public Guid ObjectId => _options.CurrentValue.PCS5ApiObjectId;

        public bool DisableRestrictionRoleUserDataClaims
            => _options.CurrentValue.DisableRestrictionRoleUserDataClaims;

        public bool DisableProjectUserDataClaims
            => _options.CurrentValue.DisableProjectUserDataClaims;

        public IDictionary<string, string> Scopes => _scopes;
    }
}
