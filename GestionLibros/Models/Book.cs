using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionLibros.Models
{
    public enum ReadingStatus
    {
        PorLeer,
        Leyendo,
        Leido
    }

    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public ReadingStatus Status { get; set; } = ReadingStatus.PorLeer;
        public int Rating { get; set; }
        public string? PhotoPath { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
