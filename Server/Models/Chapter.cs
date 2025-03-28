﻿using System.ComponentModel.DataAnnotations;
namespace ThePenfolio.Server.Models
{
    public class Chapter
    {
        [Key]
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastEditedOn { get; set; }
        public DateTime? ReleasedOn { get; set; }
        public string? AuthorNoteTop { get; set; }
        public string? AuthorNoteBottom { get; set; }
        
        public Book? Book { get; set; }
        public ICollection<ChapterUserLikes>? ChapterUserLikes { get; set; }
    }
}
