using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTL_DOTNET2.Data;
using BTL_DOTNET2.Models;
using Microsoft.AspNetCore.Authorization;
using BTL_DOTNET2.Models.Process;
using System.IO;
namespace BTL_DOTNET2.Controllers
{
    public class ContentCommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UploadImgProcess _editImgComment = new UploadImgProcess();
        private UploadVideoProcess _editVideoComment = new UploadVideoProcess();
        public ContentCommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContentComment
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContentComments.ToListAsync());
        }
        [Authorize(Roles = "SuperAdmin")]
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
        [Authorize(Roles = "SuperAdmin")]
        // GET: ContentComment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContentComment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "SuperAdmin")]
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
        [Authorize]
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
            var MediaComments = await _context.ContentTotals
                            .Where(m => m.ContentCommentId == contentComment.ContentCommentId)
                            .ToListAsync();
            PostCommentContentViewModel contentViewModel = new PostCommentContentViewModel
            {
                ContentComment = contentComment,
                MediaContentComment = MediaComments,
            };
            return View(contentViewModel);
        }

        // POST: ContentComment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostCommentContentViewModel contentCommentViewModel, List<IFormFile> fileImgs, List<IFormFile> fileVideos)
        {
            if (id != contentCommentViewModel.ContentComment.ContentCommentId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    List<string> MediaPaths = new List<string>();
                    var selected = contentCommentViewModel
                                .MediaContentComment
                                .Where(s => s != null && s.IsSelected)
                                .ToList();
                    if (selected != null)
                    {
                        foreach (var mediaPath in selected)
                        {
                            if (mediaPath.IsSelected && !string.IsNullOrEmpty(mediaPath.Path))
                            {
                                MediaPaths.Add(mediaPath.Path);
                            }else{
                                return NotFound();
                            }
                        }
                        foreach (var path in MediaPaths)
                        {
                            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path.TrimStart('/'));
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                        }
                        _context.ContentTotals.RemoveRange(selected);
                        _context.SaveChanges();
                    }


                    if (contentCommentViewModel.ImgUrls != null)
                    {
                        foreach (var img in contentCommentViewModel.ImgUrls)
                        {
                            fileImgs.Add(img);
                        }
                        foreach (var imgFile in fileImgs)
                        {
                            var imgPathStrs = await _editImgComment.UploadImage(imgFile, "/images/comment/", "Comment");
                            _context.Add(new ContentTotal { Path = imgPathStrs, MediaType = MediaType.Image, ContentCommentId = id });
                        }
                    }
                    if (contentCommentViewModel.VideoUrls != null)
                    {
                        foreach (var vid in contentCommentViewModel.VideoUrls)
                        {
                            fileVideos.Add(vid);
                        }
                        foreach (var vidFile in fileVideos)
                        {
                            var vidPathStrs = await _editVideoComment.UploadVideo(vidFile, "/videos/comment/", "Comment");
                            _context.Add(new ContentTotal { Path = vidPathStrs, MediaType = MediaType.Video, ContentCommentId = id });
                        }
                    }
                    _context.Update(contentCommentViewModel.ContentComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentCommentExists(contentCommentViewModel.ContentComment.ContentCommentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                int postId = _context.Comments.Where(cc => cc.ContentCommentId == id).Select(cc => cc.PostId).FirstOrDefault();

                return RedirectToAction("Details", "Post", new { id = postId });
            }
            return View(contentCommentViewModel);
        }
        [Authorize]
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
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contentComment = await _context.ContentComments.FindAsync(id);
            int postId = _context.Comments.Where(cc => cc.ContentCommentId == id).Select(cc => cc.PostId).FirstOrDefault();
            var mediaPaths = _context.ContentTotals
                            .Where(ct => ct.ContentCommentId == id)
                            .Select(ct => new { ct.MediaType, ct.Path })
                            .ToList();
            foreach (var mediaPath in mediaPaths)
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", mediaPath.Path!.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            if (contentComment != null)
            {
                _context.ContentComments.Remove(contentComment);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Post", new { id = postId });
        }

        private bool ContentCommentExists(int id)
        {
            return _context.ContentComments.Any(e => e.ContentCommentId == id);
        }
    }
}
