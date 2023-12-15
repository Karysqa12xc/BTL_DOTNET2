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
            PostContentViewModel contentViewModel = new PostContentViewModel
            {
                ContentComment = contentComment,
            };
            return View(contentViewModel);
        }

        // POST: ContentComment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostContentViewModel contentCommentViewModel, IFormFile file)
        {
            if (id != contentCommentViewModel.ContentComment.ContentCommentId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    string? oldImage = contentCommentViewModel.ContentComment.Image;
                    if (contentCommentViewModel.ImgUrl != null && contentCommentViewModel.ImgUrl.Length > 0)
                    {
                        file = contentCommentViewModel.ImgUrl;
                        string? strImgReplace = await UploadImage(file);
                        contentCommentViewModel.ContentComment.Image = strImgReplace;
                    }
                    _context.Update(contentCommentViewModel.ContentComment);
                    await _context.SaveChangesAsync();

                    if (!string.IsNullOrEmpty(oldImage))
                    {
                        string oldImageUrl = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldImage.TrimStart('/'));
                        if (System.IO.File.Exists(oldImageUrl))
                        {
                            System.IO.File.Delete(oldImageUrl);
                        }
                    }
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
        private async Task<string?> UploadImage(IFormFile file)
        {
            // Tạo folder nếu chưa tồn tại
            string PathImgPost = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "comment");
            string strKey = Guid.NewGuid().ToString();
            // Tạo tên file duy nhất
            string uniqueFileName = "Comment" + $"_{strKey}" + "_" + file.FileName;

            // Lưu hình ảnh vào folder
            string filePath = Path.Combine(PathImgPost, uniqueFileName);
            using (var fileStream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(fileStream);
            }

            // Trả về đường dẫn của hình ảnh để lưu vào cơ sở dữ liệu
            return "/images/comment/" + uniqueFileName;
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
            string? oldImageComment = contentComment!.Image;
            int postId = _context.Comments.Where(cc => cc.ContentCommentId == id).Select(cc => cc.PostId).FirstOrDefault();
            if (contentComment != null)
            {
                _context.ContentComments.Remove(contentComment);
            }

            await _context.SaveChangesAsync();
            if (!string.IsNullOrEmpty(oldImageComment))
            {
                string imagePathComment = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldImageComment.TrimStart('/'));
                if (System.IO.File.Exists(imagePathComment))
                {
                    System.IO.File.Delete(imagePathComment);
                }
            }
            
            return RedirectToAction("Details", "Post", new { id = postId });
        }

        private bool ContentCommentExists(int id)
        {
            return _context.ContentComments.Any(e => e.ContentCommentId == id);
        }
    }
}
