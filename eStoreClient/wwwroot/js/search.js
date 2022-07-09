$(document).ready(function () {
    let $btnSearch = $('#btnSearch');
    LoadProducts();

    $btnSearch.click(function () {
        $('tbody').empty();
        LoadProducts();
    });
});

function LoadProducts() {
    let $inpSearch = $('#inpSearch');
    let $inpFrom = $('#inpFrom');
    let $inpTo = $('#inpTo');
    let $tbody = $('tbody');

    let query = "&$filter=true ";

    let valSearch = $inpSearch.val().trim();
    if (valSearch !== '') {
        query += `and contains(tolower(ProductName),tolower('${valSearch}')) `;
    }

    let valFrom = $inpFrom.val().trim();
    if (valFrom !== '') {
        if ($.isNumeric(valFrom)) {
            query += `and UnitPrice ge ${valFrom} `;
        }
    }

    let valTo = $inpTo.val().trim();
    if (valTo !== '') {
        if ($.isNumeric(valTo)) {
            query += `and UnitPrice le ${valTo} `;
        }
    }

    $.ajax({
        method: 'GET',
        url: `http://localhost:5000/odata/Products?$expand=Category${query}`,
        success: function (result, status, xhr) {
            let $products = $(result.value);

            console.log($products);

            $products.each(function (index) {
                let $tr = $('<tr>');
                let $tdId = $('<td>').html(this['ProductId']).appendTo($tr);
                let $tdName = $('<td>').html(this['ProductName']).appendTo($tr);
                let $tdWeight = $('<td>').html(this['Weight'].toFixed(2)).appendTo($tr);
                let $tdPrice = $('<td>').html(this['UnitPrice'].toFixed(2)).appendTo($tr);
                let $tdUnits = $('<td>').html(this['UnitsInStock']).appendTo($tr);
                let $tdCategory = $('<td>').html(this['Category']['CategoryName']).appendTo($tr);
                let $tdAction = $('<td>');
                let $aEdit = $('<a>').html('Edit ').attr('href', `/Products/Edit?id=${this['ProductId']}`).appendTo($tdAction);
                let $aDetails = $('<a>').html('Details ').attr('href', `/Products/Details?id=${this['ProductId']}`).appendTo($tdAction);
                let $aDelete = $('<a>').html('Delete ').attr('href', `/Products/Delete?id=${this['ProductId']}`).appendTo($tdAction);
                $tdAction.appendTo($tr);
                $tr.appendTo($tbody);
            });
        }, error: function (xhr, status, error) {
            console.log(xhr);
        }
    });
}