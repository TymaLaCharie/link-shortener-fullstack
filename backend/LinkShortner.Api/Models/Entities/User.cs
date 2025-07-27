using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkShortner.Api.Models.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime DateRegistered { get; set; }
    public virtual ICollection<Url> Urls { get; set; }
}
