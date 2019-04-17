function ActionDeactive()
{
    var testId = Xrm.Page.data.entity.getId();
    ExecuteDeactiveEntitys(testId);
}

function ExecuteDeactiveEntitys(testId)
{
    var ref = confirm("Вы действительно хотите деактивировать все связанные записи Child Entity?");
    if (ref)
    {
        item = testId.replace("{", "");
        item = item.replace("}", "");
        item = item.toLowerCase();
        var organizationUrl = Xrm.Page.context.getClientUrl();
        var data = {

            "InputColectinEntity": item
        };
        
        var query = "new_PinkActionSecond";
        var req = new XMLHttpRequest();
        req.open("POST", organizationUrl + "/api/data/v8.0/" + query, true);
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.setRequestHeader("OData-MaxVersion", "4.0");
        req.setRequestHeader("OData-Version", "4.0");
        req.onreadystatechange = function () {
            if (this.readyState == 4) {
                req.onreadystatechange = null;
                if (this.status == 200) {
                    var data = JSON.parse(this.response);

                } else {
                    var error = JSON.parse(this.response).error;
                    alert(error.message);
                }
            }
        };
        req.send(window.JSON.stringify(data));
    }
}