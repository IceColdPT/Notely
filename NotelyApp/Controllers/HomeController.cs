using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotelyApp.Models;
using NotelyApp.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NotelyApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INoteRepository noteRepository;
       

        public HomeController(ILogger<HomeController> logger, INoteRepository _repository)
        {
            _logger = logger;
            noteRepository = _repository;
        }

        public IActionResult Index()
        {
            var notes = noteRepository.GetAllNotes().Where(n => n.IsDeleted == false);
            return View(notes);
        }

        public IActionResult NoteDetail(Guid guid)
        {
            var note = noteRepository.FindNoteById(guid);
            return View(note);
        }

        [HttpGet]
        public IActionResult NoteEditor(Guid guid = default)
        {
            if(guid != Guid.Empty)
            {
                var note = noteRepository.FindNoteById(guid);
                return View(note);
            }
            return View();
        }

        [HttpPost]
        public IActionResult NoteEditor(NoteModel noteModel)
        {
            var date = DateTime.Now;
            if(noteModel != null && noteModel.Id == Guid.Empty)
            {
                noteModel.Id = Guid.NewGuid();
                noteModel.CreatedDate = date;
                noteModel.LastModified = date;
                noteRepository.SaveNote(noteModel);
            }
            else
            {
                var note = noteRepository.FindNoteById(noteModel.Id);
                note.LastModified = date;
                note.Subject = noteModel.Subject;
                note.Detail = noteModel.Detail;
            }

            return RedirectToAction("Index");
        }

        public IActionResult DeleteNote(Guid guid)
        {
            var note = noteRepository.FindNoteById(guid);

            note.IsDeleted = true;

            return RedirectToAction("Index");
        }


    
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
