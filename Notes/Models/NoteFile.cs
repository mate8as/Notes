using System;
using System.Collections.Generic;

namespace Notes.Models;

public partial class NoteFile
{
    public int Id { get; set; }

    public string RawContent { get; set; } = null!;

    public int CreatedById { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<NoteFileUser> NoteFileUsers { get; set; } = new List<NoteFileUser>();
}
