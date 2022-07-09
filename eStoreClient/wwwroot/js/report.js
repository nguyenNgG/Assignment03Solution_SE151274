$(document).ready(function () {
    let $btnReport = $('#btnReport');
    LoadOrders();

    $btnReport.click(function () {
        $('tbody').empty();
        LoadOrders();
    });
});

function LoadOrders() {
    let $inpFrom = $('#inpFrom');
    let $inpTo = $('#inpTo');
    let $tbody = $('tbody');

    let query = "&$filter=true ";

    let valFrom = $inpFrom.val().trim();
    if (valFrom !== '') {
        valFrom += '%2b07:00';
        query += `and OrderDate ge ${valFrom} `;
    }

    let valTo = $inpTo.val().trim();
    if (valTo !== '') {
        valTo += '%2b07:00';
        query += `and OrderDate le ${valTo} `;
    }

    $.ajax({
        method: 'GET',
        url: `http://localhost:5000/odata/Orders?$expand=Member,OrderDetails${query}`,
        success: function (result, status, xhr) {
            let $orders = $(result.value);

            let periodPrice = 0.0;

            let options = { hour: 'numeric', minute: 'numeric', second: 'numeric' };
            $orders.each(function (index) {
                let $tr = $('<tr>');
                let $tdId = $('<td>').html(this['OrderId']).appendTo($tr);
                let $tdOrderDate = $('<td>').html((new Date(this['OrderDate'])).toLocaleDateString("en-US", options)).appendTo($tr);
                let $tdRequiredDate = $('<td>').html((new Date(this['RequiredDate'])).toLocaleDateString("en-US", options)).appendTo($tr);

                let shippedDateString = this['ShippedDate'];
                let shippedDateText = "Not yet";
                if (shippedDateString !== null) {
                    shippedDateText = (new Date(shippedDateString)).toLocaleDateString("en-US", options);
                }
                let $tdShippedDate = $('<td>').html(shippedDateText).appendTo($tr);
                let $tdFreight = $('<td>').html(this['Freight']).appendTo($tr);
                let $tdMember = $('<td>').html(this['Member']['Email']).appendTo($tr);

                let $orderDetails = $(this['OrderDetails']);
                let orderPrice = 0.0;
                $orderDetails.each(function (index) {
                    orderPrice += this.Quantity * this.UnitPrice - this.Quantity * this.UnitPrice * this.Discount / 100;
                });

                let $tdTotalPrice = $('<td>').html(orderPrice.toFixed(2)).appendTo($tr);
                periodPrice += orderPrice;

                $tr.appendTo($tbody);
            });

            let $trPeriod = $('<tr>');
            for (let i = 0; i < 6; i++) {
                let $tdEmpty = $('<td>').appendTo($trPeriod);
            }
            let $tdPeriodPrice = $('<td>').html(periodPrice.toFixed(2)).appendTo($trPeriod);
            $trPeriod.appendTo($tbody);
        }, error: function (xhr, status, error) {
            console.log(xhr);
        }
    });
}