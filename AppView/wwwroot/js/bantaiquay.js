function addhd() {
    $.ajax({
        async: true,
        type: 'GET',
        dataType: "json",
        data: null,
        url: '/BanHangTaiQuay/TaoHoaDon',
        success: function (response) {
            if (response.success == true) {
                toastr.success(response.message, 'Success Alert', { timeOut: 300 });
                debugger;
            } else {
                toastr.error(response.message, 'Error Alert', { timeOut: 300 });
            }
        },
        error: function (response) {
            console.log(response);
            toastr.error(response.message, 'Error Alert', { timeOut: 300 });
        }
    });
}