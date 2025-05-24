using Microsoft.AspNetCore.Identity;
using MovieAPI.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.DTO
{
    public class ReviewToReturnDTO
    {
        public int ReviewId { get; set; }
        public string UserName { get; set; }
        public double Rate { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
