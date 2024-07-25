﻿using DPWH.EDMS.Application.Models.Navigation;
using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.Navigation.Mappers;

public static class MenuItemMappers
{
    public static QueryMenuItemModel Map(MenuItem menuItem)
    {
        return new QueryMenuItemModel
        {
            Id = menuItem.Id,
            Text = menuItem.Text,
            Url = menuItem.Url,
            Icon = menuItem.Icon,
            Expanded = menuItem.Expanded,
            Level = menuItem.Level,
            SortOrder = menuItem.SortOrder,
            AuthorizedRoles = menuItem.AuthorizedRoles,
            ParentId = menuItem.ParentId,
        };
    }
}
