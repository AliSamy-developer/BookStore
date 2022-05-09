using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class BookStoreDBContext :DbContext
    {
        public BookStoreDBContext(DbContextOptions<BookStoreDBContext> options):base(options)
        {

        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        internal int Max(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}
