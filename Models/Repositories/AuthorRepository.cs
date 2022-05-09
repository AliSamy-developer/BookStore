using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class AuthorRepository : IBookStoreRepository<Author>
    {
        readonly IList<Author> authors;

        public AuthorRepository()
        {
            authors = new List<Author>()
            {
                new Author{Id=1,FullName="ali Samy"},
                new Author{Id=2,FullName="mahomoud Samy"},
                new Author{Id=3,FullName="ahmed Samy"},
            };
        }
        public void Add(Author entity)
        {
            entity.Id = authors.Max(a => a.Id + 1);
            authors.Add(entity);
        }

        public void Delete(int Id)
        {
            var author = Find(Id);
            authors.Remove(author);
        }

        public Author Find(int Id)
        {
            var author = authors.SingleOrDefault(a => a.Id == Id);
            return author;
        }

        public IList<Author> List()
        {
            return authors;
        }

        public List<Author> Search(string term)
        {
            return authors.Where(a => a.FullName.Contains(term)).ToList();
        }

        public void Update(int Id, Author newAuthor)
        {
            var author = Find(Id);
            author.FullName = newAuthor.FullName;
        }
    }
}
