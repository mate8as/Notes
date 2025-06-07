using Microsoft.AspNetCore.SignalR;
using Notes.Middlewares;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Notes.Hubs
{
    public class NotesHub : Hub
    {
        public async Task JoinNote(string noteId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, noteId);
        }

        public async Task LeaveNote(string noteId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, noteId);
        }

        public async Task UpdateContent(string noteId, string newContent, string senderId)
        {
            await Clients.OthersInGroup(noteId).SendAsync("ReceiveContentUpdate", newContent, senderId);
        }

        public async Task UpdateCursor(string noteId, string userId, int cursorX, int cursorY)
        {
            await Clients.OthersInGroup(noteId).SendAsync("ReceiveCursorUpdate", userId, cursorX, cursorY);
        }
    }
}
