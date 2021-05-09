using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly ICompanyNoteService _companyNoteService;
        private readonly ICompanyTaskService _companyTaskService;
        private readonly IQuoteService _quoteService;

        public UserController(
            INotificationHandler<DomainNotification> notifications,
            IUserService userService,
            IProductService productService,
            ICompanyNoteService companyNoteService,
            ICompanyTaskService companyTaskService,
            IQuoteService quoteService) : base(notifications){
            _userService = userService;
            _productService = productService;
            _companyNoteService = companyNoteService;
            _companyTaskService = companyTaskService;
            _quoteService = quoteService;
        }

        [HttpGet]
        [Route("users/page={page:int:min(1)}/pageSize={pageSize:int:max(10)}/status={status:int}/name")]
        public IActionResult GetUsersByStatus(int page, int pageSize, string name, int status){
            var result = _userService.GetPersonsByStatus(page, pageSize, name, (StatusType) status);
            var count = _userService.CountByUserTypeAndStatus(UserType.Person, StatusType.Active, name);
            var model = new PaginatedItemsViewModel<UserDto>(
                page, pageSize, count, result);

            return Response(model);
        }

        [HttpGet]
        [Route("users/{id}/products/page={page:int:min(1)}/pageSize={pageSize:int:max(28)}")]
        public IActionResult GetProductsByUserId(int id, int page, int pageSize, string name, int status){
            var result = _productService.GetProductsByUserId(id, page, pageSize, name, (StatusType) status);
            var count = _productService.CountProductsByUserId(id, name, (StatusType) status);
            var model = new PaginatedItemsViewModel<UserProductDto>(
                page, pageSize, count, result);
            return Response(model);
        }

        [HttpGet]
        [Route("users/{id}/company-notes/page={page:int:min(1)}/pageSize={pageSize:int:max(10)}")]
        public IActionResult GetCompanyNotesByUserId(int id, int page, int pageSize, int? status){
            var result = _companyNoteService.GetCompanyNotesByUserId(id, page, pageSize, status);
            var count = _companyNoteService.CountCompanyNotesByUserId(id, status);
            var model = new PaginatedItemsViewModel<CompanyNoteDto>(
                page, pageSize, count, result);
            return Response(model);
        }

        [HttpGet]
        [Route("users/{id}/company-tasks/page={page:int:min(1)}/pageSize={pageSize:int:max(10)}")]
        public IActionResult GetCompanyTasksByUserId(int id, int page, int pageSize, int status){
            var result = _companyTaskService.GetCompanyTasksByUserId(id, page, pageSize, (StatusType) status);
            var count = _companyTaskService.CountCompanyTasksByUserId(id, (StatusType) status);
            var model = new PaginatedItemsViewModel<UserTaskDto>(
                page, pageSize, count, result);
            return Response(model);
        }

        [HttpGet]
        [Route("users/{id}/buyer-quotes/page={page:int:min(1)}/pageSize={pageSize:int:max(10)}")]
        public IActionResult GetQuotesByBuyerUserId(int id, int page, int pageSize, int quoteStatus, int orderStatus){
            var result = _quoteService.GetQuotesByUserId(page, pageSize, (QuoteStatusType) quoteStatus,
                (OrderStatusType) orderStatus, null, id);
            var count = _quoteService.CountQuotesByUserId((QuoteStatusType) quoteStatus, (OrderStatusType) orderStatus,
                null, id);
            var model = new PaginatedItemsViewModel<QuoteListDto>(
                page, pageSize, count, result);
            return Response(model);
        }

        [HttpGet]
        [Route("users/{id}/seller-quotes/page={page:int:min(1)}/pageSize={pageSize:int:max(10)}")]
        public IActionResult GetQuotesBySellerUserId(int id, int page, int pageSize, int quoteStatus, int orderStatus){
            var result = _quoteService.GetQuotesByUserId(page, pageSize, (QuoteStatusType) quoteStatus,
                (OrderStatusType) orderStatus, id);
            var count = _quoteService.CountQuotesByUserId((QuoteStatusType) quoteStatus, (OrderStatusType) orderStatus, id);
            var model = new PaginatedItemsViewModel<QuoteListDto>(
                page, pageSize, count, result);
            return Response(model);
        }

        [HttpGet]
        [Route("users/admin-users")]
        public IActionResult GetAdminUsers(int status){
            return Response(_userService.GetAdminUsers((StatusType) status));
        }

        [HttpGet]
        [Route("users/{id:int:min(1)}")]
        public IActionResult GetUserById(int id){
            var result = _userService.GetUserById(id);
            return Response(result);
        }

        [HttpGet]
        [Route("users/{email}")]
        public IActionResult GetUserByEmail(string email){
            var result = _userService.GetUserByEmail(email);
            return Response(result);
        }

        [HttpGet]
        [Route("users/{id}/dashboard")]
        public IActionResult GetUserDashboard(int id){
            return Response(_userService.GetUserDashboardById(id));
        }

        [HttpGet]
        [Route("users/{id}/admin-dashboard")]
        public IActionResult GetAdminUserDashboard(int id){
            return Response(_userService.GetAdminUserDashboardById(id));
        }

        [HttpGet]
        [Route("users/{id}/todo-list")]
        public IActionResult GetTodoList(int id){
            return Response(_userService.GetTodoListById(id));
        }

        [HttpGet]
        [Route("users/{id}/notification-counts")]
        public IActionResult GetNotificationCountById(int id){
            return Response(_quoteService.GetNotificationCountByUserId(id));
        }

        [HttpPost]
        [Route("users/{id:int:min(1)}/upload-profile-image")]
        public IActionResult ChangeProfileImage(IFormFile files, int id){
            _userService.UploadProfileImage(files, id);
            return Response(null, "Image has been uploded");
        }

        [HttpPatch]
        [Route("users/{id:int:min(1)}/password")]
        public IActionResult ChangePassword(int id, [FromBody] UserChangePasswordViewModel model){
            _userService.ChangePassword(id, model);
            return Response(null, "Password has been changed");
        }

        [HttpPatch]
        [Route("users/{id:int:min(1)}/email")]
        public IActionResult ChangeEmail(int id, [FromBody] UserChangeEmailViewModel model){
            _userService.ChangeEmail(id, model);
            return Response(null, "Email has been changed");
        }

        [HttpPut]
        [Route("users/{id:int:min(1)}")]
        public IActionResult UpdateUser(int id, [FromBody] PersonViewModel model){
            _userService.UpdateUser(id, model);
            return Response(null, "User has been updated");
        }

        [HttpPost]
        [Route("users/send-activation-email")]
        public IActionResult SendActivationEmail([FromBody] UserSendActivationEmailViewModel model){
            _userService.SendActionEmail(model);
            return Response(null, "Email has been sent");
        }
    }
}