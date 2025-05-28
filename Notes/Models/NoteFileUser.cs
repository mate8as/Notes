using System;
using System.Collections.Generic;

namespace Notes.Models;

public partial class NoteFileUser
{
    public int Id { get; set; }

    public int NoteFileId { get; set; }

    public int UserId { get; set; }

    public virtual NoteFile NoteFile { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
