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
using System.Data;
using Microsoft.Extensions.Hosting;

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
            var applicationDbContext = _context.Posts
            .Include(p => p.Cate)
            .Include(p => p.ContentPost)
            .Include(p => p.User);
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
            var comments = await _context.Comments
               .Include(c => c.ContentComment)
               .Include(c => c.User)
               .Where(c => c.PostId == id)
               .OrderByDescending(c => c.CommentTime)
               .ToListAsync();
            int commentCount = comments.Count();
            post.CommentTotal = commentCount;
            _context.SaveChanges();
            ViewBag.ImgPostUrl = post.ContentPost.Image;
            var postContentViewModel = new PostContentViewModel
            {
                Post = post,
                Comments = comments,
            };
            return View(postContentViewModel);
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
        public async Task<IActionResult> Create(PostContentViewModel postContentViewModel, IFormFile imagePath)
        {

            if (!ModelState.IsValid)
            {
                postContentViewModel.Post!.PostTime = DateTime.Now;
                postContentViewModel.Post.CommentTotal = 0;
                if (postContentViewModel.Post.Title != null)
                {
                    imagePath = postContentViewModel.ImgUrlPost;
                    if (imagePath != null && imagePath.Length > 0)
                    {
                        var imagePathStr = await UploadImage(imagePath);
                        postContentViewModel.ContentPost.Image = imagePathStr;
                    }
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

        private async Task<string?> UploadImage(IFormFile file)
        {
            // Tạo folder nếu chưa tồn tại
            string PathImgPost = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "post");
            string strKey =  Guid.NewGuid().ToString();
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
