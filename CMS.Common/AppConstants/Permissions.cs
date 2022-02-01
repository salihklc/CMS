using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CMS.Common.AppConstants
{
    public enum Permissions
    {
        NoPermission = 0,

        #region user operations   
       
        [Display(GroupName = "User", Name = "Create users", Description = "Can Create User")]
        Create_User = 1,
        [Display(GroupName = "User", Name = "Read users", Description = "Can list User")]
        Read_User = 2,
        [Display(GroupName = "User", Name = "Edit users", Description = "Can Edit User")]
        Edit_User = 3,
        [Display(GroupName = "User", Name = "Delete users", Description = "Can Delete User")]
        Delete_User = 4,
        #endregion
        #region role operations       
        [Display(GroupName = "Role", Name = "Create Roles", Description = "Can Create Roles")]
        Create_Role = 5,
        [Display(GroupName = "Role", Name = "Read Roles", Description = "Can list Roles")]
        Read_Role = 6,
        [Display(GroupName = "Role", Name = "Edit Roles", Description = "Can Edit Roles")]
        Edit_Role = 7,
        [Display(GroupName = "Role", Name = "Delete Roles", Description = "Can Delete Roles")]
        Delete_Role = 8,
        #endregion
        #region ticket 
        [Display(GroupName = "Ticket", Name = "Create Tickets", Description = "Can Create Tickets")]
        Create_Ticket = 9,
        [Display(GroupName = "Ticket", Name = "Create Tickets", Description = "Can Create Tickets")]
        Read_Ticket = 10,
        [Display(GroupName = "Ticket", Name = "Create Tickets", Description = "Can Create Tickets")]
        Edit_Ticket = 11,
        [Display(GroupName = "Ticket", Name = "Create Tickets", Description = "Can Create Tickets")]
        Delete_Ticket = 12,
        [Display(GroupName = "Ticket", Name = "Comment Tickets", Description = "Can Comments Tickets")]
        Comment_Ticket,
        [Display(GroupName = "Ticket", Name = "Assigne Tickets", Description = "Can Assignee Tickets")]
        Assignee_Ticket,
        [Display(GroupName = "Ticket", Name = "Attach Tickets", Description = "Can Attach File Tickets")]
        Attach_Ticket,
        [Display(GroupName = "Ticket", Name = "Reopen Tickets", Description = "Can Reopen Tickets")]
        Reopen_Ticket,
        [Display(GroupName = "Ticket", Name = "Change State Tickets", Description = "Can Reopen Tickets")]
        ChangeState_Ticket,
        [Display(GroupName = "Ticket", Name = "Change State Tickets", Description = "Can Add Log Time Tickets")]
        LogTime_Ticket,
        [Display(GroupName = "Ticket", Name = "Change Description Tickets", Description = "Can Change Description Tickets")]
        ChangeDescription_Ticket,
        [Display(GroupName = "Ticket", Name = "Change Labels Tickets", Description = "Can Add Log Time Tickets")]
        ChangeLabel_Ticket,
        [Display(GroupName = "Ticket", Name = "Change Labels Tickets", Description = "Can Add Log Time Tickets")]
        Definitons_Ticket,
        #endregion
        #region TicketLabel
        [Display(GroupName = "Ticket", Name = "Read Labels Tickets", Description = "Can Add Log Time Tickets")]
        Read_TicketLabel,
        [Display(GroupName = "Ticket", Name = "Create Labels Tickets", Description = "Can Add Log Time Tickets")]
        Create_TicketLabel,
        [Display(GroupName = "Ticket", Name = "Edit Labels Tickets", Description = "Can Add Log Time Tickets")]
        Edit_TicketLabel,
        #endregion
        #region TicketPriorities
        [Display(GroupName = "Ticket", Name = "Read Labels Tickets", Description = "Can Add Log Time Tickets")]
        Read_TicketPriorities,
        [Display(GroupName = "Ticket", Name = "Create Labels Tickets", Description = "Can Add Log Time Tickets")]
        Create_TicketPriorities,
        [Display(GroupName = "Ticket", Name = "Edit Labels Tickets", Description = "Can Add Log Time Tickets")]
        Edit_TicketPriorities,
        #endregion
        #region TicketTypes
        [Display(GroupName = "Ticket", Name = "Read Ticket Types", Description = "Can Add Log Time Tickets")]
        Read_TicketTypes,
        [Display(GroupName = "Ticket", Name = "Create Ticket Types", Description = "Can Add Log Time Tickets")]
        Create_TicketTypes,
        [Display(GroupName = "Ticket", Name = "Edit Ticket Types", Description = "Can Add Log Time Tickets")]
        Edit_TicketTypes,
        #endregion
        #region TicketStatusCategories
        [Display(GroupName = "Ticket", Name = "Read Ticket Types", Description = "Can Add Log Time Tickets")]
        Read_TicketStatusCategories,
        [Display(GroupName = "Ticket", Name = "Create Ticket Types", Description = "Can Add Log Time Tickets")]
        Create_TicketStatusCategories,
        [Display(GroupName = "Ticket", Name = "Edit Ticket Types", Description = "Can Add Log Time Tickets")]
        Edit_TicketStatusCategories,
        #endregion
        #region TicketStatus
        [Display(GroupName = "Ticket", Name = "Read Ticket Types", Description = "Can Add Log Time Tickets")]
        Read_TicketStatus,
        [Display(GroupName = "Ticket", Name = "Create Ticket Types", Description = "Can Add Log Time Tickets")]
        Create_TicketStatus,
        [Display(GroupName = "Ticket", Name = "Edit Ticket Types", Description = "Can Add Log Time Tickets")]
        Edit_TicketStatus,
        #endregion
        [Display(GroupName = "Ticket", Name = "Edit Ticket Types", Description = "Can Add Log Time Tickets")]
        Change_TicketInfos,
        #region WorkingTypes
        [Display(GroupName = "Ticket", Name = "Read Ticket Types", Description = "Can Add Log Time Tickets")]
        Read_WorkingTypes,
        [Display(GroupName = "Ticket", Name = "Create Ticket Types", Description = "Can Add Log Time Tickets")]
        Create_WorkingTypes,
        [Display(GroupName = "Ticket", Name = "Edit Ticket Types", Description = "Can Add Log Time Tickets")]
        Edit_WorkingTypes,
        #endregion
        #region DeleteDefinitions
        [Display(GroupName = "Ticket", Name = "Delete Ticket Types", Description = "Can Add Log Time Tickets")]
        Delete_TicketTypes,
        [Display(GroupName = "Ticket", Name = "Delete Ticket Types", Description = "Can Add Log Time Tickets")]
        Delete_TicketPriorities,
        [Display(GroupName = "Ticket", Name = "Read Ticket Types", Description = "Can Add Log Time Tickets")]
        Delete_TicketStatusCategories,
        [Display(GroupName = "Ticket", Name = "Read Ticket Types", Description = "Can Add Log Time Tickets")]
        Delete_TicketStatus,
        [Display(GroupName = "Ticket", Name = "Read Ticket Types", Description = "Can Add Log Time Tickets")]
        Delete_TicketInfos,
        [Display(GroupName = "Ticket", Name = "Read Ticket Types", Description = "Can Add Log Time Tickets")]
        Delete_WorkingTypes,
        [Display(GroupName = "Ticket", Name = "Read Ticket Types", Description = "Can Add Log Time Tickets")]
        Delete_TicketLabels,
        #endregion
        [Display(GroupName = "Firm", Name = "Read Ticket Types", Description = "Can Add Log Time Tickets")]
        Read_TicketWatchers,
        [Display(GroupName = "Firm", Name = "Create Ticket Types", Description = "Can Add Log Time Tickets")]
        Create_TicketWatchers,
        [Display(GroupName = "Firm", Name = "Edit Ticket Types", Description = "Can Add Log Time Tickets")]
        Edit_TicketWatchers,
        #region Firms
        [Display(GroupName = "Firms", Name = "Read Firms", Description = "Can Read Firms")]
        Read_Firms,
        [Display(GroupName = "Firms", Name = "Create Firms", Description = "Can Create Firms")]
        Create_Firms,
        [Display(GroupName = "Firms", Name = "Edit Firms", Description = "Can Edit Firms")]
        Edit_Firms,
        [Display(GroupName = "Firms", Name = "Delete Firms", Description = "Can Delete Firms")]
        Delete_Firms,
        #endregion
        #region Products
        [Display(GroupName = "Product", Name = "Read Product Fields", Description = "Can Add Log Time Tickets")]
        Read_Fields,
        [Display(GroupName = "Product", Name = "Create Product Fields", Description = "Can Add Log Time Tickets")]
        Create_Fields,
        [Display(GroupName = "Product", Name = "Edit Product Fields", Description = "Can Add Log Time Tickets")]
        Edit_Fields,
        [Display(GroupName = "Product", Name = "Edit Product Fields", Description = "Can Add Log Time Tickets")]
        Delete_Fields,
        #endregion
        #region FİrmProducts
        [Display(GroupName = "Firms", Name = "Read Firms", Description = "Can Read Firms")]
        Read_FirmProducts,
        [Display(GroupName = "Firms", Name = "Create Firms", Description = "Can Create Firms")]
        Create_FirmProducts,
        [Display(GroupName = "Firms", Name = "Edit Firms", Description = "Can Edit Firms")]
        Edit_FirmProducts,
        [Display(GroupName = "Firms", Name = "Delete Firms", Description = "Can Delete Firms")]
        Delete_FirmProducts,
        #endregion
        [Display(GroupName = "Firms", Name = "Read Firms", Description = "Can Read Firms")]
        Read_Products,
        [Display(GroupName = "Firms", Name = "Create Firms", Description = "Can Create Firms")]
        Create_Products,
        [Display(GroupName = "Firms", Name = "Edit Firms", Description = "Can Edit Firms")]
        Edit_Products,
        [Display(GroupName = "Firms", Name = "Delete Firms", Description = "Can Delete Firms")]
        Delete_Products,
    }
}
