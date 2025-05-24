using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DBContext;
using MovieAPI.Model;
using MovieAPI.Repository.Contract;

namespace MovieAPI.Repository
{
    public class ActorRepository : IActorRepository
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ActorRepository(Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public bool ActorFoundById(int Id)
        => GetActorById(Id) != null;

        public async Task AddActorAsync(Actor NewActor)
        {
            await _context.Actors.AddAsync(NewActor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteActorAsync(int id)
        {
           var Actor = GetActorById(id);
            if (!string.IsNullOrEmpty(Actor.Image))
            {
                DeleteImageAsync(Actor.Image);
            }
            _context.Actors.Remove(Actor);
            await _context.SaveChangesAsync();
        }

        public void DeleteImageAsync(string Image)
        {
            var imageRelativePath = Image.TrimStart('/'); // remove the leading slash
            var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, imageRelativePath);

            // Check if the old file exists, then delete it
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
        }

        public Actor GetActorById(int id)
        => _context.Actors.FirstOrDefault(x => x.Id == id);


        public async Task<IReadOnlyList<Actor>> GetActorsAsync(int Page, int ItemsPerPage)
        => await _context.Actors.Skip((Page - 1) * ItemsPerPage).Take(ItemsPerPage).AsNoTracking().ToListAsync();

        public async Task<int> ActorsCountAsync()
        => await _context.Actors.CountAsync();

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) return "";


            // Generate a unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            string file = "/images/" + fileName;
            return file;
        }

        public async Task UpdateActorByIdAsync(int Id, Actor NewActor)
        {
            var Actor = GetActorById(Id);
            if (!string.IsNullOrEmpty(Actor.Image))
            {
                DeleteImageAsync(Actor.Image);
            }
            Actor.Name = NewActor.Name;
            Actor.Image = NewActor.Image;
            Actor.BirthDate = NewActor.BirthDate;
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Actor>> GetMovieActors(int MovieId)
        => await _context.ActorsMovies.Where(x => x.MovieId == MovieId).Include(x => x.Actor).Select(x => x.Actor!).AsNoTracking().ToListAsync();

    }
}
