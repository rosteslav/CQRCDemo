﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities
{
    [Table("Comments")]
    public class CommentsEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTime CommentDate { get; set; }
        public string Comment { get; set; }
        public bool Edited { get; set; }
        public Guid PostId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual PostEntity Post { get; set; }
    }
}