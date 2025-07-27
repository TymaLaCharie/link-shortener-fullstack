using System;
using System.Collections.Generic;

namespace LinkShortner.Api.Models.Entities;

public partial class Url
{
    public Guid Id { get; set; }

    public string LongUrl { get; set; } = null!;

    public string ShortCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public Guid UserId { get; set; }

    public virtual ICollection<Click> Clicks { get; set; } = new List<Click>();

    public virtual User User { get; set; } = null!;
}
