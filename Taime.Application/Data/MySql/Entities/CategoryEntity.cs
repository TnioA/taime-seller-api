using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Taime.Application.Contracts.Category;
using Taime.Application.Utils.Data.MySql;

namespace Taime.Application.Data.MySql.Entities
{
    [Table("category")]
    public class CategoryEntity : MySqlEntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("url")]
        public string Url { get; set; }
        public CategoryEntity() { }

        public CategoryEntity(CategoryRequest request)
        {
            Title = request.Title;
            Url = request.Url;
        }
    }
}