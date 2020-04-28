var dataTable;
$(document).ready(function () {
    console.log("here")
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/admin/category/getall",
            "type": "GET",
            "datatype": "json",

        },
        "columns": [
            { "data": "name", "width": "50%" },
            { "data": "displayOrder", "width": "20%" },
            {
                "data": "id", 
                "render": function (data) {
                    return `
                                <div class="text-center">
                                    <a href="/admin/category/upsert/${data}" class="btn btn-success text-white" style="cursor; width: 100px">
                                        <i class="far fa-edit"></i> Edit
                                    </a>
                                    &nbsp;
                                    <a onclick=Delete("/admin/category/delete/${data}") class="btn btn-danger text-white" style="cursor; width: 100px">
                                        <i class="far fa-trash-alt"></i> Delete
                                    </a>
                                </div>
                            `;
                }, "width": "30%"
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