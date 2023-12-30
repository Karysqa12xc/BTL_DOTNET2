using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL_DOTNET2.Data;
using BTL_DOTNET2.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;
using X.PagedList;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using Humanizer;
using BTL_DOTNET2.Models.Process;

namespace BTL_DOTNET2.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        protected UserManager<User> _userManager;

        private UploadImgProcess _uploadImgPost = new UploadImgProcess();
        private UploadVideoProcess _uploadVideoPost = new UploadVideoProcess();
        public PostController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Post
        public async Task<IActionResult> Index(int? page, int? PageSize, string searching, int? cateID)
        {
            ViewBag.CurrentUser = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.PageSize = new List<SelectListItem>(){
                new SelectListItem(){Value = "5", Text = "5"},
                new SelectListItem(){Value = "10", Text = "10"},
                new SelectListItem(){Value = "15", Text = "15"},
                new SelectListItem(){Value = "20", Text = "20"},
            };
            ViewBag.Categories = new SelectList(_context.Categories, "CateId", "CateName");
            ViewBag.CurrentCategoryId = cateID;
            ViewBag.SearchResult = searching;
            int pagesize = (PageSize ?? 5);
            ViewBag.psize = pagesize;
            var posts = _context.Posts
            .Include(p => p.Cate)
            .Include(p => p.ContentPost)
            .Include(p => p.User)
            .Where(p =>
            (p.Title.Contains(searching)
            || searching == null) && (cateID == null || p.CateId == cateID)
            && p.IsChecked)
            .OrderByDescending(p => p.PostTime)
            .ToList()
            .ToPagedList(page ?? 1, pagesize);
            return View(posts);
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.CurrentUser = await _userManager.GetUserAsync(HttpContext.User);
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
            var postContentViewModel = new PostCommentContentViewModel
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
        public async Task<IActionResult> Create(PostCommentContentViewModel postContentViewModel, List<IFormFile> imgPath, List<IFormFile> videoPath)
        {

            if (!ModelState.IsValid)
            {
                postContentViewModel.Post!.PostTime = DateTime.Now;
                postContentViewModel.Post.CommentTotal = 0;
                if (postContentViewModel.Post.Title != null)
                {
                    // if (postContentViewModel.ImgUrl != null && postContentViewModel.ImgUrl.Length > 0)
                    // {
                    //     imgPath = postContentViewModel.ImgUrl;
                    //     var imagePathStr = await _uploadImgPost.UploadImage(imgPath, "/images/post/", "Post");
                    //     postContentViewModel.ContentPost.Image = imagePathStr;
                    // }
                    // if (postContentViewModel.VideoUrl != null && postContentViewModel.VideoUrl.Length > 0)
                    // {
                    //     videoPath = postContentViewModel.VideoUrl;
                    //     var videoPathStr = await _uploadVideoPost.UploadVideo(videoPath, "/videos/post/", "Post");
                    //     postContentViewModel.ContentPost.Video = videoPathStr;
                    // }
                    postContentViewModel.Post.User = await _userManager.GetUserAsync(HttpContext.User);
                    _context.Add(postContentViewModel.ContentPost);
                    await _context.SaveChangesAsync();
                    if (postContentViewModel.ImgUrls != null)
                    {
                        foreach (var imgFile in postContentViewModel.ImgUrls)
                        {
                            imgPath.Add(imgFile);
                        }
                        foreach (var imgs in imgPath)
                        {
                            var imagePathStrs = await _uploadImgPost.UploadImage(imgs, "/images/post/", "Post");
                            _context.Add(new ContentTotal { Path = imagePathStrs, MediaType = MediaType.Image, ContentPostId = postContentViewModel.ContentPost.ContentPostId });
                        }
                    }
                    if(postContentViewModel.VideoUrls != null){
                        foreach(var videoFile in postContentViewModel.VideoUrls){
                            videoPath.Add(videoFile);
                        }
                        foreach(var vids in videoPath){
                            var videoPathStrs = await _uploadVideoPost.UploadVideo(vids, "/videos/post/", "Post");
                            _context.Add(new ContentTotal { Path = videoPathStrs, MediaType = MediaType.Video, ContentPostId = postContentViewModel.ContentPost.ContentPostId });
                        }
                    }
                    postContentViewModel.Post.ContentPostId = postContentViewModel.ContentPost!.ContentPostId;
                    _context.Add(postContentViewModel.Post);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
        public async Task<IActionResult> PostOfUser(int? page, int? PageSize, string searching, int? cateId)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.CurrentUser = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.PageSize = new List<SelectListItem>(){
                new SelectListItem(){Value = "5", Text = "5"},
                new SelectListItem(){Value = "10", Text = "10"},
                new SelectListItem(){Value = "15", Text = "15"},
                new SelectListItem(){Value = "20", Text = "20"},
            };
            ViewBag.Categories = new SelectList(_context.Categories, "CateId", "CateName");
            ViewBag.CurrentCategoryId = cateId;
            ViewBag.SearchResult = searching;
            int pagesize = (PageSize ?? 5);
            ViewBag.psize = pagesize;
            var post = _context.Posts
            .Include(pu => pu.Cate)
            .Include(pu => pu.ContentPost)
            .Include(pu => pu.User)
            .Where(pu => (pu.Title.Contains(searching)
            || searching == null) && (cateId == null || pu.CateId == cateId) && pu.User == currentUser)
            .OrderByDescending(pu => pu.PostTime)
            .ToList().ToPagedList(page ?? 1, pagesize);
            return View(post);
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

        public IActionResult CheckedPost(int? page, int? PageSize, string searching, int? cateId)
        {
            ViewBag.PageSize = new List<SelectListItem>(){

                new SelectListItem(){Value = "5", Text = "5"},
                new SelectListItem(){Value = "10", Text = "10"},
                new SelectListItem(){Value = "15", Text = "15"},
            };
            ViewBag.Categories = new SelectList(_context.Categories, "CateId", "CateName");
            ViewBag.CurrentCategoryId = cateId;
            ViewBag.SearchResult = searching;
            int pagesize = (PageSize ?? 5);
            ViewBag.psize = pagesize;
            var posts = _context.Posts
            .Include(p => p.Cate)
            .Include(p => p.ContentPost)
            .Include(p => p.User).Where(p => (p.IsChecked != true)
            && (cateId == null || p.CateId == cateId)
            && (p.Title.Contains(searching) || searching == null))
            .OrderByDescending(p => p.PostTime)
            .ToList()
            .ToPagedList(page ?? 1, pagesize);
            return View(posts);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckedPost(int id, bool IsChecked)
        {
            if (ModelState.IsValid)
            {
                var existingPost = await _context.Posts.FindAsync(id);

                if (existingPost == null)
                {
                    return NotFound();
                }

                existingPost.IsChecked = !IsChecked;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();

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
