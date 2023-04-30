using System;

namespace CleanArchTemplate.Shared.Models
{
    public record User(string FirstName, string LastName, string CreatedBy, DateTime CreatedOn, string LastModifiedBy, DateTime? LastModifiedOn, bool IsDeleted, DateTime? DeletedOn, bool IsActive);
}