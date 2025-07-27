using System;
using System.Collections.Generic;

namespace LinkShortner.Api.Models.Entities;

public partial class Click
{
    public Guid Id { get; set; }

    public Guid UrlId { get; set; }

    public DateTime ClickedAt { get; set; }

    public virtual Url Url { get; set; } = null!;
}
