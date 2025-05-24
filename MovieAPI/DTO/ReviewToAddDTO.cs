using Microsoft.AspNetCore.Identity;
using MovieAPI.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.DTO
{
    public class ReviewToAddDTO
    {
        public int MovieId { get; set; }
        public double Rate { get; set; }
        public string Comment { get; set; }
    }
}
