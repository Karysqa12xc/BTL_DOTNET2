using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BTL_DOTNET2.Data;
using BTL_DOTNET2.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BTL_DOTNET2.Controllers
{

    public class ContentTotalController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ContentTotalController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var contentTotal = _context.ContentTotals.ToList();
            var viewModel = new ContentCommentViewModel { contentTotals = contentTotal };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Delete(ContentCommentViewModel contentCommentViewModel)
        {
            if (contentCommentViewModel.SelectAll)
            {
                _context.ContentTotals.RemoveRange(contentCommentViewModel.contentTotals);
            }
            else
            {
                var selected = contentCommentViewModel.contentTotals.Where(s => s.IsSelected).ToList();
                _context.ContentTotals.RemoveRange(selected);
            }

            _context.SaveChanges();
            return Redirect(nameof(Index));
        }

    }
}