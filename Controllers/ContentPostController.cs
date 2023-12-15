using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL_DOTNET2.Data;
using BTL_DOTNET2.Models;
using Microsoft.AspNetCore.Authorization;

namespace BTL_DOTNET2.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Edit(int id, PostContentViewModel contentViewModel, IFormFile file)
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
                    

                    if (contentViewModel.ImgUrl != null && contentViewModel.ImgUrl.Length > 0)
                    {
                        file = contentViewModel.ImgUrl;
                        string? strImgReplace = await UploadImage(file);
                        contentViewModel.ContentPost.Image = strImgReplace;
                    }
                    _context.Update(contentViewModel.ContentPost);
                    await _context.SaveChangesAsync();

                    if(!string.IsNullOrEmpty(oldImage)){
                        string oldImageUrl = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldImage.TrimStart('/'));
                        if(System.IO.File.Exists(oldImageUrl)){
                            System.IO.File.Delete(oldImageUrl);
                        }
                    }
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
                return RedirectToAction("Index", "Post");
            }
            return View(contentViewModel);
        }
        private async Task<string?> UploadImage(IFormFile file)
        {
            // Tạo folder nếu chưa tồn tại
            string PathImgPost = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "post");
            string strKey = Guid.NewGuid().ToString();
            // Tạo tên file duy nhất
            string uniqueFileName = "Post" + $"_{strKey}" + "_" + file.FileName;

            // Lưu hình ảnh vào folder
            string filePath = Path.Combine(PathImgPost, uniqueFileName);
            using (var fileStream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(fileStream);
            }

            // Trả về đường dẫn của hình ảnh để lưu vào cơ sở dữ liệu
            return "/images/post/" + uniqueFileName;
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
            if (contentPost != null)
            {
                _context.ContentPosts.Remove(contentPost);
            }

            await _context.SaveChangesAsync();
            if(!string.IsNullOrEmpty(oldImageContentPost)){
                string imagePathContentPost = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldImageContentPost.TrimStart('/'));
                if(System.IO.File.Exists(imagePathContentPost)){
                    System.IO.File.Delete(imagePathContentPost);
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
