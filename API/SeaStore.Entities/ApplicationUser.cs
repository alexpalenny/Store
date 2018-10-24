using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SeaStore.Entities
{
  public class ApplicationUser : IdentityUser
  {
    public virtual string FriendlyName { get { return string.IsNullOrWhiteSpace(FullName) ? UserName : FullName; } }

    public string JobTitle { get; set; }
    public string FullName { get; set; }
    public string Configuration { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsLockedOut => LockoutEnabled && LockoutEnd >= DateTimeOffset.UtcNow;

    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    [NotMapped]
    public bool IsUserFirstLogin { get; set; }

    public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }
    public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
  }
}
