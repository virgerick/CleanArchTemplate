using System.Collections.Generic;

namespace CleanArchTemplate.Shared.Responses.Identity
{
    public class GetAllRolesResponse
    {
        public IEnumerable<RoleResponse> Roles { get; set; }
    }
}