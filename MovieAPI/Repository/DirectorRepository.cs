using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DBContext;
using MovieAPI.Model;
using MovieAPI.Repository.Contract;

namespace MovieAPI.Repository
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DirectorRepository(Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public bool DirectorFoundById(int Id)
        => GetDirectorById(Id) != null;

        public async Task AddDirectorAsync(Director NewDirector)
        {
            await _context.Directors.AddAsync(NewDirector);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDirectorAsync(int id)
        {
           var Director = GetDirectorById(id);
            if (!string.IsNullOrEmpty(Director.Image))
            {
                DeleteImageAsync(Director.Image);
            }
            _context.Directors.Remove(Director);
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

        public Director GetDirectorById(int id)
        => _context.Directors.FirstOrDefault(x => x.Id == id);

        public async Task<IReadOnlyList<Director>> GetDirectorsAsync(int Page, int ItemsPerPage)
        => await _context.Directors.Skip((Page - 1) * ItemsPerPage).Take(ItemsPerPage).AsNoTracking().ToListAsync();
        public async Task<IReadOnlyList<Director>> GetDirectorsAsync()
        => await _context.Directors.AsNoTracking().ToListAsync();

        public async Task<int> DirectorsCountAsync()
        => await _context.Directors.CountAsync();

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

        public async Task UpdateDirectorByIdAsync(int Id, Director NewDirector)
        {
            var Director = GetDirectorById(Id);
            if (!string.IsNullOrEmpty(Director.Image) && !string.IsNullOrEmpty(NewDirector.Image))
            {
                DeleteImageAsync(Director.Image);
                Director.Image = NewDirector.Image;
            }
            Director.Name = NewDirector.Name;
            Director.BirthDate = NewDirector.BirthDate;
            await _context.SaveChangesAsync();
        }


    }
}
