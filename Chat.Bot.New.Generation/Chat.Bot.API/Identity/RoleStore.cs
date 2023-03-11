using Chat.Bot.API.Models;
using Infra.CrossCutting.Log.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Chat.Bot.API.Identity
{
    public class RoleStore : IRoleStore<IdentityRole>
    {
        private readonly List<IdentityRole> _roles;
        private readonly ILoggerAdapter<RoleStore> _logger;

        public RoleStore(ILoggerAdapter<RoleStore> logger)
        {
            _logger= logger;
            _roles = new List<IdentityRole> { 
                new IdentityRole
                {
                    Id = "0",
                    NormalizedName= "Bot",
                    Name= "Bot"
                },
                new IdentityRole
                {
                    Id = "1",
                    NormalizedName= "User",
                    Name= "User"
                }
            };

            _logger.LogInformation("Loaded identity");
        }

        public Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            _roles.Add(role);

            return Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            var match = _roles.FirstOrDefault(r => r.Id == role.Id);
            if (match != null)
            {
                _roles.Remove(match);

                return IdentityResult.Success;
            }
            else
            {
                return IdentityResult.Failed();
            }
        }

        public void Dispose()
        {
        }

        public async Task<IdentityRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var role = _roles.FirstOrDefault(r => r.Id == roleId);

            return role;
        }

        public async Task<IdentityRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var role = _roles.FirstOrDefault(r => String.Equals(r.NormalizedName, normalizedRoleName, StringComparison.OrdinalIgnoreCase));

            return role;
        }

        public Task<string?> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string?> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string? normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;

            return Task.FromResult(true);
        }

        public Task SetRoleNameAsync(IdentityRole role, string? roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;

            return Task.FromResult(true);
        }

        public async Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            var match = _roles.FirstOrDefault(r => r.Id == role.Id);
            if (match != null)
            {
                match.Name = role.Name;

                return IdentityResult.Success;
            }
            else
            {
                return IdentityResult.Failed();
            }
        }
    }
}
