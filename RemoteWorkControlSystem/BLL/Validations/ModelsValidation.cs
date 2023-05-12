using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validations
{

    public static class ModelsValidation
    {
        public static void EmployeeScreenActivityModelValidation(EmployeeScreenActivityModel model)
        {
            if (model == null)
            {
                throw new RemoteWorkControlSystemException("EmployeeScreenActivity was null");
            }

            if (model.ScreenshotURL == null || model.ScreenshotURL == String.Empty)
            {
                throw new RemoteWorkControlSystemException("Wrong screnshotURL");
            }

            if (model.ProjectMemberId < 0)
            {
                throw new RemoteWorkControlSystemException("Wrong ProjectMemberId");
            }

        }

        public static void ProjectMemberModelValidation(ProjectMemberModel model)
        {
            if (model == null)
            {
                throw new RemoteWorkControlSystemException("ProjectMember was null");
            }

            if (model.UserId < 0)
            {
                throw new RemoteWorkControlSystemException("Wrong UserId");
            }

            if (model.ProjectKey == null || model.ProjectKey == String.Empty)
            {
                throw new RemoteWorkControlSystemException("Wrong ProjectKey");
            }

            if (model.ProjectTitle == null || model.ProjectTitle == String.Empty)
            {
                throw new RemoteWorkControlSystemException("Wrong ProjectTitle");
            }

        }


        public static void TaskDurationModelValidation(TaskDurationModel model) {
            
            if (model == null)
            {
                throw new RemoteWorkControlSystemException("ProjectMember was null");
            }

            if (model.SprintId == null || model.SprintId == String.Empty)
            {
                throw new RemoteWorkControlSystemException("Wrong SprintId");
            }

            if (model.TaskId == null || model.TaskId == String.Empty)
            {
                throw new RemoteWorkControlSystemException("Wrong TaskId");
            }

            if(model.TimeSpent < 0f)
            {
                throw new RemoteWorkControlSystemException("Wrong TimeSpent");
            }

            if (model.ProjectMemberId < 0)
            {
                throw new RemoteWorkControlSystemException("Wrong ProjectMemberId");
            }

            if(model.StartDate > model.EndDate)
            {
                throw new RemoteWorkControlSystemException("Wrong Date");
            }

        }

        public static void WorkSessionModelValidation(WorkSessionModel model)
        {
            if (model.ProjectMemberId < 0)
            {
                throw new RemoteWorkControlSystemException("Wrong ProjectMemberId");
            }

            if (model.WorkTime < 0f)
            {
                throw new RemoteWorkControlSystemException("Wrong WorkTime");
            }

            if (model.StartDate > model.EndDate)
            {
                throw new RemoteWorkControlSystemException("Wrong Date");
            }

        }

        public static void UserModelValidation(UserModel model)
        {
            if (model.FirstName == null || model.FirstName == String.Empty)
            {
                throw new RemoteWorkControlSystemException("Wrong FirstName");
            }

            if (model.LastName == null || model.LastName == String.Empty)
            {
                throw new RemoteWorkControlSystemException("Wrong LastName");
            }

            if (model.UserName == null || model.UserName == String.Empty)
            {
                throw new RemoteWorkControlSystemException("Wrong UserName");
            }

            if (model.Password == null || model.Password == String.Empty)
            {
                throw new RemoteWorkControlSystemException("Wrong Password");
            }

            if (model.Email == null || model.Email == String.Empty)
            {
                throw new RemoteWorkControlSystemException("Wrong Email");
            }

            if (model.JiraId == null || model.JiraId == String.Empty)
            {
                throw new RemoteWorkControlSystemException("Wrong JiraId");
            }
        }

        public static void ProjectModelValidation(ProjectModel model)
        {
            if (model.ProjectTitle == null || model.ProjectTitle == String.Empty)
            {
                throw new RemoteWorkControlSystemException("Wrong ProjectTitle");
            }
            if (model.ProjectKey == null || model.ProjectKey == String.Empty)
            {
                throw new RemoteWorkControlSystemException("Wrong ProjectKey");
            }

            if (model.ScreenshotInterval < 0.1f)
            {
                throw new RemoteWorkControlSystemException("Wrong ScreenshotInterval");
            }

        }

    }
}
