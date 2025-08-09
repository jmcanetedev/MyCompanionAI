using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace MyCompanionAI.Data.Models;

public class ApplicationRole : IdentityRole
{
    [JsonIgnore]
    public ICollection<ApplicationUser> Users { get; set; }
}
