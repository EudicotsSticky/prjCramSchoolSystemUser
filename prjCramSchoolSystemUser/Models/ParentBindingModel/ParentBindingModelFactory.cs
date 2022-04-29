using Microsoft.AspNetCore.Identity;
using prjCramSchoolSystemUser.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.Models.ParentBindingModel
{
    public class ParentBindingModelFactory
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public ParentBindingModelFactory(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<ParentBindingModel> LoadParentAsync(ApplicationUser user)
        {
            ParentBindingModel Input = new ParentBindingModel();
            Input.Id = user.Id;
            Input.MotherId = user.MotherId;
            Input.FatherId = user.FatherId;
            if (user.FatherId != null)
            {
                user.Father = await _userManager.FindByIdAsync(user.FatherId);
                Input.Father = user.Father;
                Input.FatherName = user.Father.LastName + user.Father.FirstName;
            }
            if (user.MotherId != null)
            {
                user.Mother = await _userManager.FindByIdAsync(user.MotherId);
                Input.Mother = user.Mother;
                Input.MotherName = user.Mother.LastName + user.Mother.FirstName;
            }
            return await Task.Run(() => Input);
        }

        public async Task<ApplicationUser> FindParentAsync(string nameOrEmail)
        {
            ApplicationUser parentData = new ApplicationUser();
            if (IsValidEmail(nameOrEmail))
                parentData = await _userManager.FindByEmailAsync(nameOrEmail);
            else
                parentData = await _userManager.FindByNameAsync(nameOrEmail);
            if (parentData != null)
            {
                if (!await _userManager.IsInRoleAsync(parentData, Enums.Roles.Parent.ToString()))
                    parentData = null;
            }
            return parentData;
        }

        public bool IsValidEmail(string emailAddress)
        {
            if (String.IsNullOrEmpty(emailAddress))
                return false;
            try
            {
                MailAddress m = new MailAddress(emailAddress);
                return true;
            }
            catch (FormatException ex)
            {
                return false;
            }
        }
    }
}
