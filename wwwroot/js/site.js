// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('.sort').on("click", sortOrder);
$(document).ready(btnActive);
$(document).ready($('select').addClass('form-select filter-by'));


function sortOrder() {
    var sortby = $.trim(this.innerText);
    var controller;
    var des = false;
    var btn = $(this).attr('class').split(/\s+/);
    if (btn.includes('descending')) {
        des = true;
    }
    if (btn.includes('Provider')) {
        controller = 'Provider';
    }
    if (btn.includes('OrderItem')) {
        controller = 'OrderItem';
    }
    if (btn.includes('Order')) {
        controller = 'Order';
    }
    $.ajax({
        url: "/" + controller + "/Sortby",
        data: {
            sortBy: sortby,
            descending: des,
            numbers: getUrlParameter('Numbers'),
            filterStartDate: getUrlParameter('FilterStartDate'),
            filterEndDate: getUrlParameter('FilterEndDate'),
            providerIds: getUrlParameter('ProviderIds'),
            names: getUrlParameter('Names'),
            units: getUrlParameter('Units')
        },
        traditional: true,    
        success: function (data) {
            $('.main-content').html($.parseHTML(data));
            descendingBtn(sortby, des);
        }
    });
}

function descendingBtn(sortby, descending) {
    var btn = $('.sort:contains(' + sortby + ')');
    if (descending) {
        btn.removeClass('descending');
        btn.prepend('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-down" viewBox="0 0 16 16"> <path fill-rule="evenodd" d="M8 1a.5.5 0 0 1 .5.5v11.793l3.146-3.147a.5.5 0 0 1 .708.708l-4 4a.5.5 0 0 1-.708 0l-4-4a.5.5 0 0 1 .708-.708L7.5 13.293V1.5A.5.5 0 0 1 8 1z"/> </svg>');
    }
    else {
        btn.addClass('descending');
        btn.prepend('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-up" viewBox="0 0 16 16"> <path fill-rule="evenodd" d="M8 15a.5.5 0 0 0 .5-.5V2.707l3.146 3.147a.5.5 0 0 0 .708-.708l-4-4a.5.5 0 0 0-.708 0l-4 4a.5.5 0 1 0 .708.708L7.5 2.707V14.5a.5.5 0 0 0 .5.5z"/> </svg>');
    }
    $('.sort').on("click", sortOrder);
}


function btnActive() {
    var nametable = $.trim($('.name-table').text());
    if (nametable == '') {
        nametable = $.trim($('h4').text());
        if (nametable === 'Order') {
            nametable = 'Orders';
        }
    }
    $('.name-nav:contains(' + nametable + ')').addClass('active');
}

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&');
    var result = [];
    sURLVariables = sURLVariables.filter((el) => el.includes(sParam));
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=')[1];
        result.push(sParameterName);
    }
    console.log(result);
    return result;
};

