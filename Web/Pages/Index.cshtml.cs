using Contentstack.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Models;

namespace Web.Pages;

public class IndexModel : PageModel
{
    private readonly ContentstackClient _contentstack;
    private readonly ILogger<IndexModel> _logger;

    public List<BlogPost> Posts { get; private set; } = new();
    public string? ErrorMessage { get; private set; }

    public IndexModel(ContentstackClient contentstack, ILogger<IndexModel> logger)
    {
        _contentstack = contentstack;
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        try
        {
            var query = _contentstack.ContentType("blog_post").Query();
            query.IncludeReference("author");

            var result = await query.Find<BlogPost>();
            Posts = result.Items.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao buscar posts no Contentstack");
            ErrorMessage = "Não foi possível carregar os posts no momento.";
        }
    }
}