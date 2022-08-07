using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTest.Infrastructure.Identity.Entities;

public class ApplicationRole : IdentityRole<int>
{
  public DateTime? CreatedDate { get; set; }
}
