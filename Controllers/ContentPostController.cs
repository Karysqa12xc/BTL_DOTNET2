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
            PostContentViewModel contentViewModel = new PostContentViewModel
            {
                ContentPost = contentPost,
            };
            return View(contentViewModel);
        }

        // POST: ContentPost/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostContentViewModel contentViewModel, IFormFile fileImg, IFormFile fileVideo)
        {
            if (id != contentViewModel.ContentPost.ContentPostId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {


                    string? oldImage = contentViewModel.ContentPost.Image;
                    string? oldVideo = contentViewModel.ContentPost.Video;
                    
                    if (contentViewModel.ImgUrl != null && contentViewModel.ImgUrl.Length > 0)
                    {
                        fileImg = contentViewModel.ImgUrl;
                        string? strImgReplace = await _editImgPost.UploadImage(fileImg, "/images/post/", "Post");
                        contentViewModel.ContentPost.Image = strImgReplace;
                        if (!string.IsNullOrEmpty(oldImage))
                        {
                            string oldImageUrl = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldImage.TrimStart('/'));
                            if (System.IO.File.Exists(oldImageUrl))
                            {
                                System.IO.File.Delete(oldImageUrl);
                            }
                        }
                    }
                    if (contentViewModel.VideoUrl != null && contentViewModel.VideoUrl.Length > 0)
                    {
                        fileVideo = contentViewModel.VideoUrl;
                        string? strVideoReplace = await _editVideoPost.UploadVideo(fileVideo, "/videos/post/", "Post");
                        contentViewModel.ContentPost.Video = strVideoReplace;
                        if (!string.IsNullOrEmpty(oldVideo))
                        {
                            string oldVideoUrl = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldVideo.TrimStart('/'));
                            if (System.IO.File.Exists(oldVideoUrl))
                            {
                                System.IO.File.Delete(oldVideoUrl);
                            }
                        }
                    }
                    _context.Update(contentViewModel.ContentPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentPostExists(contentViewModel.ContentPost.ContentPostId))
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
            return View(contentViewModel);
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
            string? oldImageContentPost = contentPost!.Image;
            string? oldVideoContentPost = contentPost!.Video;
            if (contentPost != null)
            {
                _context.ContentPosts.Remove(contentPost);
            }

            await _context.SaveChangesAsync();
            if (!string.IsNullOrEmpty(oldImageContentPost))
            {
                string imagePathContentPost = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldImageContentPost.TrimStart('/'));
                if (System.IO.File.Exists(imagePathContentPost))
                {
                    System.IO.File.Delete(imagePathContentPost);
                }
            }
            if (!string.IsNullOrEmpty(oldVideoContentPost))
            {
                string videoPathContentPost = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldVideoContentPost.TrimStart('/'));
                if (System.IO.File.Exists(videoPathContentPost))
                {
                    System.IO.File.Delete(videoPathContentPost);
                }
            }
            return RedirectToAction("Index", "Post");
        }

        private bool ContentPostExists(int id)
        {
            return _context.ContentPosts.Any(e => e.ContentPostId == id);
        }
    }
}
