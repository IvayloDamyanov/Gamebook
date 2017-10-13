var pageListVM;
(function () {
    pageListVM = {
        dt: null,

        init: function () {
            dt = $('#pages-data-table').DataTable({
                "serverSide": true,
                "processing": true,
                "ajax": { "url": "pageTable" },
                "columns": [
                    { "title": "Book Cat Num", "data": "BookCatNum", "searchable": true },
                    { "title": "Number", "data": "Number", "searchable": true },
                    { "title": "Text", "data": "Text", "searchable": true },
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
    pageListVM.init();
})();

$("#btnCreatePage").on("click", function () {

    var url = $(this).data("url");

    $.get(url, function (data) {
        $('#createPageContainer').html(data);

        $('#createPageModal').modal('show');
    });

});

function CreatePageSuccess(data) {
    if (data != "success") {
        $('#createPageContainer').html(data);
        return;
    }
    $('#createPageModal').modal('hide');
    $('#createPageContainer').html("");
    $('#createdPageContainer').html(data);
    $('#createdPageContainer').show();
    setTimeout(function () {
        $('#createdPageContainer').hide();
    }, 5000);
    pageListVM.refresh();
};