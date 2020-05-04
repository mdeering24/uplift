var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/admin/order/getall",
            "type": "GET",
            "datatype": "json",

        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "email", "width": "20%" },
            { "data": "city", "width": "10%" },
            { "data": "orderDate", "width": "15%" },
            { "data": "status", "width": "15%" },
            {
                "data": "id", 
                "render": function (data) {
                    return `
                                <div class="text-center">
                                    <a href="/admin/order/upsert/${data}" class="btn btn-success text-white" style="cursor; width: 50px">
                                        <i class="far fa-edit"></i>
                                    </a>
                                    &nbsp;
                                    <a onclick=Delete("/admin/order/delete/${data}") class="btn btn-danger text-white" style="cursor; width: 50px">
                                        <i class="far fa-trash-alt"></i>
                                    </a>
                                </div>
                            `;
                }, "width": "20%"
            }
        ],
        "language": {
            "emptyTable": "No Records Found"
        },
        "width":"100%"
    })
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    });
}