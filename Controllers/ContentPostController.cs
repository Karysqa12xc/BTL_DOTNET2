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
    public class ContentPostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContentPostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContentPost
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContentPosts.ToListAsync());
        }

        // GET: ContentPost/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contentPost = await _context.ContentPosts
                .FirstOrDefaultAsync(m => m.ContentPostId == id);
            if (contentPost == null)
            {
                return NotFound();
            }

            return View(contentPost);
        }

        // GET: ContentPost/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContentPost/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContentPostId,Paragram,Image")] ContentPost contentPost)
        {
            if (ModelState.IsValid)
            {
                    _context.Add(contentPost);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index)); 
            }
            return View(contentPost);
        }

        // GET: ContentPost/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contentPost = await _context.ContentPosts.FindAsync(id);
            if (contentPost == null)
            {
                return NotFound();
            }
            return View(contentPost);
        }

        // POST: ContentPost/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContentPostId,Paragram,Image")] ContentPost contentPost)
        {
            if (id != contentPost.ContentPostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contentPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentPostExists(contentPost.ContentPostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Post");
            }
            return View(contentPost);
        }

        // GET: ContentPost/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contentPost = await _context.ContentPosts
                .FirstOrDefaultAsync(m => m.ContentPostId == id);
            if (contentPost == null)
            {
                return NotFound();
            }

            return View(contentPost);
        }

        // POST: ContentPost/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contentPost = await _context.ContentPosts.FindAsync(id);
            if (contentPost != null)
            {
                _context.ContentPosts.Remove(contentPost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContentPostExists(int id)
        {
            return _context.ContentPosts.Any(e => e.ContentPostId == id);
        }
    }
}
