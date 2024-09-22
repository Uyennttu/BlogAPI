using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MyBlogAPI.Data;
using MyBlogAPI.DTO;
using MyBlogAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlogAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlogController : ControllerBase
	{
		private readonly BlogContext _context;		

		public BlogController(BlogContext context)
		{
			_context = context;
		}
		// GET: api/<BlogController>
		[HttpGet]
		public async Task<IEnumerable<BlogDTO>> GetAllBlogs()
		{
			var blogList = await _context.Blogs.ToListAsync();
			var blogDTOList = blogList.Select(blog => new BlogDTO
			{
				Title = blog.Title,
				Content = blog.Content,
			});
			return blogDTOList;
		}

		// GET api/<BlogController>/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Blog>> GetBlogById(int id)
		{
			var blogPost = await _context.Blogs.FindAsync(id);
			if (blogPost == null) 
			{ 
				return NotFound(); 
			}
			return blogPost;
		}
		

		// POST api/<BlogController>
		[HttpPost]
		public async Task<ActionResult<Blog>> CreateBlog(BlogDTO blog)
		{
			var blogModel =new Blog()
			{
				Title = blog.Title,
				Content = blog.Content				
			};

			_context.Blogs.Add(blogModel);
			await _context.SaveChangesAsync();
			return CreatedAtAction("GetBlogById", new { id = blogModel.Id }, blog);
		}

		// PUT api/<BlogController>/5
		[HttpPut("{id}")]
		public async Task<ActionResult<BlogDTO>> UpdateBlog(int id, BlogDTO blog)
		{
			var existingBlog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
			if (existingBlog == null)
			{
				return NotFound();
			}
			existingBlog.Title = blog.Title;
			existingBlog.Content = blog.Content;
			await _context.SaveChangesAsync();
			return Ok("Update Successfully" + blog);
		}

		// DELETE api/<BlogController>/5
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteBlog(int id)
		{
			var blog = await _context.Blogs.FindAsync(id);
			if (blog == null)
			{
				return NotFound();
			}
			_context.Blogs.Remove(blog);
			await _context.SaveChangesAsync();
			return Ok("Blog deleted");
		}
	}
}
