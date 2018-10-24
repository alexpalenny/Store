using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SeaStore.Entities
{
  public class ApplicationRole : IdentityRole
  {
    public ApplicationRole()
    {
    }

    public ApplicationRole(string roleName) : base(roleName)
    {
    }

    public ApplicationRole(string roleName, string description) : base(roleName)
    {
      Description = description;
    }
    
    public string Description { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    
    public virtual ICollection<IdentityUserRole<string>> Users { get; set; }    
    public virtual ICollection<IdentityRoleClaim<string>> Claims { get; set; }
  }
}
