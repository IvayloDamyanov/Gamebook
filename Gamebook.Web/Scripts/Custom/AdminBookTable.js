(function () {
    var assetListVM;
    assetListVM = {
        dt: null,

        init: function () {
            dt = $('#books-data-table').DataTable({
                "serverSide": true,
                "processing": true,
                "ajax": { "url": "bookTable" },
                "columns": [
                    { "title": "Id", "data": "Id", "searchable": true },
                    { "title": "Title", "data": "Title", "searchable": true },
                    { "title": "Resume", "data": "Resume", "searchable": true },
                    { "title": "Cat. Num", "data": "CatalogueNumber", "searchable": true },
                    { "title": "CreatedOn", "data": "CreatedOn", "searchable": true },
                    { "title": "ModifiedOn", "data": "ModifiedOn", "searchable": true },
                    { "title": "isDeleted", "data": "isDeleted", "searchable": true },
                    { "title": "DeletedOn", "data": "DeletedOn", "searchable": true }

                ],
                "lengthMenu": [[3, 10, 25, 50, 100], [3, 10, 25, 50, 100]],
                });
        }
    }

    // initialize the datatables
    assetListVM.init();
})();