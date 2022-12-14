using BenefactAPI.Controllers;
using BenefactAPI.DataAccess;
using Replicate;
using Replicate.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BenefactAPI.RPCInterfaces.Board
{
    [ReplicateType]
    [ReplicateRoute(Route = "api/boards/{boardId}/tags")]
    public class TagsInterface
    {
        readonly IServiceProvider Services;
        public TagsInterface(IServiceProvider services)
        {
            Services = services;
        }

        [AuthRequired(RequirePrivilege = Privilege.Admin)]
        public Task<TagData> Add(TagData tag)
        {
            return Services.DoWithDB(async db =>
            {
                tag.Id = 0;
                tag.BoardId = BoardExtensions.Board.Id;
                var result = await db.Tags.AddAsync(tag);
                await db.SaveChangesAsync();
                return result.Entity;
            });
        }

        [AuthRequired(RequirePrivilege = Privilege.Admin)]
        public Task<bool> Delete(IDRequest tag)
        {

            return Services.DoWithDB(
                db => db.DeleteAsync(db.Tags, new TagData() { Id = tag.Id, BoardId = BoardExtensions.Board.Id }),
                false);
        }

        [AuthRequired(RequirePrivilege = Privilege.Admin)]
        public Task Update(TagData tag)
        {
            return Services.DoWithDB(async db =>
            {
                var existingTag = await db.Tags.FindAsync(BoardExtensions.Board.Id, tag.Id);
                if (existingTag == null) throw new HTTPError("Tag not found", 404);
                TypeUtil.CopyFrom(existingTag, tag, whiteList: new[] { nameof(TagData.Name), nameof(TagData.Character), nameof(TagData.Color) });
                await db.SaveChangesAsync();
                return true;
            });
        }
    }
}
