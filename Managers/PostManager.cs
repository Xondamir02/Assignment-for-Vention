using BlogApi.Context;
using BlogApi.Entities;
using BlogApi.Models.CommentModels;
using BlogApi.Models.PostModels;
using BlogApi.Providers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Hosting;

namespace BlogApi.Managers;

public class PostManager
{
    private readonly AppDbContext _dbContext;
    private readonly UserProvider _userProvider;
    private readonly BlogManager _blogManager;
    public PostManager(AppDbContext dbContext, UserProvider userProvider, BlogManager blogManager)
    {
        _dbContext = dbContext;
        _userProvider = userProvider;
        _blogManager = blogManager;
    }

    //public async Task<List<PostModel>> GetPosts()
    //{
    //    var posts = await _dbContext.Posts.ToListAsync();
    //    return ParseList(posts);
    //}

    
    public async Task<List<PostModel>> GetPosts(Guid blogId)
    {
        var posts = await _dbContext.Posts.Where(p => p.BlogId ==  blogId).ToListAsync();
        return ParseList(posts);
    }

    public async Task<PostModel> GetPostById(Guid postId)
    {
        var post = IsExist(postId);
        return ParsePost(post);
    }

    public async Task<PostModel> CreatePost(Guid blogId,CreatePostModel model)
    {
        var post = new Post()
        {
            Title = model.Title,
            Description = model.Description,
            BlogId = blogId
        };
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();
        return ParsePost(post);
    }

    public async Task<PostModel> UpdatePost(Guid postId, CreatePostModel model)
    {
        var post = IsExist(postId);
        post.Title = model.Title;
        post.Description = model.Description;
        post.UpdatedDate = DateTime.Now;
        await _dbContext.SaveChangesAsync();
        return ParsePost(post);
    }

    public async Task<string> DeletePost(Guid postId)
    {
        var post = IsExist(postId);
        _dbContext.Posts.Remove(post);
        await _dbContext.SaveChangesAsync();
        return "Done :)";
    }


   
    private PostModel ParsePost(Post model)
    {
       
        var postModel = new PostModel()
        {
            PostId = model.PostId,
            Title = model.Title,
            Description = model.Description,
            CreatedDate = model.CreatedDate,
            BlogId = model.BlogId,
           
        };

        return postModel;
    }
    private List<PostModel> ParseList(List<Post> posts)
    {
        var postModels = new List<PostModel>();
        foreach (var post in posts)
        {
            postModels.Add(ParsePost(post));
        }
        return postModels;
    }

    private Post IsExist(Guid postId)
    {
        var post = _dbContext.Posts.FirstOrDefault(p => p.PostId == postId );
        if (post == null) throw new Exception("Not found");
        return post;
    }

    public async Task<List<Comment>> GetComments(Guid blogId, Guid postId)
    {
        var blog = await GetBlogById(blogId);
        var post = await GetPostByBlog(blog, postId);
        return post.Comments.Where(p => p.PostId == postId).ToList();
    }

    public async Task<Comment> CreateComment(Guid postId, CreateCommentModel model)
    {
        var comment = new Comment()
        {
            PostId = postId,
            UserId = _userProvider.UserId,
            CreatedDate = DateTime.Now,
            Text = model.Message
        };
        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment> UpdateComment(Guid postId,Guid commentId, CreateCommentModel model)
    {
        var comment = await IsExistComment(postId,commentId);
        comment.Text = model.Message;
        _dbContext.Comments.Update(comment);
        return comment;
    }

    public async Task<string> DeleteComment(Guid blogId, Guid postId, Guid commentId)
    {
        var comment = await IsExistComment(postId, commentId);

        _dbContext.Comments.Remove(comment);
        return "All done :)";
    }
    private  async Task<Comment> IsExistComment(Guid postId,Guid commentId)
    {
        var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.PostId == postId && c.Id==commentId);
        if (comment == null) throw new Exception("Comment Not found");
        return comment;
    }

    private async Task<Blog?> GetBlogById(Guid blogId)
    {

        return await _dbContext.Blogs.FirstOrDefaultAsync(b => b.Id == blogId);
    }

    private async Task<Post> GetPostByBlog(Blog blog, Guid postId)
    {
        var post = blog.Posts.FirstOrDefault(p => p.PostId == postId);
        if (post != null) return post;
        throw new Exception("Post Not found");
    }
}