using System;
using System.Collections.Generic;

namespace Notes.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? PreferredColor { get; set; }

    public virtual ICollection<NoteFileUser> NoteFileUsers { get; set; } = new List<NoteFileUser>();
}
