using System.Collections.Generic;

namespace CleanArchTemplate.Shared.Responses.Identity
{
    public class GetAllUsersResponse
    {
        public IEnumerable<UserResponse> Users { get; set; }
    }
}