using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTL_DOTNET2.Data;
using BTL_DOTNET2.Models;
using Microsoft.AspNetCore.Authorization;
using BTL_DOTNET2.Models.Process;
using System.IO;
namespace BTL_DOTNET2.Controllers
{
    [Authorize]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostCommentContentViewModel contentCommentViewModel, List<ContentTotal> contentTotals ,List<IFormFile> fileImgs, List<IFormFile> fileVideos, List<bool> isDelete)
        {
            if (id != contentCommentViewModel.ContentComment.ContentCommentId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    // string? oldImage = contentCommentViewModel.ContentComment.Image;
                    // string? strImgReplace;
                    // string? oldVideo = contentCommentViewModel.ContentComment.Video;
                    // string? strVideoReplace;
                    List<int> mediaIdsToDelete = new List<int>();
                    contentTotals = contentCommentViewModel.MediaContentComment;
                    foreach (var media in contentTotals)
                    {
                        if (media.IsSelected)
                        {
                            if (!string.IsNullOrEmpty(media.Path))
                            {
                                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", media.Path.TrimStart('/'));
                                if (System.IO.File.Exists(fullPath))
                                {
                                    System.IO.File.Delete(fullPath);
                                }
                            }
                            mediaIdsToDelete.Add(media.MediaId);
                        }
                    }
                    foreach (var mediaId in mediaIdsToDelete)
                    {
                        var mediaToDelete = await _context.ContentTotals.FindAsync(mediaId);
                        if (mediaToDelete != null)
                        {
                            _context.Remove(mediaToDelete);
                        }
                    }
                    await _context.SaveChangesAsync();
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
                    // if (contentCommentViewModel.ImgUrl != null && contentCommentViewModel.ImgUrl.Length > 0)
                    // {
                    //     fileImg = contentCommentViewModel.ImgUrl;
                    //     strImgReplace = await _editImgComment.UploadImage(fileImg, "/images/comment/", "Comment");
                    //     contentCommentViewModel.ContentComment.Image = strImgReplace;
                    //     if (!string.IsNullOrEmpty(oldImage))
                    //     {
                    //         string oldImageUrl = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldImage.TrimStart('/'));
                    //         if (System.IO.File.Exists(oldImageUrl))
                    //         {
                    //             System.IO.File.Delete(oldImageUrl);
                    //         }
                    //     }
                    // }
                    // if(contentCommentViewModel.VideoUrl != null && contentCommentViewModel.VideoUrl.Length > 0){
                    //     fileVideo = contentCommentViewModel.VideoUrl;
                    //     strVideoReplace = await _editVideoComment.UploadVideo(fileVideo, "/videos/comment/", "Comment");
                    //     contentCommentViewModel.ContentComment.Video = strVideoReplace;
                    //     if(!string.IsNullOrEmpty(oldVideo)){
                    //         string oldVideoUrl = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldVideo.TrimStart('/'));
                    //         if(System.IO.File.Exists(oldVideoUrl)){
                    //             System.IO.File.Delete(oldVideoUrl);                                
                    //         }
                    //     }
                    // }
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
            // string? oldImageComment = contentComment!.Image;
            // string? oldVideoComment = contentComment!.Video;
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


            // if (!string.IsNullOrEmpty(oldImageComment))
            // {
            //     string imagePathComment = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldImageComment.TrimStart('/'));
            //     if (System.IO.File.Exists(imagePathComment))
            //     {
            //         System.IO.File.Delete(imagePathComment);
            //     }
            // }
            // if(!string.IsNullOrEmpty(oldVideoComment)){
            //     string videoCommentPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldVideoComment.TrimStart('/'));
            //     if(System.IO.File.Exists(videoCommentPath)){
            //         System.IO.File.Delete(videoCommentPath);
            //     }
            // }
            return RedirectToAction("Details", "Post", new { id = postId });
        }

        private bool ContentCommentExists(int id)
        {
            return _context.ContentComments.Any(e => e.ContentCommentId == id);
        }
    }
}
