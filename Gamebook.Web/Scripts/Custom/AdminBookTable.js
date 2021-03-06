﻿var bookListVM;
(function () {
    bookListVM = {
        dt: null,

        init: function () {
            dt = $('#books-data-table').DataTable({
                "serverSide": true,
                "processing": true,
                "ajax": { "url": "bookTable" },
                "columns": [
                    { "title": "Title", "data": "Title", "searchable": true },
                    { "title": "Resume", "data": "Resume", "searchable": true },
                    { "title": "Cat. Num", "data": "CatalogueNumber", "searchable": true },
                    { "title": "Created", "data": "CreatedOn", "searchable": true },
                    { "title": "Modified", "data": "ModifiedOn", "searchable": true },
                    { "title": "isDeleted", "data": "isDeleted", "searchable": true },
                    { "title": "Deleted", "data": "DeletedOn", "searchable": true },
                    { "title": "Author", "data": "AuthorUsername", "searchable": true }
                ],
                "lengthMenu": [[3, 10, 25, 50, 100], [3, 10, 25, 50, 100]],
                });
        },
        refresh: function () {
            dt.ajax.reload();
        }
    }

    // initialize the datatables
    bookListVM.init();
})();

$("#btnCreateBook").on("click", function () {

    var url = $(this).data("url");

    $.get(url, function (data) {
        $('#createBookContainer').html(data);

        $('#createBookModal').modal('show');
    });

});

function CreateBookSuccess(data) {
    if (data != "success") {
        $('#createBookContainer').html(data);
        return;
    }
    $('#createBookModal').modal('hide');
    $('#createBookContainer').html("");
    $('#createdBookContainer').html(data);
    $('#createdBookContainer').show();
    setTimeout(function () {
        $('#createdBookContainer').hide();
    }, 5000);
    bookListVM.refresh();
};