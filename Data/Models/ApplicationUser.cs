using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MyCompanionAI.Data.Models;

public partial class ApplicationUser : IdentityUser
{
    [JsonIgnore, IgnoreDataMember]
    public override string PasswordHash { get; set; }

    [NotMapped]
    public string Password { get; set; }

    [NotMapped]
    public string ConfirmPassword { get; set; }

    [JsonIgnore, IgnoreDataMember, NotMapped]
    public string Name
    {
        get
        {
            return UserName;
        }
        set
        {
            UserName = value;
        }
    }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
