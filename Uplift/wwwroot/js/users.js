var dataTable;
$(document).ready(function () {
    console.log("here")
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/admin/user/getall",
            "type": "GET",
            "datatype": "json",

        },
        "columns": [
            { "data": "userName", "width": "25%" },
            { "data": "phoneNumber", "width": "25%" },
            { "data": "city", "width": "25%" },
            {
                "data": {
                    id: "id",
                    lockoutEnd: "lockoutEnd"
                }, 
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime()
                    if (lockout > today) {
                        return `
                            <div class="text-center">
                                <a href="/admin/user/UnLock/${data.id}" class="btn btn-success text-white" style="cursor; width: 100px">
                                    <i class="fas fa-lock-open"></i> Unlock
                                </a>
                            </div>
                        `;
                    } else {
                        return `
                            <div class="text-center">
                                <a href="/admin/user/Lock/${data.id}" class="btn btn-danger text-white" style="cursor; width: 100px">
                                    <i class="fas fa-lock"></i> Lock
                                </a>
                            </div>
                        `;
                    }
                }, "width": "25%"
            }
        ],
        "language": {
            "emptyTable": "No Records Found"
        },
        "width":"100%"
    })
}