using H5Api.DbContexts;
using H5Api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace H5Api.Controllers
{
    [ApiController]
    [EnableCors("allowpaired")]
    // 这样设置路由后，路径为 /Sticky，不区分大小写
    [Route("[controller]")]
    public class StickyController : ControllerBase
    {
        private readonly StickyContext context;

        public StickyController(StickyContext context)
        {
            this.context = context;
        }

        // 此处设置的 Attribute 决定了访问路径，方法名对访问路径没有影响
        [HttpGet]
        public async Task<ActionResult<List<StickyData>>> GetAll()
        {
            return await context.Stickies.ToListAsync();
        }

        [HttpGet("{uid}")]
        public async Task<ActionResult<StickyData>> Get(string uid)
        {
            var first = await context.Stickies.FirstOrDefaultAsync(note => note.Uid == uid);
            return first;
        }

        [HttpPost]
        public async Task<ActionResult<StickyData>> Post(StickyData sticky)
        {
            sticky.Uid = Guid.NewGuid().ToString("N").Substring(0, 20);
            context.Stickies.Add(sticky);
            await context.SaveChangesAsync();
            // 更新成功后将返回更新的对象
            return CreatedAtAction(nameof(Get), new { uid = sticky.Uid, id = sticky.Id }, sticky);
        }

        [HttpPut]
        public async Task<ActionResult<StickyData>> Put(string uid, StickyData sticky)
        {
            if (uid != sticky.Uid)
            {
                return BadRequest();
            }

            context.Entry(sticky).State = EntityState.Modified;

            try
            {
                var first = await context.Stickies.FirstOrDefaultAsync(note => note.Uid == uid);
                if (first == null)
                {
                    return NotFound();
                }

                // 附加到对象后，对对象的更改可以更新到数据库
                context.Attach(sticky);

                // 更新对象内容
                first.Content = sticky.Content;
                first.update_time = DateTime.Now;

                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (context.Stickies.All(e => e.Uid != uid))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult<StickyData>> Delete(string uid)
        {
            try
            {
                var first = await context.Stickies.FirstOrDefaultAsync(note => note.Uid == uid);
                if (first == null)
                {
                    return NotFound();
                }

                context.Remove(first);

                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (context.Stickies.All(e => e.Uid != uid))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
