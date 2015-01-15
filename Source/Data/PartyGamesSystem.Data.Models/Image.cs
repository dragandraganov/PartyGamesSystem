using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PartyGamesSystem.Data.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        public byte[] Content { get; set; }

        public string FileExtension { get; set; }
    }
}
