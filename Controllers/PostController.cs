using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL_DOTNET2.Data;
using BTL_DOTNET2.Models;

namespace BTL_DOTNET2.Controllers
{
    public class PostController : Controller
    {
        private readonly TestForumContext _context;

        public PostController(TestForumContext context)
        {
            _context = context;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
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
            ViewData["CateId"] = new SelectList(_context.Categories, "CateId", "CateName");
            ViewData["ContentPostId"] = new SelectList(_context.ContentPosts, "ContentPostId", "ContentPostId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserName");
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,PostTime,CommentTotal,IsSave,UserId,CateId,ContentPostId")] Post post)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // var c = from cate in _context.Categories where (cate.CateId == post.CateId) select cate.CateName;
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserName", post.UserId);
            ViewData["CateId"] = new SelectList(_context.Categories, "CateId", "CateName", post.CateId);
            ViewData["ContentPostId"] = new SelectList(_context.ContentPosts, "ContentPostId", "ContentPostId", post.ContentPostId);
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

            if (!ModelState.IsValid)
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
