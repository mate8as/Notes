using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Notes.Models;

public partial class User : IdentityUser
{
    public string Id { get; set; }

    public string? PreferredColor { get; set; }

    public virtual ICollection<NoteFileUser> NoteFileUsers { get; set; } = new List<NoteFileUser>();
}
