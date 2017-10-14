var userListVM;
(function () {
    userListVM = {
        dt: null,

        init: function () {
            dt = $('#users-data-table').DataTable({
                "serverSide": true,
                "processing": true,
                "ajax": { "url": "userTable" },
                "columns": [
                    { "title": "UserName", "data": "UserName", "searchable": true },
                    { "title": "Email", "data": "Email", "searchable": true },
                    { "title": "Lockout", "data": "Lockout", "searchable": true },
                    { "title": "Phone", "data": "Phone", "searchable": true },
                    { "title": "Created", "data": "CreatedOn", "searchable": true },
                    { "title": "Modified", "data": "ModifiedOn", "searchable": true },
                    { "title": "isDeleted", "data": "isDeleted", "searchable": true },
                    { "title": "Deleted", "data": "DeletedOn", "searchable": true }
                ],
                "lengthMenu": [[3, 10, 25, 50, 100], [3, 10, 25, 50, 100]],
                });
        },
        refresh: function () {
            dt.ajax.reload();
        }
    }

    // initialize the datatables
    userListVM.init();
})();

$("#btnCreateUser").on("click", function () {

    var url = $(this).data("url");

    $.get(url, function (data) {
        $('#createUserContainer').html(data);

        $('#createUserModal').modal('show');
    });

});

function CreateUserSuccess(data) {
    if (data != "success") {
        $('#createUserContainer').html(data);
        return;
    }
    $('#createUserModal').modal('hide');
    $('#createUserContainer').html("");
    $('#createdUserContainer').html(data);
    $('#createdUserContainer').show();
    setTimeout(function () {
        $('#createdUserContainer').hide();
    }, 5000);
    userListVM.refresh();
};