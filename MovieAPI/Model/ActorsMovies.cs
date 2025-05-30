﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Model
{
    public class ActorsMovies
    {
        public int Id { get; set; }
        [ForeignKey("Actor")]
        public int ActorId { get; set; }
        public Actor Actor { get; set; }

        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
