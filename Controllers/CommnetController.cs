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
    public class CommnetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommnetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Commnet
        public async Task<IActionResult> Index()
        {
            var testForumContext = _context.Comments.Include(c => c.ContentComment).Include(c => c.Post).Include(c => c.User);
            return View(await testForumContext.ToListAsync());
        }

        // GET: Commnet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.ContentComment)
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Commnet/Create
        public IActionResult Create()
        {
            ViewData["ContentCommentId"] = new SelectList(_context.ContentComments, "ContentCommentId", "ContentCommentId");
            ViewData["PostId"] = new SelectList(_context.Posts, "PostId", "PostId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Commnet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,CommentTime,UserId,PostId,ContentCommentId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContentCommentId"] = new SelectList(_context.ContentComments, "ContentCommentId", "ContentCommentId", comment.ContentCommentId);
            ViewData["PostId"] = new SelectList(_context.Posts, "PostId", "PostId", comment.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", comment.UserId);
            return View(comment);
        }

        // GET: Commnet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["ContentCommentId"] = new SelectList(_context.ContentComments, "ContentCommentId", "ContentCommentId", comment.ContentCommentId);
            ViewData["PostId"] = new SelectList(_context.Posts, "PostId", "PostId", comment.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", comment.UserId);
            return View(comment);
        }

        // POST: Commnet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,CommentTime,UserId,PostId,ContentCommentId")] Comment comment)
        {
            if (id != comment.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentId))
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
            ViewData["ContentCommentId"] = new SelectList(_context.ContentComments, "ContentCommentId", "ContentCommentId", comment.ContentCommentId);
            ViewData["PostId"] = new SelectList(_context.Posts, "PostId", "PostId", comment.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", comment.UserId);
            return View(comment);
        }

        // GET: Commnet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.ContentComment)
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Commnet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comments == null)
            {
                return Problem("Entity set 'TestForumContext.Comments'  is null.");
            }
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
          return (_context.Comments?.Any(e => e.CommentId == id)).GetValueOrDefault();
        }
    }
}
