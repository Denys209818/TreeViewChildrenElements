using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TreeViewAddElement.Entities
{
    [Table("tblCategoryElement")]
    public class TreeViewCategoryElement
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Name { get; set; }
        [Required, StringLength(255)]
        public string UrlSlug { get; set; }
        [ForeignKey("Parent.Id")]
        public int? ParentId { get; set; }
        public virtual TreeViewCategoryElement Parent { get; set; }
    }
}
