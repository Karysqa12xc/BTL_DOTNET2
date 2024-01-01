using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTL_DOTNET2.Data;
using BTL_DOTNET2.Models;
using Microsoft.AspNetCore.Authorization;
using BTL_DOTNET2.Models.Process;

namespace BTL_DOTNET2.Controllers
{
    [Authorize]
    public class ContentPostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UploadImgProcess _editImgPost = new UploadImgProcess();
        private UploadVideoProcess _editVideoPost = new UploadVideoProcess();
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
            var MediaPosts = await _context.ContentTotals
                            .Where(m => m.ContentPostId == contentPost.ContentPostId)
                            .ToListAsync();
            PostCommentContentViewModel contentViewModel = new PostCommentContentViewModel
            {
                ContentPost = contentPost,
                MediaContentPost = MediaPosts,
            };
            return View(contentViewModel);
        }

        // POST: ContentPost/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostCommentContentViewModel postContentViewModel, List<IFormFile> fileImgs, List<IFormFile> fileVideos)
        {
            if (id != postContentViewModel.ContentPost.ContentPostId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    List<string> MediaPaths = new List<string>();
                    var selectedItems = postContentViewModel
                                .MediaContentPost
                                .Where(s => s != null && s.IsSelected)
                                .ToList();
                    if (selectedItems != null)
                    {
                        foreach (var mediaPath in selectedItems)
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
                        _context.ContentTotals.RemoveRange(selectedItems);
                        _context.SaveChanges();
                    }
                    if (postContentViewModel.ImgUrls != null)
                    {
                        foreach (var img in postContentViewModel.ImgUrls)
                        {
                            fileImgs.Add(img);
                        }
                        foreach (var imgFile in fileImgs)
                        {
                            var imgPathStrs = await _editImgPost.UploadImage(imgFile, "/images/post/", "Post");
                            _context.Add(new ContentTotal { Path = imgPathStrs, MediaType = MediaType.Image, ContentPostId = id });
                        }
                    }
                    if (postContentViewModel.VideoUrls != null)
                    {
                        foreach (var vid in postContentViewModel.VideoUrls)
                        {
                            fileVideos.Add(vid);
                        }
                        foreach (var vidFile in fileVideos)
                        {
                            var vidPathStrs = await _editVideoPost.UploadVideo(vidFile, "/videos/Post/", "Post");
                            _context.Add(new ContentTotal { Path = vidPathStrs, MediaType = MediaType.Video, ContentPostId = id });
                        }
                    }
                    _context.Update(postContentViewModel.ContentPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentPostExists(postContentViewModel.ContentPost.ContentPostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var postId = _context.Posts.Where(cp => cp.ContentPostId == id).Select(p => p.PostId).FirstOrDefault();
                return RedirectToAction("Details", "Post", new { id = postId });
            }
            return View(postContentViewModel);
        }
        // GET: ContentPost/Delete/5
        public async Task<IActionResult> Delete(int? id, bool isDeleteFromPost = false)
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
            if (isDeleteFromPost)
            {
                return View("DeleteConfirmed", contentPost);
            }
            return View(contentPost);
        }

        // POST: ContentPost/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contentPost = await _context.ContentPosts.FindAsync(id);
            // string? oldImageContentPost = contentPost!.Image;
            // string? oldVideoContentPost = contentPost!.Video;
            var mediaPaths = _context.ContentTotals
                           .Where(ct => ct.ContentPostId == id)
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
            if (contentPost != null)
            {
                _context.ContentPosts.Remove(contentPost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Post");
        }

        private bool ContentPostExists(int id)
        {
            return _context.ContentPosts.Any(e => e.ContentPostId == id);
        }
    }
}
