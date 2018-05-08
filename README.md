# xxx

   
  Immigration: https://blog.csdn.net/dj2008/article/details/23756895

 AJAX: https://www.nuget.org/packages/Microsoft.jQuery.Unobtrusive.Ajax/
 
 Show.cshtml
 
 @{
    ViewBag.Title = Model.Name;
}

<h2>@Model.Name</h2>

<p>@Model.Description</p>


<div id="messages">

</div>

@using Microsoft.AspNet.Identity
@if (Request.IsAuthenticated)

{

    <section>
        

        @using (Ajax.BeginForm("AjaxTest", new { id = 1, email = @User.Identity.GetUserName(),report= @Model.Name},new AjaxOptions
        {
            Confirm = "Are you sure to share this report?",
            HttpMethod = "Post",
            InsertionMode = InsertionMode.Replace,
            UpdateTargetId = "messages",
            
        }
        ))

        {

            <button type="submit">Share</button>
        }

        <script>
            function ajaxComplete() {
                alert("完成");
            }
        </script>
    </section>

    //using (Html.BeginForm("LogOff", "Account", FormMethod.Post, new { id = "logoutForm", @class = "navbar-right" }))
    //{
    //@Html.AntiForgeryToken()

    //<ul class="nav navbar-nav navbar-right">
    //<li>
    //@Html.ActionLink("Hello " + User.Identity.GetUserName() + "!", "Index", "Manage", routeValues: null, htmlAttributes: new { title = "Manage" })
    //</li>
    //<li><a href="javascript:document.getElementById('logoutForm').submit()">Log off</a></li>
    //</ul>
    //}
}

@section Scripts{<script src="~/Scripts/jquery.unobtrusive-ajax.min.js"></script>}


AjaxTest.cshtml:




@if (@Model == "limited")
{

    <div class="alert alert-warning alert-dismissible" role="alert">
        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <strong>Warning!</strong> Limited Sharing Opportunites, 3 Times Sharing Most.
    </div>
}
@if(@Model != "limited"){

<div class="alert alert-success alert-dismissible" role="alert">
    <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
    <strong>Success!</strong> @Model
</div>
}



