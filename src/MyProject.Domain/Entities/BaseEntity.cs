using NUlid;
using System;

namespace MyProject.Domain.Entities;

public abstract class BaseEntity
{
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsActive { get; set; } = true;
}