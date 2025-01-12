using KCL_Web.Server.Dtos.Comment;
using KCL_Web.Server.Interfaces;
using KCL_Web.Server.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KCL_Web.Server.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // https://www.youtube.com/watch?v=1i_WE4aGVLU&list=PL82C6-O4XrHfrGOCPmKmwTO7M0avXyQKc&index=17
            // if (!ModelState.IsValid){
            //     return BadRequest(ModelState);
            // }

            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(s => s.ToCommentDto());
            return Ok(commentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [Authorize]
        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
        {
            if (!await _stockRepo.StockExists(stockId))
            {
                return BadRequest("Stock does not exist");
            }

            var CommentModel = commentDto.ToCommentFromCreateDto(stockId);
            await _commentRepo.CreateAsync(CommentModel);
            return CreatedAtAction(nameof(GetById), new { id = CommentModel.Id }, CommentModel.ToCommentDto());
        }

        [Authorize]
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateComment)
        {
            var comment = await _commentRepo.UpdateAsync(id, updateComment.ToCommentFromUpdateDto(id));
            if (comment == null)
            {
                return null;
            }

            return Ok(comment.ToCommentDto());
        }

        [Authorize]
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var commentModel = await _commentRepo.DeleteAsync(id);

            if (commentModel == null)
            {
                return NotFound("Comment dost not exist");
            }
            return Ok(commentModel);
        }
    }
}