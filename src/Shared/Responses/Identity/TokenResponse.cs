using System;

namespace CleanArchTemplate.Shared.Responses.Identity
{
    public class TokenResponse
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
        public string? UserImageURL { get; set; }
        public required DateTimeOffset RefreshTokenExpiryTime { get; set; }
    }
}