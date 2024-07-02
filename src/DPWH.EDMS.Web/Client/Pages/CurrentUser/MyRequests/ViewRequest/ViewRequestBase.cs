﻿using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.View.RequestDetailsOverview;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.MyRequests.ViewRequest;

public class ViewRequestBase : RequestDetailsOverviewBase
{
    protected override void OnParametersSet()
    {
        CancelReturnUrl = "/my-requests";
    }

    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;

        await LoadData((res) =>
        {
            SelectedRecordRequest = res;
            BreadcrumbItems.AddRange(new List<BreadcrumbModel>
            {
                new BreadcrumbModel
                {
                    Icon = "menu",
                    Text = "My Requests",
                    Url = "/my-requests",
                },
                new BreadcrumbModel
                {
                    Icon = "create_new_folder",
                    Text = GenericHelper.GetDisplayValue(SelectedRecordRequest.ControlNumber.ToString()),
                    Url = NavManager.Uri.ToString(),
                },
            });

            StateHasChanged();
        });

        IsLoading = false;
        StateHasChanged();
    }
}
