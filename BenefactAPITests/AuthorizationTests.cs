using BenefactAPI;
using BenefactAPI.Controllers;
using BenefactAPI.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BenefactAPITests
{
    [TestClass]
    public class AuthorizationTests
    {
        static ControllerContext CreateContext(string body)
        {
            RouteData routeData = new RouteData();
            routeData.Values["boardId"] = "1";
            HttpContext httpContextMock = new DefaultHttpContext();
            var bytes = Encoding.UTF8.GetBytes(body);
            (httpContextMock.Request.Body = new MemoryStream()).Write(bytes, 0, bytes.Length);
            httpContextMock.Request.Body.Position = 0;
            return new ControllerContext()
            {
                RouteData = routeData,
                HttpContext = httpContextMock,
            };
        }
        [TestMethod]
        public async Task CommentingFail()
        {
            MockServiceProvider services = new MockServiceProvider();
            MockData.AddToDb(services);
            BoardExtensions.Board = await services.BoardLookup(1);
            var user = Auth.CurrentUser = await Auth.GetUser(services, "faff@faff.faff");
            user.Roles.Add(new UserBoardRole() { BoardId = 1, UserId = user.Id, BoardRole = new BoardRole() { Privilege = Privilege.Read } });
            var rpc = new BoardController(services);
            rpc.ControllerContext = CreateContext("{\"CardId\": 1, \"Text\": \"This is a test commment!\"}");
            var error = await Assert.ThrowsExceptionAsync<HTTPError>(
                () => rpc.Post("comments/add"));
        }
        [TestMethod]
        public async Task CommentingSuccess()
        {
            MockServiceProvider services = new MockServiceProvider();
            MockData.AddToDb(services);
            BoardExtensions.Board = await services.BoardLookup(1);
            var user = Auth.CurrentUser = await Auth.GetUser(services, "faff@faff.faff");
            user.Roles.Add(new UserBoardRole() { BoardId = 1, UserId = user.Id, BoardRole = new BoardRole() { Privilege = Privilege.Contribute } });
            var rpc = new BoardController(services);
            rpc.ControllerContext = CreateContext("{\"CardId\": 1, \"Text\": \"This is a test commment!\"}");
            var result = await rpc.Post("comments/add");
        }
    }
}
