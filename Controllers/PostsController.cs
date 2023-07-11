using BlogApi.Managers;
using BlogApi.Models.CommentModels;
using BlogApi.Models.PostModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [Route("api/blogs/{blogId}/[controller]")]
    [ApiController]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly PostManager _postManager;

        public PostsController(PostManager postManager)
        {
            _postManager = postManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts(Guid blogId)
        {
            return Ok(await _postManager.GetPosts(blogId));
        }

        /*[HttpGet("blogId")]
        public async Task<IActionResult> GetPosts(Guid blogId)
        {
            return Ok(await _postManager.GetPosts(blogId));
        }*/

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById(Guid postId)
        {
            return Ok(await _postManager.GetPostById(postId));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(Guid blogId, CreatePostModel model)
        {
            return Ok(await _postManager.CreatePost(blogId, model));
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePost( Guid postId, CreatePostModel model)
        {
            return Ok(await _postManager.UpdatePost(postId, model));
        }

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            return Ok(await _postManager.DeletePost( postId));
        }

        [HttpGet("{postId}/comments")]
        public async Task<IActionResult> GetPostComments(Guid blogId, Guid postId)
        {
            return Ok(await _postManager.GetComments(blogId, postId));
        }

        [HttpPost("{postId}/comments")]
        public async Task<IActionResult> CreateComment(Guid postId, CreateCommentModel model)
        {
            return Ok(await _postManager.CreateComment(postId, model));
        }

        [HttpPut("{postId}/comments/{commentId}")]
        public async Task<IActionResult> UpdateComment( Guid postId, Guid commentId,
            CreateCommentModel model)
        {
            return Ok(await _postManager.UpdateComment(postId, commentId, model));
        }

        [HttpDelete("{postId}/comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid blogId, Guid postId, Guid commentId)
        {
            return Ok(await _postManager.DeleteComment(blogId, postId, commentId));
        }

    }
}
