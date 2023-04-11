using CleanArchTemplate.Shared.Responses.Identity;

using System.Collections.Generic;

namespace CleanArchTemplate.Shared.Requests.Identity
{
    public class UpdateUserRolesRequest
    {
        public string UserId { get; set; }
        public IList<UserRoleModel> UserRoles { get; set; }
    }
}