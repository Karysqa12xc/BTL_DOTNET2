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
using Microsoft.AspNetCore.Authorization;
using BTL_DOTNET2.Models.Process;

namespace BTL_DOTNET2.Controllers
{
    [Authorize]
    public class CommnentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UploadImgProcess _uploadImgComment = new UploadImgProcess();
        private UploadVideoProcess _uploadVideoComment = new UploadVideoProcess();
        protected UserManager<User> _userManager;
        public CommnentController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Commnent
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comments
                .Include(c => c.ContentComment)
                .Include(c => c.Post)
                .Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: /Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.ContentComment)
                .Include(c => c.Post).Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }
        // POST: Commnent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostContentViewModel postContentViewModel, IFormFile fileImage, IFormFile fileVideo)
        {
            if (!ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(postContentViewModel.ContentComment.Paragraph))
                {
                    postContentViewModel.Comment.CommentTime = DateTime.Now;
                    postContentViewModel.Comment.User = await _userManager.GetUserAsync(HttpContext.User);
                    if (postContentViewModel.ImgUrl != null && postContentViewModel.ImgUrl.Length > 0)
                    {
                        fileImage = postContentViewModel.ImgUrl;
                        var imageCommentPath = await _uploadImgComment.UploadImage(fileImage, "/images/comment/", "Comment");
                        postContentViewModel.ContentComment.Image = imageCommentPath;
                    }
                    if(postContentViewModel.VideoUrl != null && postContentViewModel.VideoUrl.Length > 0){
                        fileVideo = postContentViewModel.VideoUrl;
                        var videoCommentPath = await _uploadVideoComment.UploadVideo(fileVideo, "/videos/comment/", "Comment");
                        postContentViewModel.ContentComment.Video = videoCommentPath;
                    }
                    _context.Add(postContentViewModel.ContentComment);
                    _context.SaveChanges();

                    postContentViewModel.Comment.ContentCommentId = postContentViewModel.ContentComment.ContentCommentId;
                    _context.Add(postContentViewModel.Comment);
                    await _context.SaveChangesAsync();

                    // return RedirectToAction("Details", "Post", new { id = postContentViewModel.Comment.PostId });
                    return RedirectToAction("Details", "Post", new { id = postContentViewModel.Comment.PostId });
                }
            }
            return RedirectToAction("Details", "Post", new { id = postContentViewModel.Comment.PostId });
        }
        // GET: Commnent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
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
            return View(comment);
        }

        // POST: Commnent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,CommentTime,PostId,ContentCommentId")] Comment comment)
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
            return View(comment);
        }

        // GET: Commnent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.ContentComment)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Commnent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
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
            return _context.Comments.Any(e => e.CommentId == id);
        }
    }
}
