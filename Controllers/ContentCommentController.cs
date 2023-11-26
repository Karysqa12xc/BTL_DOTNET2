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
    public class ContentCommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContentCommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContentComment
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContentComments.ToListAsync());
        }

        // GET: ContentComment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contentComment = await _context.ContentComments
                .FirstOrDefaultAsync(m => m.ContentCommentId == id);
            if (contentComment == null)
            {
                return NotFound();
            }

            return View(contentComment);
        }

        // GET: ContentComment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContentComment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContentCommentId,Paragraph,Image")] ContentComment contentComment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contentComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contentComment);
        }

        // GET: ContentComment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contentComment = await _context.ContentComments.FindAsync(id);
            if (contentComment == null)
            {
                return NotFound();
            }
            return View(contentComment);
        }

        // POST: ContentComment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContentCommentId,Paragraph,Image")] ContentComment contentComment)
        {
            if (id != contentComment.ContentCommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contentComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentCommentExists(contentComment.ContentCommentId))
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
            return View(contentComment);
        }

        // GET: ContentComment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contentComment = await _context.ContentComments
                .FirstOrDefaultAsync(m => m.ContentCommentId == id);
            if (contentComment == null)
            {
                return NotFound();
            }

            return View(contentComment);
        }

        // POST: ContentComment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contentComment = await _context.ContentComments.FindAsync(id);
            if (contentComment != null)
            {
                _context.ContentComments.Remove(contentComment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContentCommentExists(int id)
        {
            return _context.ContentComments.Any(e => e.ContentCommentId == id);
        }
    }
}
