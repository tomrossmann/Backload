using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Backload.Examples.Example08.Data.Models;

namespace Backload.Examples.Example08.Data
{

    public class ArtistsLibrary : DbContext
    {
        public ArtistsLibrary()
            : base("name=ArtistsLibrary")
        {
        }

        public DbSet<Artist> Artists { get; set; }

    }
}