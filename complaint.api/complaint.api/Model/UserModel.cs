using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace complaint.api.Model {

    [Table("userTbl")]
    public class UserModel 
        {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userId { get; set; }
        [Required]
        public string email { get; set; } = string.Empty;
        [Required]
        [MinLength(10)]
        [MaxLength(10)]
        public string mobile { get; set; } = string.Empty;
        [Required]
        public string city { get; set; } = string.Empty;
        public string state { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string address { get; set; } = string.Empty;

    }
}
