using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class AuthorDbRepository : IBookStoreRepository<Author>
    {
        readonly BookStoreDBContext db;
        public AuthorDbRepository(BookStoreDBContext _db)
        {
            db = _db;
        }
        public void Add(Author entity)
        {
           // entity.Id = db.Authors.Max(a => a.Id + 1);
            db.Authors.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int Id)
        {
            var author = Find(Id);
            db.Authors.Remove(author);
            db.SaveChanges();
        }

        public Author Find(int Id)
        {
            var author = db.Authors.SingleOrDefault(a => a.Id == Id);
            return author;
        }

        public IList<Author> List()
        {
            return db.Authors.ToList();
        }

        public List<Author> Search(string term)
        {
            return db.Authors.Where(a => a.FullName.Contains(term)).ToList();
        }

        public void Update(int Id, Author newAuthor)
        {
            db.Authors.Update(newAuthor);
            db.SaveChanges();
        }
    }
}
