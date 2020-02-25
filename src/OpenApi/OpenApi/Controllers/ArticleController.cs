using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenApi.Models;
using OpenApi.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace OpenApi.Controllers
{
	/// <summary>
	/// Provides access to articles.
	/// </summary>
	[ApiController]
	[Authorize("api")]
	[ApiVersion("1"), ApiVersion("2"), ApiVersion("3")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class ArticleController : ControllerBase
	{
		private readonly ArticleService _articleService;

		/// <summary>
		/// Initializes a new instance of the <see cref="ArticleController"/>.
		/// </summary>
		/// <param name="articleService">The persistence service for articles.</param>
		public ArticleController(ArticleService articleService)
		{
			_articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
		}

		/// <summary>
		/// Gets all articles available
		/// </summary>
		/// <returns>A list of all available articles.</returns>
		[HttpGet, MapToApiVersion("1")]
		public ActionResult<IEnumerable<Article>> Get()
		{
			if (_articleService.Articles.Count == 0)
				return NotFound();

			return Ok(GetPagedData(0, Int32.MaxValue).Entries);
		}

		/// <summary>
		/// Gets a paged result of articles.
		/// </summary>
		/// <param name="skip">The articles to skip while fetching.</param>
		/// <param name="take">The amount of articles to return.</param>
		/// <returns>A <see cref="PagedResult{T}"/> of articles.</returns>
		[HttpGet, MapToApiVersion("2"), MapToApiVersion("3")]
		[SwaggerOperation(Tags = new[] { "Article", "Changed" })]
		public ActionResult<PagedResult<Article>> GetPaged(int skip = 0, int take = 10)
		{
			if (_articleService.Articles.Count == 0)
				return NotFound();

			return GetPagedData(skip, take);
		}

		/// <summary>
		/// Fetches an article based on its number.
		/// </summary>
		/// <param name="number">The article number.</param>
		/// <returns>The article if found; otherwise, 404.</returns>
		[HttpGet("{number}")]
		[SwaggerResponse(200, "Article was found", typeof(Article))]
		[SwaggerResponse(404, "Article was not found.")]
		public ActionResult<Article> GetByNumber([FromRoute] int number)
		{
			var article = _articleService.Articles.FirstOrDefault(a => a.Number == number);

			if (article == null)
				return NotFound();

			return article;
		}

		/// <summary>
		/// Creates an article.
		/// </summary>
		/// <param name="article">The article to create.</param>
		/// <returns>201, when creation was successful; otherwise, 409</returns>
		[HttpPost]
		[SwaggerResponse(201, "Article successfully created")]
		[SwaggerResponse(409, "Article already existing")]
		public IActionResult Add([FromBody] Article article)
		{
			try
			{
				_articleService.Add(article);
				return Created($"{Request.Path}/{article.Number}", article);
			}
			catch (InvalidOperationException)
			{
				return Conflict();
			}
		}

		/// <summary>
		/// Updates an existing article.
		/// </summary>
		/// <param name="number">The number of the article to update.</param>
		/// <param name="article">The new article data to put instead of the current one.</param>
		/// <returns>Whether the article was successfully updated or not.</returns>
		[HttpPut("{number}")]
		[SwaggerResponse(204, "Article successfully updated")]
		[SwaggerResponse(404, "Article for updating not found")]
		public IActionResult Update([FromRoute] int number, [FromBody] Article article)
		{
			try
			{
				article.Number = number;
				_articleService.Update(article);

				return NoContent();
			}
			catch (InvalidOperationException)
			{
				return NotFound();
			}
		}

		/// <summary>
		/// Deletes an existing article.
		/// </summary>
		/// <param name="number">The number of the article to remove.</param>
		/// <returns>Whether deletion of the article was successful or not.</returns>
		[HttpDelete("{number}")]
		[SwaggerOperation(OperationId = "DeleteArticleByNumber")]
		[SwaggerResponse(204, "Article successfully deleted")]
		[SwaggerResponse(404, "Article for deleting not found")]
		public IActionResult Delete([FromRoute] int number)
		{
			try
			{
				var article = new Article() { Number = number, };
				_articleService.Remove(article);

				Response.Headers.Add("x-deletion-id", Guid.NewGuid().ToString());
				return NoContent();
			}
			catch (InvalidOperationException)
			{
				return NotFound();
			}
		}

		private PagedResult<Article> GetPagedData(int skip, int take)
		{
			return new PagedResult<Article>()
			{
				Entries = _articleService.Articles.Skip(skip).Take(take).ToArray(),
				StartIndex = skip,
				TotalAmount = _articleService.Articles.Count,
			};
		}
	}
}
