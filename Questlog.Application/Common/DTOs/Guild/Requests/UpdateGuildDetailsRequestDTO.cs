using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Guild.Requests
{
    public record UpdateGuildDetailsRequestDTO
    {
        [Required(ErrorMessage = "Guild id is required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Guild name is required")]
        [MinLength(3, ErrorMessage = "Guild name must be atleast 3 characters")]
        [MaxLength(20, ErrorMessage = "Guild name cannot exceed 20 characters")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Guild description is required")]
        [MinLength(5, ErrorMessage = "Guild description must be atleast 5 characters")]
        [MaxLength(100, ErrorMessage = "Guild description cannot exceed 100 characters")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Guild leader id is required")]
        public string GuildLeaderId { get; set; }
        [Required(ErrorMessage = "Guild color is required")]
        public string Color { get; set; }
    }
}
