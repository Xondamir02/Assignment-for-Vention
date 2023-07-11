using BlogApi.Context;
using BlogApi.Entities;
using BlogApi.Models.BlogModels;
using BlogApi.Providers;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Managers;

public class BlogManager
{
    private readonly AppDbContext _dbContext;
    private readonly UserProvider _userProvider;

    public BlogManager(AppDbContext dbContext, UserProvider userProvider)
    {
        _dbContext = dbContext;
        _userProvider = userProvider;
    }

    public async Task<List<BlogModel>> GetBlogs()
    {
        var blogs = await _dbContext.Blogs.ToListAsync();

        return ParseList(blogs);
    }

    public async Task<BlogModel> GetBlogById(Guid id)
    {
        var blog = IsExist(id);

        return ParseToBlogModel(blog);
    }
    public async Task<BlogModel> CreateBlog(CreateBlogModel model)
    {
        var blog = new Blog()
        {
            Name = model.Name,
            Description = model.Description,
            UserId = _userProvider.UserId
        };
        _dbContext.Blogs.Add(blog);
        await _dbContext.SaveChangesAsync();
        return ParseToBlogModel(blog);
    }
    public async Task<BlogModel> UpdateBlog(Guid blogId, CreateBlogModel model)
    {
        var blog = IsExist(blogId);
        blog.Name = model.Name;
        blog.Description = model.Description;
        await _dbContext.SaveChangesAsync();
        return ParseToBlogModel(blog);
    }

    public async Task<string> DeleteBlog(Guid blogId)
    {
        var blog = IsExist(blogId);
        _dbContext.Blogs.Remove(blog);
        await _dbContext.SaveChangesAsync();
        return "Done :)";
    }

    private BlogModel ParseToBlogModel(Blog blog)
    {
        var blogModel = new BlogModel()
        {
            Id = blog.Id,
            Name = blog.Name,
            Description = blog.Description!,
            CreatedDate = blog.CreatedDate,
            UserId = blog.UserId,
            UserName = _userProvider.UserName,
            Posts = blog.Posts,
        };
        return blogModel;
    }


    private List<BlogModel> ParseList(List<Blog> blogs)
    {
        var blogModels = new List<BlogModel>();
        foreach (var blog in blogs)
        {
            blogModels.Add(ParseToBlogModel(blog));
        }
        return blogModels;
    }

    private Blog IsExist(Guid blogId)
    {
        var blog = _dbContext.Blogs.FirstOrDefault(b => b.Id == blogId);
        if (blog == null) throw new Exception("Not found");
        return blog;
    }
}