using BookStore.Models;
using BookStore.Models.Repositories;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookStoreRepository<Book> bookRepository;
        private readonly IBookStoreRepository<Author> authorRepository;
        private readonly IWebHostEnvironment hosting;

        public BookController(IBookStoreRepository<Book> bookRepository,
            IBookStoreRepository<Author> authorRepository,
            IWebHostEnvironment hosting)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.hosting = hosting;
        }
        // GET: BookController
        public ActionResult Index()
        {
            var books = bookRepository.List();
            return View(books);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            var books = bookRepository.Find(id);
            return View(books);
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
               // Authors = authorRepository.List().ToList()
               Authors = FillSelectList()
            };
            return View(model);
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    String fileName = UploadFile(model.File) == null ? string.Empty : UploadFile(model.File);
                    

                    if (model.AuthorId == -1)
                    {
                        ViewBag.Massage = "Please select an author from the list!";
                        
                        return View(GetAllAuthors());
                    }

                    var author = authorRepository.Find(model.AuthorId);
                    Book book = new()
                    {
                        Id = model.BookID,
                        Title = model.Titte,
                        Description = model.Description,
                        Author = author,
                        ImageUrl = fileName,
                    };
                    bookRepository.Add(book);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            ModelState.AddModelError("", "You have to fill all required fields");
            
            return View(GetAllAuthors());

            
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            var book = bookRepository.Find(id);
            var authorId = book.Author == null ? book.Author.Id = 0 : book.Author.Id;
            var ViewModel = new BookAuthorViewModel
            {
                BookID = book.Id,
                Titte = book.Title,
                Description = book.Description,
                AuthorId = authorId,
                Authors = authorRepository.List().ToList(),
                ImageUrl = book.ImageUrl,
            };
            return View(ViewModel);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BookAuthorViewModel model)
        {
            try
            {
                String fileName = UpdateFile(model.File,model.ImageUrl);
              /*  if (model.File != null)
                {
                    String uploads = Path.Combine(hosting.WebRootPath, "uploads");
                    fileName = model.File.FileName;
                    String fullPath = Path.Combine(uploads, fileName);
                    // String oldFileName = bookRepository.Find(model.BookID).ImageUrl;
                    String oldFileName = model.ImageUrl;
                    String fullOldPath = Path.Combine(uploads, oldFileName);
                    if (fullPath != fullOldPath)
                    {
                        System.IO.File.Delete(fullOldPath);
    
                        model.File.CopyTo(new FileStream(fullPath, FileMode.Create)); //to save the file in the full path
                    }
                }*/

                Book book = new()
                {
                    Id =model.BookID,
                    Title=model.Titte,
                    Description=model.Description,
                    Author =authorRepository.Find(model.AuthorId),
                    ImageUrl=fileName
                };

                bookRepository.Update(model.BookID, book);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {

                bookRepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Search(string term)
        {
            var result = bookRepository.Search(term);
            return View("index");
        }
        List<Author> FillSelectList()
        {
            var authors = authorRepository.List().ToList();
            authors.Insert(0, new Author { Id = -1, FullName = "--- please Select an author ---" });
            return authors;

        }
        BookAuthorViewModel GetAllAuthors()
        {
            var vmodel = new BookAuthorViewModel
            {
                
                Authors = FillSelectList()
            };
            return vmodel;
        }
        String UploadFile(IFormFile file)
        {
            if (file != null)
            {
                String uploads = Path.Combine(hosting.WebRootPath, "uploads");
                String fullPath = Path.Combine(uploads, file.FileName);
                file.CopyTo(new FileStream(fullPath, FileMode.Create)); //to save the file in the full path
                return file.FileName;
            }
            return null;
        }
        String UpdateFile(IFormFile file,string imageURl)
        {
            if (file != null)
            {
                String uploads = Path.Combine(hosting.WebRootPath, "uploads");
                String newPath = Path.Combine(uploads, file.FileName);
                // String oldFileName = bookRepository.Find(model.BookID).ImageUrl;
                String oldPath = Path.Combine(uploads, imageURl);
                //String fullOldPath = Path.Combine(uploads, oldFileName);
                if (oldPath != newPath)
                {
                    System.IO.File.Delete(oldPath);

                    file.CopyTo(new FileStream(newPath, FileMode.Create)); //to save the file in the full path
                }
                return file.FileName;
            }
            return imageURl;
        }
       

    }
}
