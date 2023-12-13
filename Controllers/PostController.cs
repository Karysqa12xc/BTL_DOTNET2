using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL_DOTNET2.Data;
using BTL_DOTNET2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BTL_DOTNET2.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        protected UserManager<User> _userManager;

        public PostController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Cate).Include(p => p.ContentPost).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
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
            // ViewData["ContentPostId"] = new SelectList(_context.ContentPosts, "ContentPostId", "Paragram");
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Bind("PostId,Title,PostTime,CommentTotal,IsSave,CateId,ContentPostId")] Post post
        public async Task<IActionResult> Create(PostContentViewModel postContentViewModel)
        {
            // ModelState.Remove("PostTime");
            // if (!ModelState.IsValid)
            // {
            //     post.PostTime = DateTime.Now;
            //     post.CommentTotal = 0;
            //     if (post.Title != null)
            //     {
            //         post.User = await _userManager.GetUserAsync(HttpContext.User);

            //         _context.Add(post);
            //         await _context.SaveChangesAsync();
            //         return RedirectToAction(nameof(Index));
            //     }
            // }

            // ViewData["CateId"] = new SelectList(_context.Categories, "CateId", "CateName", post.CateId);
            // return View(post);
            if (!ModelState.IsValid)
            {
                postContentViewModel.Post!.PostTime = DateTime.Now;
                postContentViewModel.Post.CommentTotal = 0;
                if (postContentViewModel.Post.Title != null)
                {

                    postContentViewModel.Post.User = await _userManager.GetUserAsync(HttpContext.User);

                    _context.Add(postContentViewModel.ContentPost);
                    _context.SaveChanges();

                    postContentViewModel.Post.ContentPostId = postContentViewModel.ContentPost!.ContentPostId;

                    _context.Add(postContentViewModel.Post);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(PostContentViewModel postContentViewModel)
        {
            // if (!ModelState.IsValid)
            // {
            //     comment.CommentTime = DateTime.Now;
            //     _context.Add(comment);
            //     await _context.SaveChangesAsync();
            //     return RedirectToAction(nameof(Index));
            // }
            if (ModelState.IsValid)
            {
                postContentViewModel.Comment.CommentTime = DateTime.Now;
                if (postContentViewModel.Comment.ContentComment != null)
                {

                    postContentViewModel.Comment.User = await _userManager.GetUserAsync(HttpContext.User);

                    _context.Add(postContentViewModel.ContentComment);
                    _context.SaveChanges();

                    postContentViewModel.Comment.ContentCommentId = postContentViewModel.ContentComment.ContentCommentId;

                    _context.Add(postContentViewModel.Comment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            return View();
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }
            var contentPost = await _context.ContentPosts.FindAsync(post.ContentPostId);
            ViewBag.ContentPost = contentPost!.Paragram;
            return View(post);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,PostTime,CommentTotal,IsSave,CateId,ContentPostId")] Post post)
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

            return View(post);
        }

        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Cate)
                .Include(p => p.ContentPost)
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
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
