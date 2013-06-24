using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Backload.Examples.Example08.Data.Models
{
    public class Artist
    {
        public string ArtistId { get; set; }
        public string ArtistName { get; set; }
    }
}
