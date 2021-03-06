using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Web.Application.Application.Helpers;
using Dfe.Rscd.Web.Application.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.Rscd.Web.Application.Application
{
    public class TasksReviewedFilterAttribute : ActionFilterAttribute
    {
        private const string TASK_LIST = "task-list-{0}";

        private readonly List<string> _allowedActions;

        public TasksReviewedFilterAttribute(string allowedActions)
        {
            _allowedActions = allowedActions
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim())
                .ToList();

        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (_allowedActions.All(a => !a.Equals(context.RouteData.Values["action"].ToString(), StringComparison.InvariantCultureIgnoreCase)))
            {
                var checkingWindow = CheckingWindowHelper.GetCheckingWindow(context.RouteData);
                var userId = ClaimsHelper.GetUserId(context.HttpContext.User) + checkingWindow.ToString();
                var viewModel = context.HttpContext.Session.Get<TaskListViewModel>(string.Format(TASK_LIST, userId));
                if (viewModel == null || !viewModel.ReviewChecked)
                {
                    context.Result = new RedirectToActionResult("Index", "TaskList", null);
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
