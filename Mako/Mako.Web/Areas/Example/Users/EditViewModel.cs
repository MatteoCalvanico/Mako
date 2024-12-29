using Mako.Web.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using Mako.Services.Shared;

namespace Mako.Web.Areas.Example.Users
{
    [TypeScriptModule("Example.Users.Server")]
    public class EditViewModel
    {
        public EditViewModel()
        {
        }

        public Guid? Id { get; set; }
        public string Email { get; set; }
        public string Cf {  get; set; }

        public string ToJson()
        {
            return Infrastructure.JsonSerializer.ToJsonCamelCase(this);
        }

        public void SetUser(UserDetailDTO userDetailDTO)
        {
            if (userDetailDTO != null)
            {
                Id = userDetailDTO.Id;
                Email = userDetailDTO.Email;
                Cf = userDetailDTO.Cf;
            }
        }

        public AddOrUpdateUserCommand ToAddOrUpdateUserCommand()
        {
            return new AddOrUpdateUserCommand
            {
                Id = Id,
                Email = Email,
                Cf = Cf
            };
        }
    }
}