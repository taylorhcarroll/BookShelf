using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookshelf.Data;
using Bookshelf.Models;
using Bookshelf.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bookshelf.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Bookshelf
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var books = await _context.Books
                .Where(b => b.ApplicationUserId == user.Id)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .ToListAsync();
            return View(books);
        }

        // GET: Bookshelf/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Bookshelf/Create
        public async Task<ActionResult> Create()
        {
            var genreOptions = await _context.Genres
               .Select(g => new SelectListItem() { Text = g.Name, Value = g.Id.ToString() })
               .ToListAsync();

            var viewModel = new BookFormViewModel();

            viewModel.GenreOptions = genreOptions;

            return View(viewModel);
        }

        // POST: Bookshelf/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BookFormViewModel bookViewModel)
        {
            try
            {
                var user = await GetCurrentUserAsync();

                var book = new Book
                {
                    Title = bookViewModel.Title,
                    Author = bookViewModel.Author,
                    ApplicationUserId = user.Id,
                };

                book.BookGenres = bookViewModel.SelectGenreIds.Select(genreId => new BookGenre()
                {
                    Book = book,
                    GenreId = genreId
                }).ToList();

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Bookshelf/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var item = await _context.Books.Include(b => b.BookGenres).FirstOrDefaultAsync(b => b.Id == id);
            var loggedInUser = await GetCurrentUserAsync();

            var GenreOptions = await _context.Genres
               .Select(g => new SelectListItem() { Text = g.Name, Value = g.Id.ToString() })
               .ToListAsync();

            if (item == null)
            {
                return NotFound();
            }


            var viewModel = new BookFormViewModel()
            {
                Id = id,
                Title = item.Title,
                Author = item.Author,
                GenreOptions = GenreOptions,
                SelectGenreIds = item.BookGenres.Select(bg => bg.GenreId).ToList()
            };

            if (item.ApplicationUserId != loggedInUser.Id)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // POST: Bookshelf/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, BookFormViewModel bookFormViewModel)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                var book = new Book()
                {
                    Id = id,
                    Title = bookFormViewModel.Title,
                    Author = bookFormViewModel.Author
                };

                book.BookGenres = bookFormViewModel.SelectGenreIds.Select(genreId => new BookGenre()
                {
                    Book = book,
                    GenreId = genreId
                }).ToList();

                book.ApplicationUserId = user.Id;

                _context.Books.Update(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Bookshelf/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var user = await GetCurrentUserAsync();
            var book = await _context.Books
                        .Include(b => b.BookGenres)
                            .ThenInclude(bg => bg.Genre)
                                .FirstOrDefaultAsync(item => item.Id == id);
            if (book.ApplicationUserId != user.Id)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Bookshelf/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Book book)
        {
            try
            {

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}