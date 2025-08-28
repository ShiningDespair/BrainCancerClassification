using System;
using System.Collections.Generic;

namespace BrainCancerClassification.Models;

public partial class User
{
    public int Id { get; set; }

    public string ClerkId { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? ProfileImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public bool IsActive { get; set; }

    public string? PhoneNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public int RoleId { get; set; }

    public bool IsEmailVerified { get; set; }

    public string PreferredLanguage { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? DeletedByNavigation { get; set; }

    public virtual ICollection<User> InverseCreatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseDeletedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseUpdatedByNavigation { get; set; } = new List<User>();

    public virtual Role Role { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual ICollection<Upload> Uploads { get; set; } = new List<Upload>();
}
