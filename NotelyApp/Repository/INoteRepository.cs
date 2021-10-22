using NotelyApp.Models;
using System;
using System.Collections.Generic;

namespace NotelyApp.Repository
{
    public interface INoteRepository
    {
        void DeleteNote(NoteModel noteModel);
        NoteModel FindNoteById(Guid id);
        IEnumerable<NoteModel> GetAllNotes();
        void SaveNote(NoteModel noteModel);
    }
}