using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL_DOTNET2.Data;
using BTL_DOTNET2.Models;

namespace BTL_DOTNET2.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            TempData["UserId"] = 4;
            var testForumContext = _context.Posts.Include(p => p.Cate).Include(p => p.ContentPost).Include(p => p.User);
            return View(await testForumContext.ToListAsync());
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Cate)
                .Include(p => p.ContentPost)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Post/Create
        public IActionResult Create()
        {
            ViewData["CateId"] = new SelectList(_context.Categories, "CateId", "CateId");
            // ViewData["ContentPostId"] = new SelectList(_context.ContentPosts, "ContentPostId", "ContentPostId");
            // ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewBag.UserId = 4;
            // ViewBag.CateId = 4;
            ViewBag.ContentPostId = 2;
            ViewBag.PostTime = DateTime.Now;
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,PostTime,CommentTotal,IsSave,UserId,CateId,ContentPostId")] Post post)
        {
            ModelState.Remove("PostTime");
            if (!ModelState.IsValid)
            {
                post.PostTime = DateTime.Now;
                if (post.Title != null && post.CommentTotal >= 0)
                {
                    
                    _context.Add(post);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            ViewData["CateId"] = new SelectList(_context.Categories, "CateId", "CateId", post.CateId);
            ViewData["ContentPostId"] = new SelectList(_context.ContentPosts, "ContentPostId", "ContentPostId", post.ContentPostId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", post.UserId);
            return View(post);

        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["CateId"] = new SelectList(_context.Categories, "CateId", "CateId", post.CateId);
            ViewData["ContentPostId"] = new SelectList(_context.ContentPosts, "ContentPostId", "ContentPostId", post.ContentPostId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", post.UserId);

            return View(post);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,PostTime,CommentTotal,IsSave,UserId,CateId,ContentPostId")] Post post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CateId"] = new SelectList(_context.Categories, "CateId", "CateId", post.CateId);
            ViewData["ContentPostId"] = new SelectList(_context.ContentPosts, "ContentPostId", "ContentPostId", post.ContentPostId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", post.UserId);
            return View(post);
        }

        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Cate)
                .Include(p => p.ContentPost)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'TestForumContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return (_context.Posts?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
