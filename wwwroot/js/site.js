// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('.sort').on("click", sort);

function sort() {
    var sortby = this.innerText;
    $.ajax({
        url: "Order/FilterOrders",
        data: { sortBy: sortby },
        success: result,
    });
    
}
function result(data) {
    $('.main-content').html(data);
    console.log($('.main-content').html);
}
