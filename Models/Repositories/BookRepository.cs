using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class BookRepository : IBookStoreRepository<Book>
    {
        readonly List<Book> books;

        public BookRepository()
        {
            books = new List<Book>()
            {
                new Book
                {
                    Id=1,Title="C#",
                    Description="no des",
                    ImageUrl="IMG-20211212-WA0067.jpg",
                    Author= new Author(){Id=2}
                },
                new Book
                {
                    Id=2,
                    Title="java",
                    Description="nothing",
                    ImageUrl="IMG-20211212-WA0068.jpg",
                    Author= new Author()
                },
                new Book
                {
                    Id=3,
                    Title="python",
                    Description="no data",
                    ImageUrl="IMG-20211212-WA0072.jpg",
                    Author= new Author()
                },
            };
        }
        public void Add(Book entity)
        {
            entity.Id = books.Max(b => b.Id+1);
            books.Add(entity);
        }

        public void Delete(int Id)
        {
            var book = Find(Id);
            books.Remove(book);
        }

        public Book Find(int Id)
        {
            var book = books.SingleOrDefault(b => b.Id == Id);
            return book;
        }

        public IList<Book> List()
        {
            return books;
        }

        public List<Book> Search(string term)
        {
            return books.Where(a => a.Title.Contains(term)).ToList();
        }

        public void Update(int Id ,Book newBook)
        {
            var book = Find(Id);
            book.Title = newBook.Title;
            book.Description = newBook.Description;
            book.Author = newBook.Author;
            book.ImageUrl = newBook.ImageUrl;
        }
    }
}
